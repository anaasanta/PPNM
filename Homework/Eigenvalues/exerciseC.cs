using System;
using System.Diagnostics;
using static System.Console;
using static System.Math;

class exerciseC
{
    static int Main(string[] args)
    {
        int n = 5;
        int rep = 10;
        foreach (var arg in args)
        {
            var words = arg.Split('=');
            if (words[0] == "n")
            {
                n = int.Parse(words[1]);
            }
            if (words[0] == "repetitions")
            {
                rep = int.Parse(words[1]);
            }
        }

        // random matrix
        Random rand = new Random();
        matrix A = new matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                A[i, j] = rand.NextDouble();
                A[j, i] = A[i, j];
            }
        }

        // measure diagonalization time with Stopwatch
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < rep; i++)
        {
            (vector d, matrix V) = jacobi.cyclic(A.copy());
        }
        sw.Stop();

        WriteLine($"{n} {sw.Elapsed.TotalSeconds}");
        return 0;
    }
}
