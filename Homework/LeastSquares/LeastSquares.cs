using System;
using System.IO;
using static System.Console;
using static System.Math;

public static class LeastSquares
{
    public static int Main()
    {

        double[] time = { 1, 2, 3, 4, 6, 9, 10, 13, 15 };  // Time in days
        double[] activity = { 117, 100, 88, 72, 53, 29.5, 25.2, 15.2, 11.1 }; // Activity y of ThX in relative units
        double[] deltay = { 6, 5, 4, 4, 4, 3, 3, 2, 2 }; // Uncertainties dy of the activity

        vector x = new vector(time);
        vector y = new vector(activity);
        vector dy = new vector(deltay);

        vector Y = new vector(x.size);
        vector dY = new vector(x.size);

        for (int i = 0; i < x.size; i++)
        {
            Y[i] = Log(y[i]);  // Logarithm of the activity
            dY[i] = dy[i] / y[i]; // Uncertainty of the logarithm: dy/y
        }

        Func<double, double>[] fs = { t => 1.0, t => -t }; // Functions for the linear fit: f(t) = a * exp(-λt) -> ln(f(t)) = ln(a) - λt
        (vector c, matrix cov) = lsfit(fs, x, Y, dY);

        double ln_a = c[0];
        double lambda = c[1];
        double a = Exp(ln_a);
        double halfLife = Log(2) / lambda;
        double delta_lambda = Sqrt(cov[1, 1]);
        double delta_halfLife = Log(2) / (lambda * lambda) * delta_lambda;


        WriteLine($"Results of the fit:");
        WriteLine($"a = {a}");
        WriteLine($"λ = {lambda}");
        WriteLine($"T₁/₂ = {halfLife}");
        WriteLine($"δλ = {delta_lambda}");
        WriteLine($"δT₁/₂ = {delta_halfLife}");

        using (StreamWriter file = new StreamWriter("out_exA_data.txt"))
        {
            file.WriteLine($"----- t  y  dy halflife");
            for (int i = 0; i < x.size; i++)
            {
                file.WriteLine($"{x[i]} {y[i]} {dy[i]} {halfLife}");
            }
        }

        // Generate file with the fit curve
        using (StreamWriter file = new StreamWriter("out_exA_fit.txt"))
        {
            file.WriteLine($"------- t  y_fit");
            for (int i = 0; i < x.size; i++)
            {
                double t = x[i];
                double y_fit = a * Exp(-lambda * t);
                file.WriteLine($"{t} {y_fit}");
            }
        }
        return 0;
    }

    //LSFIT FROM EXERCISE B
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


    /* //LSFIT FROM EXERCISE A
    public static vector lsfit(Func<double, double>[] fs, vector x, vector y, vector dy)
    {
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
        return c;
    }
    */

}
