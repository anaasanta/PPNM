using static System.Console;
using static System.Math;
using System;
using System.IO;
class exB
{
    static int Main(string[] args)
    {
        // 1. Integrals we are going to test:
        var tests = new (string name, Func<double, double> f, double a, double b, double exact)[]
        {
            ("1/sqrt(x)",      x => 1.0/Sqrt(x),     0.0, 1.0, 2.0),
            ("ln(x)/sqrt(x)",  x => Log(x)/Sqrt(x),  0.0, 1.0, -4.0)
        };

        WriteLine($"Testing Clenshaw-Curtis integrator on known integrals: \n");
        foreach (var test in tests)
        {
            var name = test.name;
            var f = test.f;
            var a = test.a;
            var b = test.b;
            var exact = test.exact; // exact value 
            double val = integrator.ClenshawCurtis(f, a, b); //value witht the Clenshaw-Curtis method
            int calls = integrator.ncalls; // number of function calls
            integrator.ncalls = 0; // Reset function call counter for next test
            double val2 = integrator.integrate(f, a, b); //value with the ordinary integrator, without variable transformation 

            WriteLine($"Function = {name}, Exact = {exact:E}");
            WriteLine($"\t\tComputed with Clenshaw-Curtis= {val:E}, Function calls = {calls}, Error = {Abs(val - exact):E}");
            WriteLine($"\t\tComputed with ordinary integrator= {val2:E}, Error with exact value = {Abs(val2 - exact):E}, Error with Clenshaw-Curtis = {Abs(val2 - val):E}");
            integrator.ncalls = 0; // reset call count for next test

            // Check that integrator returns results within the given accuracy goals.
            if (Abs(val - exact) > 0.001)
            {
                WriteLine($"\n\t\tError with Clenshaw-Curtis: {name} integral failed! \n");
            }
            else
            {
                WriteLine($"\n\t\tWith Clenshaw-Curtis, {name} integral passed. \n");
            }
        }

        var tests_inf = new (string name, Func<double, double> f, double a, double b, double exact)[] {
            ("exp(x)",    x => Exp(x), double.NegativeInfinity, 1.0, Exp(1)),
            ("x*exp(-x)",  x => x*Exp(-x), 0.0, double.PositiveInfinity, 1.0),
            ("exp(-x^2)",   x => Exp(-x*x), double.NegativeInfinity, double.PositiveInfinity, Sqrt(PI)),
        };

        using (var file = new StreamWriter("exB_inf.txt"))
        {
            file.WriteLine($"Test of infinite-limit integrals with integrate_generalized\n");
            foreach (var test in tests_inf)
            {
                integrator.ncalls = 0;
                double val = integrator.integrate_generalized(test.f, test.a, test.b);
                int calls = integrator.ncalls;
                integrator.ncalls = 0; // Reset function call counter for next test
                double err = Abs(val - test.exact);
                file.WriteLine($"{test.name}:\t Generalized integrator value:{val:E8}\t Exact value:{test.exact:E8}\t Error:{err:E2}\t Number of calls:{calls}\n");
            }

        }
        return 0;

    }

}