using static System.Console;
using static System.Math;
using System;
using System.IO;

public static class exC
{
    public static vector newton(
        Func<vector, vector> f /* the function to find the root of */
        , vector start        /* the start point */
        , double acc = 1e-2   /* accuracy goal: on exit ‖f(x)‖ should be <acc */
        , vector deltax = null /* optional deltax-vector for calculation of jacobian */
    )
    {
        double lambdamin = 1e-7; // minimum step size for line-search
        vector x = start.copy();
        vector fx = f(x);

        if (deltax == null)
        {
            deltax = x.map(xi => Max(Abs(xi), 1.0) * Math.Pow(2.0, -26.0));
        }
        matrix J = new matrix(x.size, x.size);
        do
        {
            // compute numerical Jacobian
            jacobian(f, x, fx, deltax, J);
            // QR decomposition
            (matrix Q, matrix R) = QRGS.decomp(J);
            // Newton step: solve J * Dx = -f(x)
            vector Dx = QRGS.solve(Q, R, -fx);

            // prepare quadratic line-search
            double lambda = 1.0;
            double phi0 = 0.5 * fx.norm() * fx.norm(); // φ(0) = 0.5 * ||f||^2
            double dphi0 = -fx.norm() * fx.norm(); // φ'(0) = -||f||^2
            vector z = null;
            vector fz = null;
            double phi = phi0;

            // backtracking with quadratic interpolation
            while (true)
            {
                z = x + lambda * Dx;
                fz = f(z); // ||f(z)|| = ||f(x + λDx)||
                double phiTrial = 0.5 * fz.norm() * fz.norm(); // φ(λ) = 0.5 * ||f(z)||^2
                if (phiTrial <= (1 - lambda / 2) * phi0) break; // same as proving ||f(z)||^2 <= (1 - λ/2) * ||f(x)||^2
                if (lambda < lambdamin) break;

                // quadratic interpolation to update lambda
                double c = (phiTrial - phi0 - dphi0 * lambda) / (lambda * lambda); // c = (φ(λ) - φ(0) - φ'(0) * λ) / λ^2
                double lambdaNext = -dphi0 / (2 * c);
                lambda = Math.Min(lambda / 2, Math.Max(lambdaNext, lambda / 4)); // λ = min(λ/2, max(λ_next, λ/4))
            }

            // update x
            x = z;
            fx = fz;

        } while (fx.norm() > acc);

        return x;
    }

    public static void jacobian(Func<vector, vector> f, vector x, vector fx, vector deltax, matrix J)
    {
        int n = x.size;
        for (int j = 0; j < n; j++)
        {
            double saved = x[j];
            x[j] += deltax[j];
            vector df = f(x) - fx;
            for (int i = 0; i < n; i++)
                J[i, j] = df[i] / deltax[j];
            x[j] = saved;
        }
    }
    // now is a void jacobian method that fills the matrix J directly
}
