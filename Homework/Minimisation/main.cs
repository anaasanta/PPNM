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
        var (rootR, iterR) = minimisation.newton(rosenbrock, startR, 1e-6);
        WriteLine($"Rosenbrock minimum: {rootR.print()}, iterations: {iterR}");
        WriteLine($"\nRunning Newton's method on Himmelblau's function...");
        var (rootH, iterH) = minimisation.newton(himmelblau, startH, 1e-6);
        WriteLine($"Himmelblau minimum: {rootH.print()}, iterations: {iterH}");


        // EXERCISE B

        var energy = new List<double>();
        var signal = new List<double>();
        var error = new List<double>();
        var separators = new char[] { ' ', ',', '\t' };
        var options = StringSplitOptions.RemoveEmptyEntries;
        do
        {
            string line = ReadLine();
            if (line == null) break; // exit on empty line
            string[] words = line.Split(separators, options);
            if (words.Length < 3) continue; // skip lines with insufficient data
            energy.Add(double.Parse(words[0]));
            signal.Add(double.Parse(words[1]));
            error.Add(double.Parse(words[2]));
        } while (true);

        // Convert lists to vectors
        vector energyVec = new vector(energy.ToArray());
        vector signalVec = new vector(signal.ToArray());
        vector errorVec = new vector(error.ToArray());

        // Initial guess: [m, Gamma, A]
        Func<vector, double> phi = v =>
        {
            double m = v[0];
            double Gamma = v[1];
            double A = v[2];
            double sum = 0.0;

            for (int i = 0; i < energyVec.size; i++)
            {
                double e = energyVec[i];   // energy E_i
                double sig = signalVec[i];    // signal σ_i
                double err = errorVec[i];   // error Δσ_i

                // Breit–Wigner:
                double model = A / ((e - m) * (e - m) + (Gamma * Gamma) / 4);

                double gdiff = (model - sig) / err;
                sum += gdiff * gdiff;
            }

            return sum;
        };

        var (rootBW, iterBW) = minimisation.newton(phi, new vector(125.0, 2.0, 10.0), 1e-6);

        // Write the initial guess and function value to a file
        WriteLine("\nRunning Newton's method on Breit-Wigner fit...");
        WriteLine($"Converged in {iterBW} iterations.");
        WriteLine($"Fitted parameters: m = {rootBW[0]}, Gamma = {rootBW[1]}, A = {rootBW[2]}");


        using (var file = new StreamWriter("breit_wigner_fit.dat"))
        { // Write the fitted model to a file
            for (int i = 0; i < energyVec.size; i++)
            {
                double e = energyVec[i];
                double sig = signalVec[i];
                double model = rootBW[2] / ((e - rootBW[0]) * (e - rootBW[0]) + (rootBW[1] * rootBW[1]) / 4);
                file.WriteLine($"{e} {sig} {model}");
            }
        }

        return 0;
    }
}