using static System.Console;
using static System.Math;
using System;
using System.Collections.Generic;
using System.IO;

public class exB
{
    static double eps;

    // Given hint: y[0]=u, y[1]=du/dφ
    // y0' = y1
    // y1' = 1 - y0 + eps*y0^2
    public static vector f(double phi, vector y)
    {
        var dydphi = new vector(2);
        dydphi[0] = y[1];
        dydphi[1] = 1.0 - y[0] + eps * y[0] * y[0];
        return dydphi;
    }
    static void calculate_fixed(string filename, double epsilon, double u0, double du0, double phiMax = 8 * PI, double h = 0.001)
    {
        eps = epsilon;
        double phi = 0.0;
        vector y = new vector(2);
        y[0] = u0;
        y[1] = du0;

        using (var w = new StreamWriter(filename))
        {
            while (phi <= phiMax)
            {
                w.WriteLine($"{phi:F6} {y[0]:F6}");
                var dydphi = f(phi, y);
                y = y + h * dydphi;
                phi += h;
            }
        }
    }
    static void calculate(string filename, double epsilon, double u0, double du0, double phiMax = 8 * PI)
    {
        eps = epsilon;
        var yinit = new vector(2);
        yinit[0] = u0;
        yinit[1] = du0;
        double acc = 1e-6, eps_tol = 1e-6;
        var (phis, us) = solveODE.driver(f, (0.0, phiMax), yinit, acc, eps_tol);

        using (var w = new StreamWriter(filename))
        {
            for (int i = 0; i < phis.Count; i++)
            {
                w.WriteLine($"{phis[i]:F6} {us[i][0]:F6}");
            }
        }
    }

    public static int Main(string[] args)
    {
        // 1) Newtonian circular: eps=0, u'(0)=0 (PASO FIJO)
        calculate_fixed("out_exB_circular.txt", 0.0, 1.0, 0.0);

        // 2) Newtonian elliptical: eps=0, u'(0)=-0.5 (DRIVER)
        calculate("out_exB_ellipse.txt", 0.0, 1.0, -0.5);

        // 3) Relativistic precession: eps=0.01, u'(0)=-0.5 (DRIVER)
        calculate("out_exB_precession.txt", 0.01, 1.0, -0.5);

        return 0;
    }
}
