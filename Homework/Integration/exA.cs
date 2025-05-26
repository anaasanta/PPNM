using static System.Console;
using static System.Math;
using System;
using System.IO;


class exA
{

    static int Main(string[] args)
    {
        // 1. To test the adaptive integrator, we will use the following known integrals (tests is a tuple of name, function, lower limit, upper limit, exact value):
        var tests = new (string name, Func<double, double> f, double a, double b, double exact)[]
        {
            ("sqrt(x)",        x => Sqrt(x),         0.0, 1.0, 2.0/3.0),
            ("1/sqrt(x)",      x => 1.0/Sqrt(x),     0.0, 1.0, 2.0),
            ("sqrt(1-x^2)",    x => Sqrt(1 - x*x),   0.0, 1.0, PI/2),
            ("ln(x)/sqrt(x)",  x => Log(x)/Sqrt(x),  0.0, 1.0, -4.0)
        };

        WriteLine($"Testing adaptive integrator on known integrals: \n");
        foreach (var test in tests)
        {
            var name = test.name;
            var f = test.f;
            var a = test.a;
            var b = test.b;
            var exact = test.exact;
            double val = integrator.integrate(f, a, b);
            WriteLine($"Function = {name}, Exact = {exact:E}, Computed = {val:E}, Error = {Abs(val - exact):E}");

            // Check that integrator returns results within the given accuracy goals.
            if (Abs(val - exact) > 0.001)
            {
                WriteLine($"Error: {name} integral failed! \n");
            }
            else
            {
                WriteLine($"{name} integral passed. \n");
            }
        }

        // 2. Compare erf function we implemented with integretator with the tabulated values
        double[] zs = { -3.0, -2.0, -1.0, -0.5, 0.0, 0.5, 1.0, 1.5, 2.0, 3.0 };
        double[] exactErfs = { -0.9999779095030014, -0.9953222650189527, -0.84270079294971486934, -0.5204998778130465, 0.0, 0.5204998778130465, 0.84270079294971486934, 0.9661051464753108, 0.9953222650189527, 0.9999779095030014 };

        using (var file = new StreamWriter("erf.txt"))
        {
            for (int i = 0; i < zs.Length; i++)
            {
                double z = zs[i];
                double cErf = integrator.erf(z);
                double exactErf = exactErfs[i];
                double diff = Abs(cErf - exactErf);
                file.WriteLine($"{z:F1}\t{cErf:F10}\t{exactErf:F10}\t{diff:E}");
            }
        }


        //3. Calculate erf(1) with eps=0 and decreasing acc= 0.1, 0.01, 0.001, which we'll plot in log-log scale.
        using (var file = new StreamWriter("erf1.txt"))
        {
            double z1 = 1.0;
            double exactErf1 = 0.84270079294971486934;
            for (int i = 1; i <= 10; i++)
            {
                double acc = Pow(10, -i); // accuracy goal
                double result = integrator.erf(z1, acc, 0.0);
                double err = Abs(result - exactErf1);
                file.WriteLine($"{acc:E}\t{err:E}\t{result:F10}");
            }
        }

        return 0;
    }



}