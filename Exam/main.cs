using static System.Console;
using static System.Math;
using System;
using System.IO;


class main
{

    static int Main(string[] args)
    {
        // ON THE MAIN WE TRY DIFFERENT WAYS TO COMPARE THE EFFICIENCY OF THE NEW INTEGRATOR COMPARED WITH THE ONE WE IMPLEMENTED IN THE HOMEWORK

        double accuracy = 1e-3; // accuracy goal
        double epsilon = 1e-3; // relative error goal

        //1. First we will compare the new integrator with the one we implemented in Exercise A of the homework
        var tests = new (string name, Func<double, double> f, double a, double b, double exact)[]
         {
            ("sqrt(x)",        x => Sqrt(x),         0.0, 1.0, 2.0/3.0),
            ("1/sqrt(x)",      x => 1.0/Sqrt(x),     0.0, 1.0, 2.0),
            ("sqrt(1-x^2)",    x => Sqrt(1 - x*x),   0.0, 1.0, PI/4),
            ("ln(x)/sqrt(x)",  x => Log(x)/Sqrt(x),  0.0, 1.0, -4.0)
         };

        WriteLine($"================= TEST INTEGRATOR WITH INTERESTING INTEGRALS =================");

        foreach (var test in tests)
        {
            var name = test.name;
            var f = test.f;
            var a = test.a;
            var b = test.b;
            var exact = test.exact;

            var sw = System.Diagnostics.Stopwatch.StartNew();
            double val = integrator.integrate(f, a, b);
            sw.Stop(); double t1 = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            double val3 = integrator_3.adaptive3(f, a, b);
            sw.Stop(); double t3 = sw.Elapsed.TotalMilliseconds;

            WriteLine($"\n\tHomework integrator:");
            WriteLine($"\t\tFunction = {name}, Exact = {exact:E}, Computed = {val:E}, Error = {Abs(val - exact):E}, Time = {t1:F5} ms");
            WriteLine($"\t\t\tNumber of function calls = {integrator.ncalls}");
            integrator.ncalls = 0; // reset function call counter
            WriteLine($"\n\tNew integrator:");
            WriteLine($"\t\tFunction = {name}, Exact = {exact:E}, Computed = {val3:E}, Error = {Abs(val3 - exact):E}, Time = {t3:F5} ms");
            WriteLine($"\t\t\tNumber of function calls = {integrator_3.ncalls}");
            integrator_3.ncalls = 0; // reset function call counter

            if (Abs(val - exact) > accuracy + epsilon * Abs(exact))
            {
                WriteLine($"\n\t\tHomework: Error: {name} integral failed! \n");
            }
            if (Abs(val3 - exact) > accuracy + epsilon * Abs(exact))
            {
                WriteLine($"\n\t\tNew: Error: {name} integral failed! \n");
            }
            if (Abs(val - exact) <= accuracy + epsilon * Abs(exact))
            {
                WriteLine($"\n\t\tHomework: {name} integral passed! \n");
            }
            if (Abs(val3 - exact) <= accuracy + epsilon * Abs(exact))
            {
                WriteLine($"\n\t\tNew: {name} integral passed! \n");
            }
        }



        // 2.1 Compare erf functions we implemented with the integretators with the tabulated values
        double[] zs = { -3.0, -2.0, -1.0, -0.5, 0.0, 0.5, 1.0, 1.5, 2.0, 3.0 };
        double[] exactErfs = { -0.9999779095030014, -0.9953222650189527, -0.84270079294971486934, -0.5204998778130465, 0.0, 0.5204998778130465, 0.84270079294971486934, 0.9661051464753108, 0.9953222650189527, 0.9999779095030014 };

        using (var file = new StreamWriter("erf.txt"))
        {
            file.WriteLine("x=z\terf value (Homework)\tTime Homework (ms)\terf value (New)\tTime New (ms)\tExact erf value\t Difference  Homework\tDifference New");
            for (int i = 0; i < zs.Length; i++)
            {
                double z = zs[i];

                var sw = System.Diagnostics.Stopwatch.StartNew();
                double cErf = integrator.erf(z);
                sw.Stop();
                double et1 = sw.Elapsed.TotalMilliseconds;

                sw.Restart();
                double cErf3 = integrator_3.erf(z);
                sw.Stop();
                double et3 = sw.Elapsed.TotalMilliseconds;

                double exactErf = exactErfs[i];
                double diff = Abs(cErf - exactErf);
                double diff3 = Abs(cErf3 - exactErf);

                file.WriteLine($"{z:F1}\t{cErf:F10}\t{et1:F5}\t{cErf3:F10}\t{et3:F5}\t{exactErf:F10}\t{diff:E}\t{diff3:E}");

                integrator.ncalls = 0; // reset function call counter for next test
                integrator_3.ncalls = 0; // reset function call counter for next test
            }
        }


        // 3. Now we will compare the Clenshaw-Curtis integrator we implemented in the homework with the new one
        using (var file = new System.IO.StreamWriter("f_counts_cc.txt", false))
        {
            file.WriteLine("Number\tFunction\tCC (Homework)\tIntegrator (Homework)\tCC (New)\tAdaptive3 (New)");
        }

        var tests2 = new (string name, Func<double, double> f, double a, double b, double exact)[]
        {
            ("1/sqrt(x)",      x => 1.0/Sqrt(x),     0.0, 1.0, 2.0),
            ("ln(x)/sqrt(x)",  x => Log(x)/Sqrt(x),  0.0, 1.0, -4.0),
            ("Sqrt(1-x^2)",    x => Sqrt(1 - x * x), -1.0, 1.0, PI / 2.0),
        };

        WriteLine($"\n========================== CLENSHAW-CURTIS ================================= \n");

        foreach (var test in tests2)
        {
            var name = test.name;
            var index = Array.IndexOf(tests2, test);
            var f = test.f;
            var a = test.a;
            var b = test.b;
            var exact = test.exact; // exact value of the integral

            var sw = System.Diagnostics.Stopwatch.StartNew();
            double val = integrator.ClenshawCurtis(f, a, b); // Homework Clenshaw-Curtis integrator
            sw.Stop(); double t = sw.Elapsed.TotalMilliseconds;
            int calls = integrator.ncalls; // calls from CC homework
            integrator.ncalls = 0;

            sw.Restart();
            double val2 = integrator.integrate(f, a, b);
            sw.Stop(); double t2 = sw.Elapsed.TotalMilliseconds;
            int calls2 = integrator.ncalls;      // calls from integrate homework
            integrator.ncalls = 0;

            sw.Restart();
            double val3 = integrator_3.ClenshawCurtis(f, a, b);
            sw.Stop(); double t3 = sw.Elapsed.TotalMilliseconds;
            int calls3 = integrator_3.ncalls;   // calls from CC new
            integrator_3.ncalls = 0;

            sw.Restart();
            double val4 = integrator_3.adaptive3(f, a, b);
            sw.Stop(); double t4 = sw.Elapsed.TotalMilliseconds;
            int calls4 = integrator_3.ncalls;  // calls from adaptive3 new
            integrator_3.ncalls = 0;

            WriteLine($"\tFunction = {name}, Exact = {exact:E}");
            WriteLine($"\t\tComputed with CC homework   = {val:E}, Calls = {calls},  Error = {Abs(val - exact):E}, Time = {t:F5} ms");
            WriteLine($"\t\tComputed with integrate home = {val2:E}, Calls = {calls2}, Error = {Abs(val2 - exact):E}, Time = {t2:F5} ms");
            WriteLine($"\t\tComputed with CC new        = {val3:E}, Calls = {calls3}, Error = {Abs(val3 - exact):E}, Time = {t3:F5} ms");
            WriteLine($"\t\tComputed with adaptive3 new = {val4:E}, Calls = {calls4}, Error = {Abs(val4 - exact):E}, Time = {t4:F5} ms");

            // We write all function calls to a file for later analysis
            using (var writer = new System.IO.StreamWriter("f_counts_cc.txt", true))
            {
                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                    index + 1, // index starts from 1
                    name,
                    calls,
                    calls2,
                    calls3,
                    calls4
                );
            }
            integrator.ncalls = 0;
            integrator_3.ncalls = 0;

        }


        // 4. Test the generalized integrators with some known integrals with infinite limits
        var tests_inf = new (string name, Func<double, double> f, double a, double b, double exact)[] {
            ("exp(x)",    x => Exp(x), double.NegativeInfinity, 1.0, Exp(1)),
            ("x*exp(-x)",  x => x*Exp(-x), 0.0, double.PositiveInfinity, 1.0),
            ("exp(-x^2)",   x => Exp(-x*x), double.NegativeInfinity, double.PositiveInfinity, Sqrt(PI)),
            ("1/(1+x^2)",   x => 1/(1+x*x), double.NegativeInfinity, double.PositiveInfinity, PI),

        };

        WriteLine($"\n========================== GENERALIZED INTEGRATOR, INFINITE LIMIT INTEGRALS ============================= \n");
        WriteLine($"Test of infinite-limit integrals with integrate_generalized\n");
        foreach (var test in tests_inf)
        {
            double val = integrator.integrate_generalized(test.f, test.a, test.b);
            double val3 = integrator_3.integrate_generalized(test.f, test.a, test.b);

            int calls = integrator.ncalls;
            int calls3 = integrator_3.ncalls;

            double err = Abs(val - test.exact);
            double err3 = Abs(val3 - test.exact);

            WriteLine($"Homework:\n");
            WriteLine($"\t{test.name}:\t Generalized integrator value:{val:E8}\t Exact value:{test.exact:E8}\t Error:{err:E2}\t Number of calls:{calls}\n");
            WriteLine($"New:\n");
            WriteLine($"\t{test.name}:\t Generalized integrator value:{val3:E8}\t Exact value:{test.exact:E8}\t Error:{err3:E2}\t Number of calls:{calls3}\n");

            integrator.ncalls = 0;
            integrator_3.ncalls = 0;

        }


        // 5. Test the new integrator error estimation with some known integrals, compared with the one we implemented in the homework

        WriteLine($"\n========================== TEST INTEGRATOR ERROR ESTIMATION ============================= \n");
        WriteLine($"\n\nName\tcomputed (Homework) \ttest_err (Homework) \tactual_err (Homework) \tcomputed (New) \ttest_err (New) \tactual_err (New) \t calls (Homework) \t calls (New)\n");

        using (var file = new StreamWriter("integrator_error.txt"))
        {
            file.WriteLine("Number\tTest Error (Homework)\tActual Error (Homework)\tTest Error (New)\tActual Error (New)");
        }
        foreach (var test in tests)
        {
            var (Q, err_est) = integrator.integrate_error(test.f, test.a, test.b, 1e-6, 1e-6);
            var (Q3, err_est3) = integrator_3.integrate_error3(test.f, test.a, test.b, 1e-6, 1e-6);
            double actual_err = Abs(Q - test.exact);
            double actual_err3 = Abs(Q3 - test.exact);
            WriteLine($"{test.name}\t\t{Q:E8}\t\t{err_est:E2}\t\t\t{actual_err:E2}\t\t\t{Q3:E8}\t\t{err_est3:E2}\t\t{actual_err3:E2}\t\t{integrator.ncalls}\t\t{integrator_3.ncalls}");

            using (var writer = new System.IO.StreamWriter("integrator_error.txt", true))
            {
                writer.WriteLine("{0}\t{1:E}\t{2:E}\t{3:E}\t{4:E}",
                    Array.IndexOf(tests, test) + 1, // index starts from 1
                    err_est,
                    actual_err,
                    err_est3,
                    actual_err3
                );
            }
            integrator.ncalls = 0; // reset function call counter for next test
            integrator_3.ncalls = 0; // reset function call counter for next test

        }


        // 6. Try calculate 'erf(1)' with the maximum precision that the new integrator can do.
        using (var file = new StreamWriter("erf1.txt"))
        {
            double z1 = 1.0;
            double exactErf1 = 0.84270079294971486934;
            double bestErr = double.PositiveInfinity;
            double bestAcc = 0.0;
            double bestValue = 0.0;

            for (int i = 1; i <= 20; i++)
            {
                double acc = Pow(10, -i); // accuracy goal

                double result3 = integrator_3.erf(z1, acc, acc);

                double err3 = Abs(result3 - exactErf1);

                if (err3 < bestErr)
                {
                    bestErr = err3;
                    bestAcc = acc;
                    bestValue = result3;
                }

                file.WriteLine($"{acc:E}\t{err3:E}\t{result3:F20}");

                integrator.ncalls = 0; // reset function call counter for next test
                integrator_3.ncalls = 0; // reset function call counter for next test
            }


            WriteLine($"\n\n=========================== ERF(1) COMPARISON ============================= \n");

            WriteLine($"\nBest accuracy: {bestAcc:E}, Error: {bestErr:E}, Value: {bestValue:F10} for erf(1) with new integrator.");
        }



        return 0;
    }


}

