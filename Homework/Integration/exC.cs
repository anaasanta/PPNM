using static System.Console;
using static System.Math;
using System;
using System.IO;

class exC
{
    static int Main(string[] args)
    {
        var tests = new (string name, Func<double, double> f, double a, double b, double exact)[]
        {
            ("sqrt(x)",        x => Sqrt(x),         0.0, 1.0, 2.0/3.0),
            ("1/sqrt(x)",      x => 1.0/Sqrt(x),     0.0, 1.0, 2.0),
            ("sqrt(1-x^2)",    x => Sqrt(1 - x*x),   0.0, 1.0, PI/4),
            ("ln(x)/sqrt(x)",  x => Log(x)/Sqrt(x),  0.0, 1.0, -4.0)
        };

        WriteLine($"Index\tName\tcomputed\test_err\tactual_err");

        foreach (var test in tests)
        {
            var (Q, err_est) = integrator.integrate_error(test.f, test.a, test.b, 1e-6, 1e-6);
            double actual_err = Abs(Q - test.exact);
            int index = Array.IndexOf(tests, test) + 1; // index starts from 1
            WriteLine($"{index}\t{test.name}\t{Q:E8}\t{err_est:E2}\t{actual_err:E2}");
        }

        return 0;
    }
}