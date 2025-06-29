using static System.Console;
using static System.Math;
using System;
using System.Collections.Generic;

public class solveODE
{

    public static (vector, vector) rkstep12(
    Func<double, vector, vector> f,/* the f from dy/dx=f(x,y) */
    double x,                    /* the current value of the variable */
    vector y,                    /* the current value y(x) of the sought function */
    double h                     /* the step to be taken */
    )
    {
        vector k0 = f(x, y);              /* embedded lower order formula (Euler) */
        vector k1 = f(x + h / 2, y + k0 * (h / 2)); /* higher order formula (midpoint) */
        vector yh = y + k1 * h;              /* y(x+h) estimate */
        vector δy = (k1 - k0) * h;           /* error estimate */
        return (yh, δy);
    }


    public static (List<double>, List<vector>) driver(
    Func<double, vector, vector> F,/* the f from dy/dx=f(x,y) */
    (double, double) interval,    /* (initial-point,final-point) */
    vector yinit,                /* y(initial-point) */
    double h = 0.125,              /* initial step-size */
    double acc = 0.01,             /* absolute accuracy goal */
    double eps = 0.01              /* relative accuracy goal */
)
    {
        var (a, b) = interval;
        double x = a;
        vector y = yinit.copy();

        var xlist = new List<double>();
        xlist.Add(x);
        var ylist = new List<vector>();
        ylist.Add(y);
        do
        {
            if (x >= b)
            {
                return (xlist, ylist); /* job done */
            }
            if (x + h > b)
            {
                h = b - x; /* last step should end at b */
            }

            var (yh, δy) = rkstep12(F, x, y, h); // take a step

            double tol = (acc + eps * yh.norm()) * Sqrt(h / (b - a));
            double err = δy.norm();

            if (err <= tol)
            { // accept step
                x += h; y = yh;
                xlist.Add(x);
                ylist.Add(y);
            }
            if (err > 0)
            {
                h *= Min(Pow(tol / err, 0.25) * 0.95, 2); // readjust stepsize
            }
            else
            {
                h *= 2; // if the error is zero, increase the step size
            }
        } while (true);
    }//driver

    public static (vector, vector) rk23step(Func<double, vector, vector> f, double x, vector y, double h)
    {
        // Coefficients for Bogacki-Shampine RK23
        vector k1 = f(x, y);
        vector k2 = f(x + h / 2, y + k1 * (h / 2));
        vector k3 = f(x + 3 * h / 4, y + k2 * (3 * h / 4));
        // third order estimate
        vector y3 = y + (h / 9) * (2 * k1 + 3 * k2 + 4 * k3);
        // second order estimate
        vector k4 = f(x + h, y3);
        vector y2 = y + (h / 24) * (7 * k1 + 6 * k2 + 8 * k3 + 3 * k4);
        vector err = y3 - y2;
        return (y3, err);
    }

    public static (List<double>, List<vector>, List<double>) driver23(Func<double, vector, vector> F, (double, double) interval, vector yinit, double h = 0.125)
    {
        var (a, b) = interval;
        double x = a;
        vector y = yinit.copy();

        var xlist = new List<double>();
        xlist.Add(x);
        var ylist = new List<vector>();
        ylist.Add(y);
        var hlist = new List<double>();
        hlist.Add(h);

        do
        {
            if (x >= b)
            {
                return (xlist, ylist, hlist); /* job done */
            }
            if (x + h > b)
            {
                h = b - x; /* last step should end at b */
            }

            var (yh, deltay) = rk23step(F, x, y, h); // take a step

            x += h;
            y = yh;
            xlist.Add(x);
            ylist.Add(y);
            hlist.Add(h);

            h *= 2;

        } while (true);
    }//driver23


}