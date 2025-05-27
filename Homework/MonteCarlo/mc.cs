using static System.Console;
using static System.Math;
using System.Collections.Generic;
using System;

public class mc
{
    public static (double, double) plainmc(Func<vector, double> f, vector a, vector b, int N)
    { // plain Monte Carlo integration
        int dim = a.size; // dimension of the integral
        double V = 1; // volume of the integration domain
        for (int i = 0; i < dim; i++)
        {
            V *= b[i] - a[i]; // calculate the volume of the hyperrectangle
        }
        double sum = 0, sum2 = 0; // sum and sum of squares
        var x = new vector(dim); // point in the integration domain
        var rnd = new Random(); // random number
        for (int i = 0; i < N; i++)
        { // loop over the number of points
            for (int k = 0; k < dim; k++)
            { // loop over the dimensions
                x[k] = a[k] + rnd.NextDouble() * (b[k] - a[k]); // generate a random point in the integration domain
            }
            double fx = f(x); // evaluate the function at the random point
            sum += fx; sum2 += fx * fx; // accumulate the sum and sum of squares
        }
        double mean = sum / N, sigma = Sqrt(sum2 / N - mean * mean); // calculate the mean and standard deviation
        var result = (mean * V, sigma * V / Sqrt(N)); // return the result as a tuple

        return result;
    }

    public static (double, double) quasirandmc(Func<vector, double> f, vector a, vector b, int N)
    { // quasi-random Monte Carlo integration
        int dim = a.size; // dimension of the integral
        double V = 1; // volume of the integration domain
        for (int i = 0; i < dim; i++)
        {
            V *= b[i] - a[i]; // calculate the volume of the hyperrectangle
        }
        double sum = 0, sum2 = 0; // sum and sum of squares
        var h1 = new Halton(dim); // create a Halton sequence generator
        var h2 = new Halton(dim); // create a second Halton sequence generator
        for (int i = 0; i < N; i++)
        { // loop over the number of points
            var x = h1.get(i); // generate the i-th point in the Halton sequence
            var y = h2.get(i); // generate the i-th point in the Halton sequence for the next dimension
            for (int k = 0; k < dim; k++)
            { // loop over the dimensions
                x[k] = a[k] + x[k] * (b[k] - a[k]); // scale the point to the integration domain
                y[k] = a[k] + y[k] * (b[k] - a[k]); // scale the point to the integration domain
            }
            double fx = f(x); // evaluate the function at the random point
            double fy = f(y); // evaluate the function at the random point in the next dimension
            sum += (fx + fy);
            sum2 += (fx * fx + fy * fy); // accumulate the sum and sum of squares
        }
        double mean = sum / (2 * N), sigma = Sqrt(sum2 / (2 * N) - mean * mean); // calculate the mean and standard deviation
        var result = (mean * V, sigma * V / Sqrt(2 * N)); // return the result as a tuple
        // the factor of 2 in the denominator is because we are using two points per iteration

        return result;
    }

    public static (double, double) strata(Func<vector, double> f, vector a, vector b, double acc = 0.01, double eps = 0.01, int n_reuse = 0, double mean_reuse = 0, int nmin = 32)
    {
        int dim = a.size; // dimension of the integral
        int N = 16 * dim; // number of points in each stratum
        if (N < nmin)
        {
            return plainmc(f, a, b, N); // if the number of points is less than 32, use plain Monte Carlo integration
        }
        double V = 1;
        for (int k = 0; k < dim; k++)
        {
            V *= b[k] - a[k]; // calculate the volume of the hyperrectangle
        }
        double sum = 0.0, sum2 = 0.0; // sum and sum of squares
        double[] sumLeft = new double[dim]; // array to store the sum for the left stratum
        double[] sumRight = new double[dim]; // array to store the sum for the right stratum
        double[] sum2Left = new double[dim]; // array to store the sum of squares for the left stratum
        double[] sum2Right = new double[dim]; // array to store the sum of squares for the right stratum

        var mean_left = new double[dim]; // array to store the mean of each stratum
        var mean_right = new double[dim]; // array to store the mean of each stratum
        var n_left = new int[dim]; // array to store the number of points in each stratum
        var n_right = new int[dim]; // array to store the number of points in each stratum
        var x = new vector(dim); // point in the integration domain

        var rnd = new Random(); // random number generator

        for (int i = 0; i < N; i++)
        {
            for (int k = 0; k < dim; k++)
            {
                x[k] = a[k] + (b[k] - a[k]) * (rnd.NextDouble()); // generate a point in the integration domain
            }
            double fx = f(x); // evaluate the function at the point
            sum += fx; // accumulate the sum
            sum2 += fx * fx; // accumulate the sum of squares


            for (int k = 0; k < dim; k++)
            {
                double mid = (a[k] + b[k]) / 2; // calculate the midpoint of the stratum
                if (x[k] <= mid)
                {
                    mean_left[k] += fx; // accumulate the mean for the left stratum
                    n_left[k]++; // increment the number of points in the left stratum
                    sumLeft[k] += fx; // accumulate the sum for the left stratum
                    sum2Left[k] += fx * fx; // accumulate the sum of squares for the left stratum
                }

                else
                {
                    mean_right[k] += fx; // accumulate the mean for the right stratum
                    n_right[k]++; // increment the number of points in the right stratum
                    sumRight[k] += fx; // accumulate the sum for the right stratum
                    sum2Right[k] += fx * fx; // accumulate the sum of squares for the right stratum
                }
            }
        }

        double mean = sum / N; // calculate the overall mean
        double variable = sum2 / N - mean * mean; // calculate the variance
        double var_left = 0.0; // variance for the left stratum
        double var_right = 0.0; // variance for the right stratum

        int kdiv = 0;
        double maxvar = 0.0; // maximum variance

        for (int k = 0; k < dim; k++)
        {
            if (n_left[k] > 0)
            {
                mean_left[k] /= n_left[k]; // calculate the mean for the left stratum
                var_left = sum2Left[k] / n_left[k] - mean_left[k] * mean_left[k]; // calculate the variance for the left stratum
            }
            if (n_right[k] > 0)
            {
                mean_right[k] /= n_right[k]; // calculate the mean for the right stratum
                var_right = sum2Right[k] / n_right[k] - mean_right[k] * mean_right[k]; // calculate the variance for the right stratum
            }
            double tot_var = var_left + var_right; // total variance for the dimension
            if (tot_var > maxvar)
            {
                maxvar = tot_var; // update the maximum variance
                kdiv = k; // store the index of the dimension with the maximum variance
            }
        }

        for (int k = 0; k < dim; k++)
        {
            variable = Abs(mean_right[k] - mean_left[k]);
            if (variable > maxvar)
            {
                maxvar = variable; // update the maximum variance
                kdiv = k; // store the index of the dimension with the maximum variance
            }
        }

        double integ = (mean * N + mean_reuse * n_reuse) / (N + n_reuse) * V; // calculate the integral using the means from the strata and the reused points
        double error = Abs(mean_reuse - mean) * V;
        double toler = acc + Abs(integ) * eps;

        if (error < toler)
        {
            return (integ, error); // if the error is within the tolerance, return the integral and error
        }

        var a2 = a.copy(); // create a copy of the lower limits of integration
        var b2 = b.copy(); // create a copy of the upper limits of integration

        double middiv = 0.5 * (a[kdiv] + b[kdiv]); // calculate the midpoint of the dimension with the maximum variance

        b2[kdiv] = middiv; // set the upper limit of integration for the left stratum
        a2[kdiv] = middiv; // set the lower limit of integration for the right stratum

        var (integ_left, err_left) = strata(f, a, b2, acc / Math.Sqrt(2), eps, n_left[kdiv], mean_left[kdiv]); // recursively calculate the integral for the left stratum
        var (integ_right, err_right) = strata(f, a2, b, acc / Math.Sqrt(2), eps, n_right[kdiv], mean_right[kdiv]); // recursively calculate the integral for the right stratum

        return (integ_left + integ_right, err_left + err_right); // return the sum of the integrals and errors from both strata
    }


}

public class Halton
{
    List<int> bases;
    public Halton(int dim)
    { // constructor to initialize the bases for the Halton sequence
        bases = prime_numbers(dim);
    }

    public vector get(int n)
    { // generate the n-th point in the Halton sequence
        return x(n); // call the x method to get the point
    }
    public vector x(int n)
    { // generate the n-th point in the Halton sequence
        int dim = bases.Count;
        var result = new vector(dim);
        for (int i = 0; i < dim; i++)
        {
            result[i] = corput(n, bases[i]); // calculate the i-th coord using the corput function
        }
        return result;
    }

    private static double corput(int n, int b)
    { // calculate the n-th term in the van der Corput sequence for a given base
        double q = 0;
        double bk = 1.0 / b;
        while (n > 0)
        {
            q += (n % b) * bk;
            n /= b;
            bk /= b;
        }
        return q;
    }

    public static List<int> prime_numbers(int n)
    { // generate the first n prime numbers
        List<int> primes = new List<int>();
        int candidate = 2;
        while (primes.Count < n)
        {
            bool candidate_is_prime = true;
            foreach (int p in primes)
            {
                if (p * p > candidate) break; // no need to check beyond the square root
                if (candidate % p == 0)
                {
                    candidate_is_prime = false; // not a prime number
                    break;
                }
            }
            if (candidate_is_prime) primes.Add(candidate); // add the prime number to the list
            candidate++;
        }
        return primes;
    }
}