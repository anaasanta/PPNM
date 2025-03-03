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
            double Rii = Q[i].norm(); // norm of Qi
            R[i, i] = Rii;
            Q[i] /= Rii; //normalize Qi
            for (int j = i + 1; j < m; j++)
            {
                double Rij = Q[i].dot(Q[j]); // dot product of Qi and Qj
                R[i, j] = Rij;
                Q[j] -= Q[i] * Rij; //subtract the projection of ai onto aj from Qj
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

    public static matrix inverse(matrix Q, matrix R)
    {
        /*
        the inverse A-1 of a square nxn matrix A can be calculated by solving
        n linear equations Axi = ei, where ei is the unit vector in the i-direction
        The matrix made of columns xi is apparently the inverse of. A. 
        */

        matrix inv = new matrix(R.size1, R.size2);
        matrix Id = matrix.id(R.size1);
        for (int i = 0; i < R.size1; i++)
        {
            vector ei = Id[i];
            vector xi = solve(Q, R, ei);
            for (int j = 0; j < R.size2; j++)
            {
                inv[j, i] = xi[j];
            }

        }
        return inv;
    }
}
