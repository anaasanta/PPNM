using static System.Console;
using static System.Math;
using System;


public class integrator
{
    public static int ncalls = 0; // count function calls

    // FROM EXERCISE A
    public static double integrate(Func<double, double> f, double a, double b, double acc = 0.001, double eps = 0.001, double f2 = double.NaN, double f3 = double.NaN) // NaN indicates first call
    {
        double h = b - a;  // step size
        if (double.IsNaN(f2))
        {
            f2 = f(a + 2 * h / 6);
            f3 = f(a + 4 * h / 6);
        } // first call, no points to reuse

        double f1 = f(a + h / 6), f4 = f(a + 5 * h / 6);
        double Q = (2 * f1 + f2 + f3 + 2 * f4) / 6 * (b - a); // higher order rule
        double q = (f1 + f2 + f3 + f4) / 4 * (b - a); // lower order rule
        double err = Math.Abs(Q - q); // error estimate
        if (err <= acc + eps * Math.Abs(Q))
        {
            return Q;
        }
        else
        {
            return integrate(f, a, (a + b) / 2, acc / Math.Sqrt(2), eps, f1, f2) + integrate(f, (a + b) / 2, b, acc / Math.Sqrt(2), eps, f3, f4);
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
            return 2.0 / Sqrt(PI) * integrate(x => Exp(-x * x), 0.0, z, acc, eps);
        }
        else
        {
            return 1.0 - 2.0 / Sqrt(PI) * integrate(t => Exp(-Pow(z + (1 - t) / t, 2)) / (t * t), 0.0, 1.0, acc, eps);
        }
    }

    // FROM EXERCISE B
    public static double ClenshawCurtis(Func<double, double> f, double a, double b, double acc = 0.001, double eps = 0.001)
    {
        if (a >= b) throw new ArgumentException("Lower limit must be less than upper limit.");
        Func<double, double> g = theta =>
        {
            double x = (a + b) / 2 + (b - a) / 2 * Cos(theta);
            ncalls++;
            return f(x) * Sin(theta) * (b - a) / 2;
        };

        return integrate(g, 0, PI, acc, eps);

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
            return ClenshawCurtis(g, 0.0, 1.0, acc, eps);
        }
        else if (double.IsNegativeInfinity(a) && !double.IsInfinity(b))
        {
            // if lower limit is -infinity and upper limit is finite, we change variables 
            Func<double, double> g = x => f(b - (1 - x) / x) / (x * x);
            return ClenshawCurtis(g, 0.0, 1.0, acc, eps);
        }
        else
        {
            double func1 = integrate_generalized(f, double.NegativeInfinity, 0.0, acc, eps);
            double func2 = integrate_generalized(f, 0.0, double.PositiveInfinity, acc, eps);
            return func1 + func2; // if both limits are infinite, we split the integral into two parts and sum them
        }

    }

    // FROM EXERCISE C
    public static (double Q, double err) integrate_error(Func<double, double> f, double a, double b, double acc = 0.001, double eps = 0.001, double ef2 = double.NaN, double ef3 = double.NaN)
    {

        double h = b - a;  // step size
        if (double.IsNaN(ef2))
        {
            ef2 = f(a + 2 * h / 6);
            ef3 = f(a + 4 * h / 6);
        } // first call, no points to reuse

        double ef1 = f(a + h / 6), ef4 = f(a + 5 * h / 6);
        double eQ = (2 * ef1 + ef2 + ef3 + 2 * ef4) / 6 * (b - a); // higher order rule
        double eq = (ef1 + ef2 + ef3 + ef4) / 4 * (b - a); // lower order rule
        double eerr = Math.Abs(eQ - eq); // error estimate
        double tol = acc + eps * Abs(eQ);
        if (eerr <= tol)
        {
            return (eQ, eerr);
        }
        else
        {
            var (Q1, e1) = integrate_error(f, a, (a + b) / 2, acc / Sqrt(2), eps, ef1, ef2);
            var (Q2, e2) = integrate_error(f, (a + b) / 2, b, acc / Sqrt(2), eps, ef3, ef4);
            return (Q1 + Q2, Sqrt(e1 * e1 + e2 * e2));
        }
    }


}