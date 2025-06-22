using static System.Console;
using static System.Math;
using System;
using System.IO;

public static class exA
{
    public static vector newton(
    Func<vector, vector> f /* the function to find the root of */
    , vector start        /* the start point */
    , double acc = 1e-2     /* accuracy goal: on exit ‖f(x)‖ should be <acc */
    , vector deltax = null      /* optional deltax-vector for calculation of jacobian */
    )
    {
        double lambdamin = 1e-7; /* minimum step size for linesearch */
        vector x = start.copy();
        vector fx = f(x), z, fz;

        do
        { /* Newton's iterations */
            if (fx.norm() < acc)
            {
                break; /* job done */
            }
            matrix J = jacobian(f, x, fx, deltax);
            (matrix Q, matrix R) = QRGS.decomp(J);
            vector Dx = QRGS.solve(Q, R, -fx); /* Newton's step */
            double lambda = 1;
            do
            { /* linesearch */
                z = x + lambda * Dx;
                fz = f(z);
                if (fz.norm() < (1 - lambda / 2) * fx.norm())
                {
                    break; /* sufficient decrease condition */
                }
                if (lambda < lambdamin)
                {
                    break; /* if lambda is too small, stop */
                }
                lambda /= 2;
            } while (true);
            x = z;
            fx = fz;
        } while (true);

        return x;
    }


    public static matrix jacobian(Func<vector, vector> f, vector x, vector fx = null, vector deltax = null)
    {
        int n = x.size;
        if (fx == null)
        {
            fx = f(x);
        }
        if (deltax == null)
        {
            deltax = x.map(xi => Max(Abs(xi), 1.0) * Math.Pow(2.0, -26.0));
        }
        for (int i = 0; i < n; i++)
        {
            deltax[i] = Max(Math.Abs(x[i]), 1.0) * Math.Pow(2.0, -26.0);
        }
        matrix J = new matrix(n, n);

        for (int j = 0; j < n; j++)
        {
            x[j] += deltax[j];
            vector df = f(x) - fx;
            for (int i = 0; i < x.size; i++)
            {
                J[i, j] = df[i] / deltax[j];
            }
            x[j] -= deltax[j];
        }

        return J;
    }


}