using static System.Console;
using static System.Math;
using System;
using System.Collections.Generic;
using System.IO;

class exA
{
    static double b = 0.25;
    static double c = 5.0;

    // Solve u''=-u 
    //This is a second order equation, so we need to convert it to a system of first order equations.
    public static vector f(double x, vector y)
    {
        vector r = new vector(2);
        r[0] = y[1];
        r[1] = -y[0];
        return r;
    }

    public static vector exOscillator(double t, vector y)
    {
        // y[0] = theta, y[1] = omega
        vector dydt = new vector(2);
        dydt[0] = y[1];
        dydt[1] = -b * y[1] - c * Sin(y[0]);
        return dydt;

        // y0 = theta, y1 = omega = y0', y1' = omega' = -b * y1 - c * Sin(y0)
    }

    public static int Main(string[] args)
    {

        // FIRST EXAMPLE: u''=-u
        vector yinit = new vector(2);
        yinit[0] = 1; // u(0)=1
        yinit[1] = 0; // u'(0)=0
        double a = 0; // start point
        double b = 10; // end point
        var (xlist, ylist) = solveODE.driver(f, (a, b), yinit);

        using (StreamWriter file = new StreamWriter("out_exA.txt"))
        {
            for (int i = 0; i < xlist.Count; i++)
            {
                file.WriteLine($"{xlist[i]} {ylist[i][0]} {ylist[i][1]}");
            }
        }

        // SECOND EXAMPLE: oscillator with friction 
        vector y0 = new vector(2);
        y0[0] = PI - 0.1;
        y0[1] = 0.0;

        double t0 = 0;
        double t1 = 10;

        var (ts, ys) = solveODE.driver(exOscillator, (t0, t1), y0);

        using (StreamWriter file = new StreamWriter("out_exA_oscillator.txt"))
        {
            for (int i = 0; i < ts.Count; i++)
            {
                file.WriteLine($"{ts[i]} {ys[i][0]} {ys[i][1]}");
            }
        }
        return 0;

    }


}

