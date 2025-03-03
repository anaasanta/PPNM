using System;
using static System.Console;
using static System.Math;

class main
{
    static int Main(string[] args)
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
        /*
        To change the size of the matrix manually
        */
        int n = 5;
        int m = 3;
        foreach (var arg in args)
        {
            var words = arg.Split('=');
            if (words[0] == "n")
            {
                n = int.Parse(words[1]);
            }
            else if (words[0] == "m")
            {
                m = int.Parse(words[1]);
            }
        }

        WriteLine($"n = {n}, m = {m}\n\n");
        /*
        To create random matrix
        */

        //int n = rand.Next(3, 10); // needs to be a modest size 
        //int m = rand.Next(2, n);  // m<n

        matrix A = new matrix(n, m);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                A[i, j] = rand.NextDouble(); // random matrix A 
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


        WriteLine($"Check if R is upper triangular... \n");
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


        WriteLine($"Check if Q^T*Q = 1...\n");
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

        WriteLine($"Check if QR = A...\n");
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

            1. generate a random square matrix B (of a modest size); 
            2. generate a random vector b (of the same size);
            3. factorize B into QR; 
            4. solve QRx=b;
            5. check that Ax=b;
        */

        matrix B = new matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                B[i, j] = rand.NextDouble(); // random matrix B
            }
        }
        WriteLine($"B =");
        B.print();
        WriteLine($"\n\n");
        (matrix Q2, matrix R2) = QRGS.decomp(B);

        vector b = new vector(n);
        for (int i = 0; i < m; i++)
        {
            b[i] = rand.NextDouble(); // random vector b
        }

        WriteLine($"b =");
        b.print();
        WriteLine($"\n\n");


        WriteLine($"Solve QRx=b...\n");
        vector x = QRGS.solve(Q2, R2, b);
        WriteLine($"x =");
        x.print();
        WriteLine($"\n\n");


        WriteLine($"Check if Bx = b...");
        vector Bx = B * x;
        WriteLine($"Bx =");
        Bx.print();
        WriteLine($"\n");
        if (b.approx(Bx))
        {
            WriteLine($"------------Bx = b");
        }
        else
        {
            WriteLine($"------------Bx != b");
        }
        WriteLine($"\n\n");


        /*
        Check that you function works as intended:

            1. generate a random square matrix A (of a modest size) (we will use the same B as above);
            2. factorize A into QR; (already done)
            3. calculate the inverse B;
            4. check that AB=I, where I is the identity matrix;
        */

        matrix B_inv = QRGS.inverse(Q2, R2);
        WriteLine($"B^-1 =");
        B_inv.print();
        WriteLine($"\n\n");

        WriteLine($"Check if B*B^-1 = Id...\n");
        matrix Id2 = new matrix(n, n);
        Id2.set_identity();
        matrix BBinv = B * B_inv;
        WriteLine($"B*B^-1 =");
        BBinv.print();
        WriteLine($"\n");
        if (Id2.approx(BBinv))
        {
            WriteLine($"------------B*B^-1 = Id");
        }
        else
        {
            WriteLine($"------------B*B^-1 != Id");
        }
        WriteLine($"\n\n");

        return 0;



    }
}
