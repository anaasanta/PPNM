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
    }