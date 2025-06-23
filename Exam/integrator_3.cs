using static System.Math;
using System;

public class integrator_3
{
    public static int ncalls = 0;

    public static double integrate3(Func<double, double> f, double a, double b, double acc = 1e-3, double eps = 1e-3, double fmid = double.NaN)
    {
        double h = b - a;
        // To make sure we don't divide by zero
        if (h < 1e-16)
        {
            return 0.0;
        }


        // We have xi={1/6,3/6,5/6}
        double x1 = a + h / 6.0;
        double x2 = a + 3.0 * h / 6.0;
        double x3 = a + 5.0 * h / 6.0;

        double f1, f2, f3;

        if (double.IsNaN(fmid))
        {
            f1 = f(x1);
            f2 = f(x2);
            f3 = f(x3);
        } // First call, no points to reuse
        else
        {
            f1 = f(x1);
            f2 = fmid;
            f3 = f(x3);
        }

        // We caclulate the upper and lower order rules
        // We had wi={3/8,2/8,3/8}, vi={1/3,1/3,1/3}
        double Q = (3.0 / 8.0 * f1 + 2.0 / 8.0 * f2 + 3.0 / 8.0 * f3) * h;
        double q = ((f1 + f2 + f3) / 3.0) * h;

        double err = Abs(Q - q);
        double tol = acc + eps * Abs(Q);
        if (err <= tol)
        {
            return Q;
        }
        else
        {
            double m1 = a + h / 3.0;
            double m2 = a + 2.0 * h / 3.0;

            double subAcc = acc / Sqrt(3.0);

            double I1 = integrate3(f, a, m1, subAcc, eps, f1);
            double I2 = integrate3(f, m1, m2, subAcc, eps, f2);
            double I3 = integrate3(f, m2, b, subAcc, eps, f3);

            return I1 + I2 + I3;
        }
    }


    public static double erf(double z, double acc = 0.001, double eps = 0.001)
    {
        if (z < 0)
        {
            return -erf(-z, acc, eps);
        }
        else if (z <= 1)
        {
            return 2.0 / Sqrt(PI) * integrate3(x => Exp(-x * x), 0.0, z, acc, eps);
        }
        else
        {
            return 1.0 - 2.0 / Sqrt(PI) * integrate3(t => Exp(-Pow(z + (1 - t) / t, 2)) / (t * t), 0.0, 1.0, acc, eps);
        }
    }
}
