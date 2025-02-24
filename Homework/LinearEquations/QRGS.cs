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
using static Homework.LinearEquations.matrix;

public static class QRGS
{
    public static (matrix, matrix) decomp(matrix A)
    {
        matrix Q = A.copy(); 
        matrix R = new matrix(A.size2, A.size2);
        
        /* orthogonalize Q and fill-in R */
        return (Q, R);
    }
    public static vector solve(matrix Q, matrix R, vector b) { ... }
    public static double det(matrix R) { ... }
    public static matrix inverse(matrix Q, matrix R) { ... }
}
