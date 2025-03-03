//main for exercise C

using System;
using static System.Console;
using static System.Math;

class mainC
{
    static int Main(string[] args)
    {


        Random rand = new Random();
        /*
        To change the size of the matrix manually
        */
        int n = 5;
        foreach (var arg in args)
        {
            var words = arg.Split(':');
            if (words[0] == "-size")
            {
                n = int.Parse(words[1]);
            }
        }

        WriteLine($"n = {n}\n\n");

        matrix A = new matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                A[i, j] = rand.NextDouble(); // random matrix A 
            }
        }

        (matrix Q, matrix R) = QRGS.decomp(A);
        return 0;



    }
}
