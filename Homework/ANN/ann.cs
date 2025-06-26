using static System.Console;
using static System.Math;
using System;
using System.IO;

public class ann
{
    int n; /* number of hidden neurons */

    Func<double, double> f = x => x * Exp(-x * x); /* activation function */
    Func<double, double> df = x => (1 - 2 * x * x) * Exp(-x * x); /* derivative of the activation function */
    Func<double, double> d2f = x => (-6 * x + 4 * x * x * x) * Exp(-x * x); /* second derivative of the activation function */
    Func<double, double> F = x => -Exp(-x * x) / 2; /* antiderivative of the activation function*/

    public vector p; /* network parameters */
    private vector xs;
    private vector ys;

    public ann(int n)
    {/* constructor */
        this.n = n;
        p = new vector(3 * n); // Allocate space for params
        var rand = new Random(); // Random number generator

        for (int i = 0; i < n; i++)
        {
            if (n == 1)
            {
                p[3 * i] = 0.0; // a_0 = 0.0 for single hidden neuron
            }
            else
            {
                p[3 * i] = -1.0 + 2.0 * i / (n - 1); // a_i equally spaced in [-1, 1]

            }
            p[3 * i + 1] = 0.5 + rand.NextDouble(); // b_i = 0.5 + random value in [0, 1]
            p[3 * i + 2] = 1.0;
        }
    }

    public double response(double x)
    {
        /* return the response of the network to the input signal x */
        double sum = 0.0; // accumulator for the response
        for (int i = 0; i < n; i++)
        {
            double ai = p[3 * i];
            double bi = p[3 * i + 1];
            double wi = p[3 * i + 2];
            sum += wi * f((x - ai) / bi); // compute the contribution of each hidden neuron
        }
        return sum;
    }

    public double response_derivative(double x)
    {
        /* return the derivative of the response of the network to the input signal x */
        double sum = 0.0; // accumulator for the derivative
        for (int i = 0; i < n; i++)
        {
            double ai = p[3 * i];
            double bi = p[3 * i + 1];
            double wi = p[3 * i + 2];
            sum += wi * df((x - ai) / bi) / bi; // compute the contribution of each hidden neuron
        }
        return sum;
    }

    public double response_derivative2(double x)
    {
        /* return the second derivative of the response of the network to the input signal x */
        double sum = 0.0; // accumulator for the second derivative
        for (int i = 0; i < n; i++)
        {
            double ai = p[3 * i];
            double bi = p[3 * i + 1];
            double wi = p[3 * i + 2];
            sum += wi * d2f((x - ai) / bi) / (bi * bi); // compute the contribution of each hidden neuron
        }
        return sum;
    }

    public double response_antiderivative(double x)
    {
        /* return the antiderivative of the response of the network to the input signal x */
        double sum = 0.0; // accumulator for the antiderivative
        for (int i = 0; i < n; i++)
        {
            double ai = p[3 * i];
            double bi = p[3 * i + 1];
            double wi = p[3 * i + 2];
            sum += wi * F((x - ai) / bi) * bi; // compute the contribution of each hidden neuron
        }
        return sum;
    }

    //Cost function C(p) = sum_k (F_p(x_k) - y_k)^2 / n
    public double cost(vector pp)
    {
        // copy candidate params into p
        p = pp.copy();
        double c = 0.0;
        for (int k = 0; k < xs.size; k++)
        {
            double d = response(xs[k]) - ys[k];
            c += d * d;
        }
        return c; // xs.size; // return the average squared error
        // I put /xs.size like in the lecture notes, but it seems that without it it ajusts more to the actual values of the function 
    }
    public void train(vector x, vector y)
    {/* train the network to interpolate the given table {x,y} */

        this.xs = x.copy();
        this.ys = y.copy();


        Func<vector, double> phi = p => cost(p); // cost function

        WriteLine($"Initial cost: {cost(p)}"); // print initial cost. I put it to see if the cost function is working correctly

        // Newton with our gradient
        vector x0 = p.copy(); // initial guess for parameters
        (vector p_opt, int iter) = minimisation.newton_c(phi, x0); // optimize parameters using Newton's method
        this.p = p_opt.copy(); // update network parameters with optimized values
        WriteLine($"Training completed in {iter} iterations with final cost: {cost(p)}");

    }

}