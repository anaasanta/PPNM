using System;
using static System.Console;
using static System.Math;

class main
{
    static int Main(string[] args)
    {
        /*
        Check that your "jacobi.cs" works as intended:

            1. generate a random symmetric matrix A
            2. apply your routine to perform the eigenvalue-decomposition
            3. check that A = V*D*V^T (V is the orthogonal matrix of eigenvectors, D is the diagonal matrix of eigenvalues)
            4. check that V^T*A*V == D, V*D*V^T == A, V^T*V==1, V*V^T==1
        */

        Random rand = new Random();
        /*
        To change the size of the matrix manually
        */
        int n = 5;
        foreach (var arg in args)
        {
            var words = arg.Split('=');
            if (words[0] == "n")
            {
                n = int.Parse(words[1]);
            }
        }

        WriteLine($"n = {n}\n\n");

        matrix A = new matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                A[i, j] = rand.NextDouble();
                A[j, i] = A[i, j];
            }
        }



        WriteLine($"A =");
        A.print();
        WriteLine($"\n\n");

        (vector d, matrix V) = jacobi.cyclic(A);

        matrix D = new matrix(n, n);
        D.set_identity();
        for (int i = 0; i < n; i++)
        {
            D[i, i] = d[i];
        }

        WriteLine($"V =");
        V.print();
        WriteLine($"\n");
        WriteLine($"D =");
        D.print();
        WriteLine($"\n");


        WriteLine($"Check that V^T*A*V = D... \n");

        matrix VTAV = V.T * A * V;
        WriteLine($"V^T*A*V =");
        VTAV.print();
        WriteLine($"\n");
        if (VTAV.approx(D))
        {
            WriteLine($"------------V^T*A*V = D");
        }
        else
        {
            WriteLine($"------------V^T*A*V != D");
        }
        WriteLine($"\n\n");



        WriteLine($"Check that V*D*V^T = A...\n");
        matrix VDVT = V * D * V.T;
        WriteLine($"V*D*V^T =");
        VDVT.print();
        WriteLine($"\n");
        if (VDVT.approx(A))
        {
            WriteLine($"------------V*D*V^T = A");
        }
        else
        {
            WriteLine($"------------V*D*V^T != A");
        }
        WriteLine($"\n\n");


        WriteLine($"Check that V^T*V = 1...\n");
        matrix VTV = V.T * V;
        WriteLine($"V^T*V =");
        VTV.print();
        WriteLine($"\n");
        if (VTV.approx(matrix.id(n)))
        {
            WriteLine($"------------V^T*V = 1");
        }
        else
        {
            WriteLine($"------------V^T*V != 1");
        }
        WriteLine($"\n\n");


        WriteLine($"Check that V*V^T = 1...\n");
        matrix VVT = V * V.T;
        WriteLine($"V*V^T =");
        VVT.print();
        WriteLine($"\n");
        if (VVT.approx(matrix.id(n)))
        {
            WriteLine($"------------V*V^T = 1");
        }
        else
        {
            WriteLine($"------------V*V^T != 1");
        }
        WriteLine($"\n\n");



        return 0;



    }
}
