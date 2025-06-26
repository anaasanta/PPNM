using static System.Console;
using static System.Math;
using System;
using System.IO;

public static class minimisation
{

    public static (vector, int) newton(
        Func<vector, double> phi /* the function to find the minimum of */
        , vector x        /* the start point */
        , double acc = 1e-3   /* accuracy goal: on exit ‖∇f(x)‖ should be <acc */
    )
    {
        double lambdamin = 2e-7; /* minimum step size for linesearch */
        double alpha = 1e-4; /* scaling factor for the Hessian */
        int i = 0; /* iteration counter */
        vector z = new vector(x.size); /* temporary vector for the new point */

        do
        { /* Newton's iterations */
            vector g = gradient(phi, x);
            if (g.norm() < acc)
            {
                break; /* job done */
            }
            matrix H = hessian(phi, x);
            (matrix Q, matrix R) = QRGS.decomp(H);
            vector Dx = QRGS.solve(Q, R, -g); /* Newton's step */
            double lambda = 1;
            do
            { /* backtracking linesearch */
                z = x + lambda * Dx;
                if (phi(z) < phi(x) + alpha * lambda * Dx.dot(g))
                {
                    break; /* good step */
                }
                lambda /= 2;
            } while (lambda >= lambdamin);
            x = z;
            i++;
        } while (i < 1000); /* limit iterations to avoid infinite loops */

        return (x, i);
    }

    public static vector gradient(Func<vector, double> phi, vector x)
    {
        int n = x.size;
        double phix = phi(x);
        vector g = new vector(n);
        for (int i = 0; i < n; i++)
        {
            double dxi = (1 + Abs(x[i])) * Math.Pow(2.0, -26.0); /* finite difference step */
            x[i] += dxi;
            g[i] = (phi(x) - phix) / dxi; /* finite difference approximation */
            x[i] -= dxi; /* restore x */
        }
        return g;
    }

    public static matrix hessian(Func<vector, double> phi, vector x)
    {
        int n = x.size;
        matrix H = new matrix(n, n);
        vector g = gradient(phi, x);
        for (int j = 0; j < n; j++)
        {
            double dxj = (1 + Abs(x[j])) * Math.Pow(2.0, -13.0); /* finite difference step */
            x[j] += dxj;
            vector dgphi = gradient(phi, x) - g; /* gradient at perturbed x */
            for (int i = 0; i < n; i++)
            {
                H[i, j] = dgphi[i] / dxj; /* finite difference approximation */
            }
            x[j] -= dxj; /* restore x */
        }
        return H;
    }


    // EXERCISE C

    public static vector gradient_c(Func<vector, double> phi, vector x)
    {
        int n = x.size;
        vector g = new vector(n);
        double fx;
        double[] fplus = new double[n];
        double[] fminus = new double[n];

        // precompute function value at x
        fx = phi(x);
        // Compute f(x +- dxj) for each dimension
        for (int j = 0; j < n; j++)
        {
            double dxj = (1 + Abs(x[j])) * Math.Pow(2.0, -26.0); // finite difference step
            x[j] += dxj;
            fplus[j] = phi(x); // f(x + dxj)
            x[j] -= 2 * dxj; // f(x - dxj)
            fminus[j] = phi(x); // f(x - dxj)
            x[j] += dxj; // restore x

        }
        // Compute gradient using central difference
        for (int i = 0; i < n; i++)
        {
            g[i] = (fplus[i] - fminus[i]) / (2 * (1 + Abs(x[i])) * Math.Pow(2.0, -26.0));
        }
        return g;

    }

    public static matrix hessian_c(Func<vector, double> phi, vector x)
    {
        int n = x.size;
        matrix H = new matrix(n, n);
        double[] h = new double[n];
        double phi0 = phi(x);
        double[] fplus = new double[n];
        double[] fminus = new double[n];

        // diagonal terms
        for (int j = 0; j < n; j++)
        {
            h[j] = (1 + Abs(x[j])) * Pow(2.0, -26);
            x[j] += h[j];
            fplus[j] = phi(x);
            x[j] -= 2 * h[j];
            fminus[j] = phi(x);
            x[j] += h[j];
            H[j, j] = (fplus[j] - 2 * phi0 + fminus[j]) / (h[j] * h[j]);
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                x[i] += h[i]; x[j] += h[j]; double fpp = phi(x);
                x[j] -= 2 * h[j]; double fpm = phi(x);
                x[i] -= 2 * h[i]; x[j] += 2 * h[j]; double fmp = phi(x);
                x[j] -= 2 * h[j]; double fmm = phi(x);
                x[i] += h[i]; x[j] += h[j]; // restore

                double val = (fpp - fpm - fmp + fmm) / (4 * h[i] * h[j]);
                H[i, j] = val;
                H[j, i] = val;
            }
        }
        return H;
    }

    public static (vector, int) newton_c(
        Func<vector, double> phi /* the function to find the minimum of */
        , vector start        /* the start point */
        , double acc = 1e-3   /* accuracy goal: on exit ‖∇f(x)‖ should be <acc */
    )
    {
        double lambdamin = 2e-10; /* minimum step size for linesearch */
        double alpha = 10e-4; /* scaling factor for the Hessian */
        int i = 0; /* iteration counter */
        vector x = start.copy();
        vector z;

        do
        { /* Newton's iterations */
            vector g = gradient_c(phi, x);
            if (g.norm() < acc)
            {
                break; /* job done */
            }
            matrix H = hessian_c(phi, x);
            (matrix Q, matrix R) = QRGS.decomp(H);
            vector Dx = QRGS.solve(Q, R, -g); /* Newton's step */
            double lambda = 1;
            do
            { /* backtracking linesearch */
                z = x + lambda * Dx;
                if (phi(z) < phi(x) + alpha * lambda * Dx.dot(g))
                {
                    break; /* good step */
                }
                lambda /= 2;
            } while (lambda >= lambdamin);
            x = z;
            i++;
        } while (i < 1000); /* limit iterations to avoid infinite loops */

        return (x, i);
    }

}