using static System.Console;
using static System.Math;
using System;
using System.IO;
using System.Collections.Generic;

class main
{
    // Find minimum of the rosenbrocks valley function
    static double rosenbrock(vector v)
    {
        double x = v[0], y = v[1];
        return Pow(1 - x, 2) + 100 * Pow(y - x * x, 2);
    }

    static double himmelblau(vector v)
    {
        double x = v[0], y = v[1];
        return Pow(x * x + y - 11, 2) + Pow(x + y * y - 7, 2);
    }

    static int Main(string[] args)
    {

        // EXERCISE A

        // Initial guesses
        var startR = new vector(-1.2, 1.0);
        var startH = new vector(-2.0, 3.0);

        WriteLine($"Running Newton's method on Rosenbrock function...");
        var (rootR, iterR) = minimisation.newton(rosenbrock, startR);
        WriteLine($"Rosenbrock minimum: {rootR.print()}, iterations: {iterR}");
        WriteLine($"\nRunning Newton's method on Himmelblau's function...");
        var (rootH, iterH) = minimisation.newton(himmelblau, startH);
        WriteLine($"Himmelblau minimum: {rootH.print()}, iterations: {iterH}");


        // EXERCISE B

        var energy = new List<double>();
        var signal = new List<double>();
        var error = new List<double>();
        var separators = new char[] { ' ', '\t' };
        var options = StringSplitOptions.RemoveEmptyEntries;

        do
        {
            string line = Console.In.ReadLine();
            if (line == null) break; // exit on empty line
            string[] words = line.Split(separators, options);
            energy.Add(double.Parse(words[0], System.Globalization.CultureInfo.InvariantCulture));
            signal.Add(double.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture));
            error.Add(double.Parse(words[2], System.Globalization.CultureInfo.InvariantCulture));

        } while (true);


        // Initial guess: [m, Gamma, A]
        Func<vector, double> phi = v =>
        {
            double m = v[0];
            double Gamma = v[1];
            double A = v[2];
            if (Gamma <= 0.0 || A <= 0.0 || m < 100.0 || m > 140.0)
                return 1e10;
            double sum = 0.0;

            for (int i = 0; i < energy.Count; i++)
            {
                double e = energy[i];   // energy E_i
                double sig = signal[i];    // signal σ_i
                double err = error[i];   // error Δσ_i

                // Breit–Wigner:
                double model = A / ((e - m) * (e - m) + (Gamma * Gamma) / 4.0);

                double resid = (model - sig) / err; // residuals
                sum += resid * resid; // sum of squares

            }

            return sum;
        };


        var (rootBW, iterBW) = minimisation.newton(phi, new vector(125.0, 2.0, 10.0));

        // Write the initial guess and function value to a file
        WriteLine("\nRunning Newton's method on Breit-Wigner fit...");
        WriteLine($"Converged in {iterBW} iterations.");
        WriteLine($"Fitted parameters: m = {rootBW[0]}, Gamma = {rootBW[1]}, A = {rootBW[2]}");


        using (var file = new StreamWriter("breit_wigner_fit.dat"))
        { // Write the fitted model to a file
            for (int i = 0; i < energy.Count; i++)
            {
                double e = energy[i];
                double sig = signal[i];
                double model = rootBW[2] / ((e - rootBW[0]) * (e - rootBW[0]) + (rootBW[1] * rootBW[1]) / 4);
                file.WriteLine($"{e} {sig} {model}");
            }
        }


        // EXERCISE C
        WriteLine($"\n Rosenbrock minimum, with forward v central difference starting at {startR.print()}");
        var (rootR_f, iterR_f) = minimisation.newton(rosenbrock, startR, 1e-6);
        WriteLine($"\tForward difference:");
        WriteLine($"\t\tRosenbrock minimum: {rootR_f.print()}, iterations: {iterR_f}");
        WriteLine($"\n\tCentral difference:");
        var (rootR_c, iterR_c) = minimisation.newton_c(rosenbrock, startR, 1e-6);
        WriteLine($"\t\tRosenbrock minimum: {rootR_c.print()}, iterations: {iterR_c}");
        var startR2 = new vector(1.2, 1.0); // another starting point
        WriteLine($"\n Rosenbrock minimum, with forward v central difference, starting at {startR2.print()}");
        var (rootR2_f, iterR2_f) = minimisation.newton(rosenbrock, startR2, 1e-6);
        WriteLine($"\tForward difference:");
        WriteLine($"\t\tRosenbrock minimum: {rootR2_f.print()}, iterations: {iterR2_f}");
        WriteLine($"\n\tCentral difference:");
        var (rootR2_c, iterR2_c) = minimisation.newton_c(rosenbrock, startR2, 1e-6);
        WriteLine($"\t\tRosenbrock minimum: {rootR2_c.print()}, iterations: {iterR2_c}");


        WriteLine($"\n Himmelblau minimum, with forward v central difference starting at {startH.print()}");
        var (rootH_f, iterH_f) = minimisation.newton(himmelblau, startH, 1e-6);
        WriteLine($"\tForward difference:");
        WriteLine($"\t\tHimmelblau minimum: {rootH_f.print()}, iterations: {iterH_f}");
        WriteLine($"\n\tCentral difference:");
        var (rootH_c, iterH_c) = minimisation.newton_c(himmelblau, startH, 1e-6);
        WriteLine($"\t\tHimmelblau minimum: {rootH_c.print()}, iterations: {iterH_c}");
        var startH2 = new vector(2.0, 3.0); // another starting point
        WriteLine($"\n Himmelblau minimum, with forward v central difference, starting at {startH2.print()}");
        var (rootH2_f, iterH2_f) = minimisation.newton(himmelblau, startH2, 1e-6);
        WriteLine($"\tForward difference:");
        WriteLine($"\t\tHimmelblau minimum: {rootH2_f.print()}, iterations: {iterH2_f}");
        WriteLine($"\n\tCentral difference:");
        var (rootH2_c, iterH2_c) = minimisation.newton_c(himmelblau, startH2, 1e-6);
        WriteLine($"\t\tHimmelblau minimum: {rootH2_c.print()}, iterations: {iterH2_c}");


        return 0;
    }
}