using System;
using static System.Console;
using static System.Math;

class main
{
    static void Main()
    {
        /*
        Check that your "decomp" works as intended:

            1. generate a random tall (n>m) matrix A (of a modest size);
            2. factorize it into QR;
            3. check that R is upper triangular;
            4. check that QTQ=1;
            5. check that QR=A;
        */

        Random rand = new Random();
        int n = rand.Next(3, 10); // needs to be a modest size 
        int m = rand.Next(2, n);  // m<n
        matrix A = new matrix(n, m);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                A[i, j] = rand.Next(20); // random matrix A 
            }
        }


        (matrix Q, matrix R) = QRGS.decomp(A);
        matrix QTQ = Q.T * Q;
        matrix QR = Q * R;
        matrix Id = new matrix(QTQ.size1, QTQ.size2);
        Id.set_identity();


        WriteLine($"A =");
        A.print();
        WriteLine($"\n\n");


        WriteLine($"Check if R is upper triangular...");
        WriteLine($"R =");
        R.print();
        WriteLine($"\n");
        for (int i = 0; i < R.size1; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (R[i, j] != 0)
                {
                    WriteLine($"------------R is not upper triangular");
                    break;
                }
            }
        }
        WriteLine($"------------R is upper triangular \n\n");


        WriteLine($"Check if Q^T*Q = 1...");
        WriteLine($"Q =");
        Q.print();
        WriteLine($"\n");
        WriteLine($"Q^T*Q =");
        QTQ.print();
        WriteLine($"\n");
        if (QTQ.approx(Id))
        {
            WriteLine($"------------Q^T*Q = 1");
        }
        else
        {
            WriteLine($"------------Q^T*Q != 1");
        }
        WriteLine($"\n\n");

        WriteLine($"Check if QR = A...");
        WriteLine($"QR =");
        QR.print();
        WriteLine($"\n");
        if (A.approx(QR))
        {
            WriteLine($"------------QR = A");
        }
        else
        {
            WriteLine($"------------QR != A");
        }
        WriteLine($"\n\n");

        /*
        Check that you "solve" works as intended:

            1. generate a random square matrix A (of a modest size); (we use the same A as above)
            2. generate a random vector b (of the same size);
            3. factorize A into QR; (already done it)
            4. solve QRx=b;
            5. check that Ax=b;
        */

        vector b = new vector(n);
        for (int i = 0; i < m; i++)
        {
            b[i] = rand.Next(20); // random vector b
        }

        WriteLine($"b =");
        b.print();
        WriteLine($"\n\n");
        WriteLine($"Solve QRx=b...");
        vector x = QRGS.solve(Q, R, b);
        WriteLine($"x =");
        x.print();
        WriteLine($"\n\n");
        WriteLine($"Check if Ax = b...");
        vector Ax = A * x;
        WriteLine($"Ax =");
        Ax.print();
        WriteLine($"\n");
        if (b.approx(Ax))
        {
            WriteLine($"Ax = b");
        }
        else
        {
            WriteLine($"Ax != b");
        }
        WriteLine($"\n\n");

    }
}
