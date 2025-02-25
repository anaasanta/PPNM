/*
QR decomposition with Gram-Schmidt orthogonalization

Functions:
(matrix,matrix) decomp(matrix A) - returns the QR decomposition of A
vector solve(matrix Q, matrix R, vector b) - solves the linear equation Q*R*x=b
double det(matrix R) - returns the determinant of R
matrix inverse(matrix Q,matrix R) - returns the inverse of R
*/

using System;
using static System.Console;
using static System.Math;
using static System.Double;
using static System.Array;

public static class QRGS
{
    public static (matrix, matrix) decomp(matrix A)
    {
        matrix Q = A.copy();
        matrix R = new matrix(A.size2, A.size2);
        int m = A.size2;

        for (int i = 0; i < m; i++)
        {
            double Rii = Q[i].norm(); // norm of ai
            R[i, i] = Rii;
            Q[i] /= Rii; //normalize ai
            for (int j = i + 1; j < m; j++)
            {
                double Rij = Q[i].dot(Q[j]); // dot product of ai and aj
                R[i, j] = Rij;
                Q[j] -= Q[i] * Rij; //subtract the projection of ai onto aj from aj
            }
        }
        return (Q, R);
    }
    public static vector solve(matrix Q, matrix R, vector b)
    {
        // Solve Q*R*x=b
        // 1. R*x = Q^T*b --> y = Q^T*b
        // 2. Solve R*x = y using back-substitution
        vector y = Q.T * b;
        vector x = new vector(R.size2);
        // use back-subsitution method in matrix.cs
        x = R.backsub(y);
        return x;

    }
    public static double det(matrix R)
    { // det(A) = det(QR) = det(Q)*det(R) = det(R), because Q is ortogonal((det(Q)^2=1))
        double det = 1;
        for (int i = 0; i < R.size1; i++)
        {
            det *= R[i, i];
        }
        return det;
    }
    //public static matrix inverse(matrix Q, matrix R) { ... }
}
