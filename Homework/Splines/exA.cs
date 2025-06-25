using static System.Console;
using static System.Math;
using System;

class exA
{

    public static double linterp(double[] x, double[] y, double z)
    {/* Function that makes linear spline interpolation from a table {x[i],y[i]} at a given point z*/
        int i = binsearch(x, z);
        if (i < 0)
        {
            throw new Exception("z is out of range");
        }
        double dx = x[i + 1] - x[i];
        if (!(dx > 0))
        {
            throw new Exception("x[i+1] must be greater than x[i]");
        }
        double dy = y[i + 1] - y[i];

        return y[i] + dy / dx * (z - x[i]);
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

    public static double linterpInteg(double[] x, double[] y, double z)
    {/* Function that calculates the integral (analytically) of the linear spline from the point x[0] to a given point z*/
        int i = binsearch(x, z);
        double a = 0, dx = 0, dy = 0;
        if (i < 0)
        {
            throw new Exception("z is out of range");
        }
        vector p = new vector(x.Length);
        for (int j = 0; j < i; j++)
        {
            dx = x[j + 1] - x[j];
            dy = y[j + 1] - y[j];
            p[j] = dy / dx;
            a += y[j] * dx + p[j] * dx * dx / 2;
        }
        dx = x[i + 1] - x[i];
        dy = y[i + 1] - y[i];
        p[i] = dy / dx;
        a += y[i] * (z - x[i]) + p[i] * (z - x[i]) * (z - x[i]) / 2;

        return a;

    }

    static int Main(string[] args)
    {
        // we will take the table {xi=0,1,…,9; yi=cos(xi)}, and plot its linear interpolant together with the
        // interpolant's antiderivative  

        vector x = new vector(10);
        vector y = new vector(10);

        for (int i = 0; i < 10; i++)
        {
            x[i] = i;
            y[i] = Cos(i);
        }

        double linz, integz;
        for (double z = x[0]; z <= x[x.size - 1]; z += 1.0 / 10)
        {
            linz = linterp(x, y, z);
            integz = linterpInteg(x, y, z);
            WriteLine($"{z} {linz} {integz}");
        }

        return 0;
    }
}
