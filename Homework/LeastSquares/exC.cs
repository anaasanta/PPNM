using System;
using System.IO;
using static System.Console;
using static System.Math;

public static class exC
{
    public static int Main()
    {

        double[] time = { 1, 2, 3, 4, 6, 9, 10, 13, 15 };
        double[] activity = { 117, 100, 88, 72, 53, 29.5, 25.2, 15.2, 11.1 };
        double[] deltay = { 6, 5, 4, 4, 4, 3, 3, 2, 2 };


        vector x = new vector(time);
        vector y = new vector(activity);
        vector dy = new vector(deltay);


        vector Y = new vector(x.size);
        vector dY = new vector(x.size);
        for (int i = 0; i < x.size; i++)
        {
            Y[i] = Log(y[i]);
            dY[i] = dy[i] / y[i];
        }


        Func<double, double>[] fs = { t => 1.0, t => -t };
        (vector c, matrix cov) = lsfit(fs, x, Y, dY);

        double ln_a = c[0];
        double lambda = c[1];
        double a = Exp(ln_a);
        double halfLife = Log(2) / lambda;
        double delta_ln = Sqrt(cov[0, 0]);
        double delta_lambda = Sqrt(cov[1, 1]);
        double delta_halfLife = Log(2) / (lambda * lambda) * delta_lambda;

        WriteLine($"Results of the fit:");
        WriteLine($"a = {a}");
        WriteLine($"λ = {lambda} ± {delta_lambda}");
        WriteLine($"T₁/₂ = {halfLife} ± {delta_halfLife}");

        // Generate file with the ajusted data varying the coefficients
        // F_best(x) = c0 + c1*x,
        // F_+(x) = (c0+δc0) + (c1+δc1)*x,
        // F_-(x) = (c0-δc0) + (c1-δc1)*x.

        using (StreamWriter file = new StreamWriter("exC_fits.txt"))
        {
            file.WriteLine("----- t   F_best   F_+  F_-");
            for (int i = 0; i < x.size; i++)
            {
                double t_val = x[i];
                // Exponential function for the orginal state
                double F_best = Exp((c[0]) - (c[1]) * t_val);
                double F_p = Exp((c[0] + delta_ln) - (c[1] + delta_lambda) * t_val);
                double F_n = Exp((c[0] - delta_ln) - (c[1] - delta_lambda) * t_val);

                file.WriteLine($"{t_val} {F_best} {F_p} {F_n}");
            }
        }

        return 0;
    }

    public static (vector, matrix) lsfit(Func<double, double>[] fs, vector x, vector y, vector dy)
    { // In this one, we also calculate the covariance matrix and the uncertanties of the fitting coefficients
        int n = x.size;
        int m = fs.Length;
        matrix A = new matrix(n, m);
        vector b = new vector(n);
        for (int i = 0; i < n; i++)
        {
            b[i] = y[i] / dy[i];
            for (int k = 0; k < m; k++)
            {
                A[i, k] = fs[k](x[i]) / dy[i];
            }
        }
        var (Q, R) = QRGS.decomp(A);
        vector c = QRGS.solve(Q, R, b);

        matrix Atrans = A.T;
        matrix AT = Atrans * A;
        (matrix QAT, matrix RAT) = QRGS.decomp(AT);
        matrix cov = QRGS.inverse(QAT, RAT);

        return (c, cov);

    }
}
