using static System.Console;
using static System.Math;
using System;
using System.IO;

class main
{
    static int Main(string[] args)
    {
        /*Train network to approximate g(x)=cos(5*x-1)*exp(-x*x) on [-1,1] using Gaussian wavelets.*/

        int n = 3; // number of hidden neurons
        var network = new ann(n); // create a new ANN with n hidden neurons
        int m = 100; // number of data points
        var x = new vector(m); // input vector
        var y = new vector(m); // output vector

        for (int i = 0; i < m; i++)
        {
            x[i] = -1.0 + 2.0 * i / (m - 1); // fill input vector with values in the range [-1, 1]
            y[i] = Cos(5.0 * x[i] - 1.0) * Exp(-x[i] * x[i]); // compute target output using the function g(x)
        }

        network.train(x, y); // train the network with the input and output vectors

        using (var file = new StreamWriter("out_exA.txt"))
        {

            file.WriteLine($"=====Network with {n} hidden neurons trained to approximate g(x)=cos(5*x-1)*exp(-x*x) on [-1,1] using Gaussian wavelets.=====");


            file.WriteLine($"\tx\tg(x)\tANN response\tError");
            for (double i = -1; i <= 1; i += 0.05)
            {
                double x_test = i; // test point
                double y_test = Cos(5.0 * x_test - 1.0) * Exp(-x_test * x_test); // target output for the test point
                double response = network.response(x_test); // network response for the test point
                double error = Abs(response - y_test); // compute the error

                file.WriteLine($"{x_test:F2}\t{y_test:F4}\t{response:F4}\t{error:F4}"); // print the results

            }
        }

        // EXERCISE B

        int nb = 3; // number of hidden neurons
        var net = new ann(nb); // create a new ANN with n hidden neurons

        var xb = new vector(m); // input vector
        var yb = new vector(m); // output vector

        for (int i = 0; i < m; i++)
        {
            xb[i] = -1.0 + 2.0 * i / (m - 1); // fill input vector with values in the range [-1, 1]
            yb[i] = Pow(xb[i], 3); // compute target output using the function g(x)
        }

        net.train(xb, yb); // train the network with the input and output vectors

        using (var file = new StreamWriter("out_exB.txt"))
        {
            // Compare original, derivative, second derivative, antiderivative
            file.WriteLine(" x\ttrue\tnet\tdtrue\tdnet\td2true\td2net\tAtrue\tAnet");
            for (double i = -1; i <= 1.0001; i += 0.05)
            {
                double t = Pow(i, 3); // true value of the function g(x) = x^5
                double dt = 3 * Pow(i, 2); // true derivative of g(x)
                double d2t = 6 * i; // true second derivative of g(x)
                double at = (Pow(i, 4) - 1.0) / 4; // true antiderivative 

                double r = net.response(i);
                double dr = net.response_derivative(i);
                double d2r = net.response_derivative2(i);
                double Ar = net.response_antiderivative(i);

                file.WriteLine($"{i,5:F2} {t,8:F4} {r,8:F4} {dt,8:F4} {dr,8:F4} {d2t,8:F4} {d2r,8:F4} {at,8:F4} {Ar,8:F4}");
            }
        }



        return 0;

    }
}