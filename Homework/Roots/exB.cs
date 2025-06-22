using static System.Console;
using static System.Math;
using System;
using System.IO;

public static class exB
{
    // Schrödinger equation for the radial wavefunction f(r) with energy E
    // y0 = f(r), y1 = f'(r)
    // The equation is: f''(r) = -2 * (E + 1/r) * f(r)
    public static Func<double, vector, vector> Schrodinger(double E)
    {
        return (r, y) => new vector(
            y[1],
            -2.0 * (E + 1.0 / r) * y[0]
        );
    }
    // Computes f(rmax) by integrating from rmin to rmax
    public static double shoot(double E, double rmin, double rmax, double acc, double eps)
    {
        var y0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin);
        var (xs, ys) = solveODE.driver(Schrodinger(E), (rmin, rmax), y0);
        var yEnd = ys[ys.Count - 1];
        return yEnd[0];
    }

    public static double findE0(double rmin, double rmax, double acc, double eps, double E1 = -1.0, double E2 = -0.1, double tol = 1e-6)
    {
        double f1 = shoot(E1, rmin, rmax, acc, eps); // Evaluate at E1
        double f2 = shoot(E2, rmin, rmax, acc, eps); // Evaluate at E2
        if (f1 * f2 > 0)
        {
            throw new Exception("E1 and E2 do not bracket a root.");
        }
        for (int iter = 0; iter < 50; iter++)
        {
            double Em = (E1 + E2) / 2;
            double fm = shoot(Em, rmin, rmax, acc, eps); // Evaluate at midpoint Em
            if (Abs(fm) < tol)
            {
                return Em; // If close enough to zero, we return Em
            }
            if (f1 * fm < 0)
            {
                E2 = Em;
                f2 = fm;
            } // If f1 and fm have opposite signs, the root is in [E1, Em]
            else
            {
                E1 = Em;
                f1 = fm;
            } // If f2 and fm have opposite signs, the root is in [Em, E2]
        }
        return (E1 + E2) / 2;
    }

}