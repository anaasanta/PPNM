using static System.Console;
using static System.Math;
using System;

public class qspline
{
    public vector x, y, b, c;
    private int n;

    public qspline(vector xs, vector ys)
    {
        n = xs.size;
        if (ys.size != n)
            throw new Exception("xs and ys must have the same length");
        b = new vector(n - 1);
        c = new vector(n - 1);
        this.x = xs.copy();
        this.y = ys.copy();

        vector h = new vector(n - 1);  // lengths of intervals
        vector p = new vector(n - 1);  // slopes of intervals

        for (int i = 0; i < n - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
            if (h[i] <= 0)
                throw new Exception("x[i+1] must be greater than x[i]");
            p[i] = (y[i + 1] - y[i]) / h[i];
        }


        c[0] = 0.0;
        for (int i = 0; i < n - 2; i++)
        {
            c[i + 1] = (p[i + 1] - p[i] - c[i] * h[i]) / h[i + 1];
        }
        c[n - 2] /= 2.0;
        for (int i = n - 3; i >= 0; i--)
        {
            c[i] = (p[i + 1] - p[i] - c[i + 1] * h[i + 1]) / h[i];
        }

        for (int i = 0; i < n - 1; i++)
        {
            b[i] = p[i] - c[i] * h[i];
        }
    }

    public double evaluate(double z)
    {/* Evaluate the spline*/

        int i = binsearch(x, z);
        if (i < 0)
        {
            throw new Exception("z is out of range");
        }

        double h = z - x[i];
        return y[i] + h * (b[i] + h * c[i]);
    }

    public double derivative(double z)
    {/* evaluate the derivative*/

        int i = binsearch(x, z);
        if (i < 0)
        {
            throw new Exception("z is out of range");
        }

        double h = z - x[i];
        return b[i] + 2 * c[i] * h;

    }

    public double integral(double z)
    {/* evaluate the integral*/

        int i = binsearch(x, z);
        if (i < 0)
        {
            throw new Exception("z is out of range");
        }
        double a = 0, dx = 0;
        for (int j = 0; j < i; j++)
        {
            dx = x[j + 1] - x[j];
            a += y[j] * dx + b[j] * dx * dx / 2.0 + c[j] * dx * dx * dx / 3.0;
        }

        a += y[i] * (z - x[i]) + b[i] * (z - x[i]) * (z - x[i]) / 2 + c[i] * (z - x[i]) * (z - x[i]) * (z - x[i]) / 3.0;

        return a;

    }

    public static int binsearch(double[] x, double z)
    {/* locates the interval for z by bisection */
        if (z < x[0] || z > x[x.Length - 1])
        {
            throw new Exception("z is out of range");
        }
        int i = 0, j = x.Length - 1;
        while (j - i > 1)
        {
            int mid = (i + j) / 2;
            if (z > x[mid])
            {
                i = mid;
            }

            else
            {
                j = mid;
            }
        }

        return i;
    }



    public static int Main(string[] args)
    {
        vector x = new vector(10);
        vector y = new vector(10);
        for (int i = 0; i < 10; i++)
        {
            x[i] = i;
            y[i] = Sin(i);
        }
        qspline s = new qspline(x, y);
        double sinz, derz, integz;
        for (double z = x[0]; z < x[x.size - 1]; z += 0.1)
        {
            sinz = s.evaluate(z);
            derz = s.derivative(z);
            integz = s.integral(z);
            Console.WriteLine($"{z} {sinz} {derz} {integz}");

        }

        return 0;
    }
}
