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