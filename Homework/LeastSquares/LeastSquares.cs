using System;
using System.IO;
using static System.Console;
using static System.Math;

public static class LeastSquares
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
        vector c = lsfit(fs, x, Y, dY);

        double ln_a = c[0];
        double lambda = c[1];
        double a = Exp(ln_a);
        double halfLife = Log(2) / lambda;

        WriteLine("Results of the fit:");
        WriteLine($"a = {a}");
        WriteLine($"λ = {lambda}");
        WriteLine($"T₁/₂ = {halfLife}");

        using (StreamWriter file = new StreamWriter("exA_data.txt"))
        {
            file.WriteLine($"----- t  y  dy");
            for (int i = 0; i < x.size; i++)
            {
                file.WriteLine($"{x[i]} {y[i]} {dy[i]}");
            }
        }

        // Gnerate file with the fit curve
        using (StreamWriter file = new StreamWriter("exA_fit.txt"))
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
}
