using static System.Math;
using System;

public class integrator_3
{
    public static int ncalls = 0;


    public static double adaptive3(Func<double, double> f, double a, double b, double acc = 1e-3, double eps = 1e-3, double fmid = double.NaN)
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
            ncalls += 3; // Count function calls
        } // First call, no points to reuse
        else
        {
            f1 = f(x1);
            f2 = fmid;
            f3 = f(x3);
            ncalls += 2; // Count function calls, we reuse f2
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
            double m1 = a + h / 3.0; // Midpoint for first segment
            double m2 = a + 2.0 * h / 3.0; // Midpoint for second segment

            double subAcc = acc / Sqrt(3.0); // Subdivision accuracy

            double I1 = adaptive3(f, a, m1, subAcc, eps, f1);
            double I2 = adaptive3(f, m1, m2, subAcc, eps, f2);
            double I3 = adaptive3(f, m2, b, subAcc, eps, f3);

            return I1 + I2 + I3;
        }
    }


    public static double erf(double z, double acc = 0.001, double eps = 0.001)
    { // Same as in the homework, but with a different integration method
        if (z < 0)
        {
            return -erf(-z, acc, eps);
        }
        else if (z <= 1)
        {
            return 2.0 / Sqrt(PI) * adaptive3(x => Exp(-x * x), 0.0, z, acc, eps);
        }
        else
        {
            return 1.0 - 2.0 / Sqrt(PI) * adaptive3(t => Exp(-Pow(z + (1 - t) / t, 2)) / (t * t), 0.0, 1.0, acc, eps);
        }
    }


    // From exercise B of the homework, but with the different integration method
    public static double ClenshawCurtis(Func<double, double> f, double a, double b, double acc = 0.001, double eps = 0.001)
    { // Change of variables to map [0, PI] to [a, b], following the definition on the homework
        if (a >= b) throw new ArgumentException("Lower limit must be less than upper limit.");
        Func<double, double> g = theta =>
        {
            double x = (a + b) / 2 + (b - a) / 2 * Cos(theta);
            ncalls++;// Count function calls
            return f(x) * Sin(theta) * (b - a) / 2;
        };

        return adaptive3(g, 0, PI, acc, eps);

    }

    // Integrator to accept infinite limits
    public static double integrate_generalized(Func<double, double> f, double a, double b, double acc = 0.001, double eps = 0.001, double f2 = double.NaN, double f3 = double.NaN)
    { // This cases are found on page 12 
        if (!double.IsInfinity(a) && !double.IsInfinity(b))
        {
            return ClenshawCurtis(f, a, b, acc, eps); // if both limits are finite, we use the ordinary integrator
        }
        else if (!double.IsInfinity(a) && double.IsPositiveInfinity(b))
        {
            // if lower limit is finite and upper limit is +infinity, we change variables 
            Func<double, double> g = x => f(a + (1 - x) / x) / (x * x);
            ncalls++; // Count function calls
            return ClenshawCurtis(g, 0.0, 1.0, acc, eps);
        }
        else if (double.IsNegativeInfinity(a) && !double.IsInfinity(b))
        {
            // if lower limit is -infinity and upper limit is finite, we change variables 
            Func<double, double> g = x => f(b - (1 - x) / x) / (x * x);
            ncalls++; // Count function calls
            return ClenshawCurtis(g, 0.0, 1.0, acc, eps);
        }
        else
        {
            double func1 = integrate_generalized(f, double.NegativeInfinity, 0.0, acc, eps);
            double func2 = integrate_generalized(f, 0.0, double.PositiveInfinity, acc, eps);
            return func1 + func2; // if both limits are infinite, we split the integral into two parts and sum them
        }

    }

    // FROM EXERCISE C of the homework, but with the different integration method
    public static (double Q, double err) integrate_error3(Func<double, double> f, double a, double b, double acc = 0.001, double eps = 0.001, double efmid = double.NaN)
    {

        double h = b - a;  // step size

        double ef1, ef2, ef3;
        // To make sure we don't divide by zero
        if (h < 1e-16)
        {
            return (0.0, 0.0);
        }

        // We have xi={1/6,3/6,5/6}
        double x1 = a + h / 6.0;
        double x2 = a + 3.0 * h / 6.0;
        double x3 = a + 5.0 * h / 6.0;

        if (double.IsNaN(efmid))
        {
            ef1 = f(x1);
            ef2 = f(x2);
            ef3 = f(x3);
            ncalls += 3; // Count function calls
        } // First call, no points to reuse
        else
        {
            ef1 = f(x1);
            ef2 = efmid;
            ef3 = f(x3);
            ncalls += 2; // Count function calls, we reuse ef2
        }

        double eQ = (3.0 / 8.0 * ef1 + 2.0 / 8.0 * ef2 + 3.0 / 8.0 * ef3) * h; // higher order rule
        double eq = ((ef1 + ef2 + ef3) / 3.0) * h; // lower order rule


        double eerr = Math.Abs(eQ - eq); // error estimate
        double tol = acc + eps * Abs(eQ);
        if (eerr <= tol)
        {
            return (eQ, eerr);
        }
        else
        {
            double em1 = a + h / 3.0; // Midpoint for first segment
            double em2 = a + 2.0 * h / 3.0; // Midpoint for second segment

            double subAcc = acc / Sqrt(3.0); // Subdivision accuracy

            var (Q1, e1) = integrate_error3(f, a, em1, subAcc, eps, ef1);
            var (Q2, e2) = integrate_error3(f, em1, em2, subAcc, eps, ef2);
            var (Q3, e3) = integrate_error3(f, em2, b, subAcc, eps, ef3);
            return (Q1 + Q2 + Q3, Sqrt(e1 * e1 + e2 * e2 + e3 * e3));
        }
    }

}
