using static System.Console;
using static System.Math;
using System;
using System.IO;
using System.Collections.Generic;

class main
{

    //EXERCISE A

    // Gradient of Rosenbrock function f(x,y) = (1-x)^2 + 100(y-x^2)^2
    static vector GradRosen(vector v)
    {
        double x = v[0], y = v[1];
        // df/dx = -2(1-x) - 400 x (y - x^2)
        // df/dy =  200 (y - x^2)
        return new vector(
            -2 * (1 - x) - 400 * x * (y - x * x),
            200 * (y - x * x)
        );
    }

    // Gradient of Himmelblau's function f(x,y) = (x^2 + y - 11)^2 + (x + y^2 - 7)^2
    static vector GradHimmelblau(vector v)
    {
        double x = v[0], y = v[1];
        // df/dx = 2(x^2 + y - 11) * 2x + 2(x + y^2 - 7) = 4x(x^2 + y - 11) + 2(x + y^2 - 7)
        // df/dy = 2(x^2 + y - 11) + 2(x + y^2 - 7) * 2y = 2(x^2 + y - 11) + 4y(x + y^2 - 7)
        return new vector(
            4 * x * (x * x + y - 11) + 2 * (x + y * y - 7),
            2 * (x * x + y - 11) + 4 * y * (x + y * y - 7)
        );
    }


    static int Main(string[] args)
    {
        // EXERCISE A

        // Initial guesses
        var startR = new vector(-1.2, 1.0);
        var startH = new vector(-2.0, 3.0);

        WriteLine($"Running Newton's method on Rosenbrock gradient...");
        var rootR = exA.newton(GradRosen, startR, 1e-6);
        WriteLine($"  Rosenbrock minimum approx at (x,y) = ({rootR[0]:F6}, {rootR[1]:F6}), f-||grad|| = {GradRosen(rootR).norm():E3}");

        WriteLine($"\nRunning Newton's method on Himmelblau gradient...");
        var rootH = exA.newton(GradHimmelblau, startH, 1e-6);
        WriteLine($"  Himmelblau minimum approx at (x,y) = ({rootH[0]:F6}, {rootH[1]:F6}), f-||grad|| = {GradHimmelblau(rootH).norm():E3}");


        // EXERCISE B

        double rmin = 1e-7;
        double rmax = 8.0;
        double h0 = 0.125;
        double acc = 1e-6;
        double eps = 1e-6;

        // Find ground-state energy
        double E0 = exB.findE0(rmin, rmax, acc, eps);
        WriteLine($"Computed E0 = {E0:F6} (exact -0.5)");

        // Integrate nominal wavefunction
        var y0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin);
        var tupleNom = solveODE.driver(exB.Schrodinger(E0), (rmin, rmax), y0, h: h0, acc: acc, eps: eps);
        var rs = tupleNom.Item1;
        var ys = tupleNom.Item2;

        using (var file = new StreamWriter("wave.dat"))
        {
            for (int i = 0; i < rs.Count; i++)
            {
                double r = rs[i];
                double fnum = ys[i][0];
                double fana = r * Exp(-r);
                file.WriteLine($"{r:E6} {fnum:E6} {fana:E6}");
            }
        }

        double[] rmaxVals = { 4, 6, 8, 10 };
        foreach (var rmaxVal in rmaxVals)
        {
            var (rListTemp, yListTemp) = solveODE.driver(exB.Schrodinger(E0), (rmin, rmaxVal), y0, 0.1);
            using (var file = new StreamWriter($"wave_rmax_{rmaxVal}.dat"))
            {
                for (int i = 0; i < rListTemp.Count; i++)
                {
                    double r = rListTemp[i];
                    double f = yListTemp[i][0];
                    file.WriteLine($"{r} {f}");
                }
            }
        }

        double[] rminVals = { 1e-6, 1e-5, 1e-4, 1e-3 };
        foreach (var rminVal in rminVals)
        {
            var (rListTemp, yListTemp) = solveODE.driver(exB.Schrodinger(E0), (rminVal, rmax), y0, 0.1);
            using (var file = new StreamWriter($"wave_rmin_{rminVal}.dat"))
            {
                for (int i = 0; i < rListTemp.Count; i++)
                {
                    double r = rListTemp[i];
                    double f = yListTemp[i][0];
                    file.WriteLine($"{r} {f}");
                }
            }
        }

        double[] accVals = { 1e-7, 1e-6, 1e-5, 1e-4 };
        foreach (var accVal in accVals)
        {
            var (rListTemp, yListTemp) = solveODE.driver(exB.Schrodinger(E0), (rmin, rmax), y0, 0.1, accVal);
            using (var file = new StreamWriter($"wave_acc_{accVal}.dat"))
            {
                for (int i = 0; i < rListTemp.Count; i++)
                {
                    double r = rListTemp[i];
                    double f = yListTemp[i][0];
                    file.WriteLine($"{r} {f}");
                }
            }
        }

        double[] epsVals = { 1e-7, 1e-6, 1e-5, 1e-4 };
        foreach (var e in epsVals)
        {
            var (rListTemp, yListTemp) = solveODE.driver(exB.Schrodinger(E0), (rmin, rmax), y0, 0.1, acc, e);
            using (var file = new StreamWriter($"wave_eps_{e}.dat"))
            {
                for (int i = 0; i < rListTemp.Count; i++)
                {
                    double r = rListTemp[i];
                    double f = yListTemp[i][0];
                    file.WriteLine($"{r} {f}");
                }
            }
        }


        // EXERCISE C

        WriteLine($"\nEXERCISE C: Optimized Newton's method with quadratic line search:");

        WriteLine($"Running Rosenbrock...");
        var rootR_c = exC.newton(GradRosen, startR, 1e-6);
        WriteLine($"  Rosenbrock minimum approx at (x,y) = ({rootR_c[0]:F6}, {rootR_c[1]:F6}), f-||grad|| = {GradRosen(rootR_c).norm():E3}");

        WriteLine($"Running Himmelblau...");
        var rootH_c = exC.newton(GradHimmelblau, startH, 1e-6);
        WriteLine($"  Himmelblau minimum approx at (x,y) = ({rootH_c[0]:F6}, {rootH_c[1]:F6}), f-||grad|| = {GradHimmelblau(rootH_c).norm():E3}");

        return 0;
    }
}
