using static System.Console;
using static System.Math;
using System;
using System.Collections.Generic;
using System.IO;

class exC
{
    public static vector f(double x, vector y)
    {
        vector r = new vector(2);
        r[0] = y[1];
        r[1] = 2 * x;
        return r;
    }

    public static Func<double, vector> make_qspline(List<double> x, List<vector> y) //using Splines exercise
    {
        /* calculate bi and ci of the quadratic spline */

        int n = x.Count;
        if (n < 2) throw new ArgumentException("Need at least 2 points");

        var b = new vector[n - 1];
        var c = new vector[n - 1];
        for (int i = 0; i < n - 1; i++)
        {
            double dx = x[i + 1] - x[i];
            if (dx <= 0) throw new ArgumentException("x have to be different and ordered");
            b[i] = (y[i + 1] - y[i]) * (1.0 / dx);
        }
        c[0] = new vector(y[0].size);  // natural spline: c[0] = 0
        for (int i = 1; i < n - 1; i++)
        {
            double dx = x[i + 1] - x[i];
            c[i] = (b[i] - b[i - 1]) * (1.0 / dx);
        }
        Func<double, vector> qspline = delegate (double z)
        {
            int i = binsearch(x, z);
            return y[i] + (z - x[i]) * (b[i] + c[i] * (z - x[i])); ;
        };
        return qspline;
    }

    public static int binsearch(List<double> x, double z) //From Splines exercise
    {/* locates the interval for z by bisection */
        if (z < x[0] || z > x[x.Count - 1])
        {
            throw new Exception("z is out of range");
        }
        int i = 0, j = x.Count - 1;
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


    public static Func<double, vector> make_ode_ivp_qspline
    (Func<double, vector, vector> f, (double, double) interval, vector y,
    double acc = 0.01, double eps = 0.01, double hstart = 0.01)
    {
        var (xlist, ylist) = solveODE.driver(f, interval, y, hstart, acc, eps);

        return make_qspline(xlist, ylist);
    }


    public static int Main(string[] args)
    {
        // SECTION 1 
        vector yinit = new vector(2);
        yinit[0] = 0; // y(0) = 0
        yinit[1] = 0; // y'(0) = 0

        double a = 0;
        double b = 10;

        var (xlist, ylist, hlist) = solveODE.driver23(f, (a, b), yinit);


        using (StreamWriter file = new StreamWriter("out_exC_1.txt"))
        {
            file.WriteLine($"\tx\t\ty0\t\ty1\t\th\t\tif <1e-10 h doubles ");
            for (int i = 0; i < xlist.Count; i++)
            {
                if (i <= 1) // first two points
                {
                    file.WriteLine($"{xlist[i]} {ylist[i][0]} {ylist[i][1]} {hlist[i]}");
                    continue; // skip the first point
                }
                else
                {
                    string doubled = Abs(hlist[i] - 2 * hlist[i - 1]) <= 1e-10 ? "h doubles" : "not doubled";
                    file.WriteLine($"{xlist[i]} {ylist[i][0]} {ylist[i][1]} {hlist[i]} {doubled}");
                }
            }
        }

        // SECTION 2 and 3
        var y0 = new vector(2);
        Func<double, vector> qs = make_ode_ivp_qspline(f, (0, 10), y0, 1e-6, 1e-6, 0.1);
        using (StreamWriter file = new StreamWriter("out_exC_2.txt"))
        {
            int N = 100;
            for (int k = 0; k <= N; k++)
            {
                double j = 10.0 * k / N;
                var l = qs(j);
                file.WriteLine($"{j} {l[0]} {l[1]}");
            }
        }


        // SECTION 4

        Func<double, vector, vector> three_body = (t, z) =>
        {
            var zz = new vector(12);
            // z = {x'1, y'1, x'2, y'2, x'3, y'3, x1, y1, x2, y2, x3, y3}
            zz[0] = (z[8] - z[6]) / Pow(Pow(z[8] - z[6], 2) + Pow(z[9] - z[7], 2), 1.5) + (z[10] - z[6]) / Pow(Pow(z[10] - z[6], 2) + Pow(z[11] - z[7], 2), 1.5);
            zz[1] = (z[9] - z[7]) / Pow(Pow(z[8] - z[6], 2) + Pow(z[9] - z[7], 2), 1.5) + (z[11] - z[7]) / Pow(Pow(z[10] - z[6], 2) + Pow(z[11] - z[7], 2), 1.5);
            zz[2] = (z[6] - z[8]) / Pow(Pow(z[6] - z[8], 2) + Pow(z[7] - z[9], 2), 1.5) + (z[10] - z[8]) / Pow(Pow(z[10] - z[8], 2) + Pow(z[11] - z[9], 2), 1.5);
            zz[3] = (z[7] - z[9]) / Pow(Pow(z[6] - z[8], 2) + Pow(z[7] - z[9], 2), 1.5) + (z[11] - z[9]) / Pow(Pow(z[10] - z[8], 2) + Pow(z[11] - z[9], 2), 1.5);
            zz[4] = (z[6] - z[10]) / Pow(Pow(z[6] - z[10], 2) + Pow(z[7] - z[11], 2), 1.5) + (z[8] - z[10]) / Pow(Pow(z[8] - z[10], 2) + Pow(z[9] - z[11], 2), 1.5);
            zz[5] = (z[7] - z[11]) / Pow(Pow(z[6] - z[10], 2) + Pow(z[7] - z[11], 2), 1.5) + (z[9] - z[11]) / Pow(Pow(z[8] - z[10], 2) + Pow(z[9] - z[11], 2), 1.5);
            zz[6] = z[0];
            zz[7] = z[1];
            zz[8] = z[2];
            zz[9] = z[3];
            zz[10] = z[4];
            zz[11] = z[5];

            return zz;
        };

        vector zi = new vector(0.4662036850, 0.4323657300, -0.93240737, -0.86473146, 0.4662036850, 0.4323657300, -0.97000436, 0.24308753, 0.0, 0.0, 0.97000436, -0.24308753);

        var (xtb, ytb) = solveODE.driver(three_body, (0, 6.3259 / 3), zi, 0.001, 1e-4, 1e-4);

        using (var file = new StreamWriter("out_exC_4.txt"))
        {
            for (int i = 0; i < xtb.Count; i++)
            {
                file.WriteLine($"{ytb[i][6]} {ytb[i][7]} {ytb[i][8]} {ytb[i][9]} {ytb[i][10]} {ytb[i][11]}");
            }
        }

        return 0;
    }
}