using static System.Console;
using static System.Math;
using System;
using System.IO;

/* your main should get rmax and dr from the command line, like this */
/* mono main.exe rmax=10 dr=0.3 */

class exerciseB
{
    static int Main(string[] args)
    {

        // 1. NUMERICAL CALCULATION
        double rmax = 10;
        double dr = 0.3;

        string mode = "dr";

        foreach (var arg in args)
        {
            var words = arg.Split('=');
            if (words[0] == "rmax") rmax = double.Parse(words[1]);
            if (words[0] == "dr") dr = double.Parse(words[1]);
            if (words[0] == "mode") mode = words[1];
        }

        int npoints = (int)(rmax / dr) - 1;
        vector r = new vector(npoints);
        for (int i = 0; i < npoints; i++) r[i] = dr * (i + 1); // r[0] = dr, r[1] = 2*dr, ..., r[npoints-1] = rmax-dr 
        matrix H = new matrix(npoints, npoints); // Hamiltonian matrix H
        for (int i = 0; i < npoints - 1; i++)
        {
            H[i, i] = -2 * (-0.5 / dr / dr);
            H[i, i + 1] = 1 * (-0.5 / dr / dr);
            H[i + 1, i] = 1 * (-0.5 / dr / dr);
        }
        H[npoints - 1, npoints - 1] = -2 * (-0.5 / dr / dr);
        for (int i = 0; i < npoints; i++) H[i, i] += -1 / r[i];

        // Diagonalize the matrix using your Jacobi routine and obtain the eigenvalues and igenvectors
        (vector eigenval, matrix eigenvect) = jacobi.cyclic(H);

        // We normalize the eigenvectors for the wavefunctions on part 3
        for (int j = 0; j < npoints; j++)
        {
            for (int i = 0; i < npoints; i++)
            {
                eigenvect[i, j] /= Sqrt(dr);
            }
        }

        if (mode == "dr")
        {
            using (StreamWriter file = new StreamWriter("convergence_dr.txt", append: true))
            {
                file.WriteLine($"{dr}, {eigenval[0]}");
            }
        }
        else if (mode == "rmax")
        {
            using (StreamWriter file = new StreamWriter("convergence_rmax.txt", append: true))
            {
                file.WriteLine($"{rmax}, {eigenval[0]}");
            }
        }
        // 3. WAVE FUNCTIONS 
        /*
        - We will plot 3 eigenfunctions for the hydrogen atom and save them to a file called wavefunctions.txt
        - The hydrogen s-wave reduced radial eigen-function f(k)(r) with the radial quantum number k is represented by
        the k'th eigenvector of our Hamiltonian matrix. That is, by the k'th column of the matrix eigenvec
        - We've already normalized the eigenvectors for the wavefunctions.
        - The eigenfunctions are given by
            f(k)(r) = Const × eigenvec[i,k] 
        - The constant is given by Const = 1/√(Δr)
        */
        else if (mode == "wavefunction")
        {
            using (StreamWriter file = new StreamWriter("wavefunction.txt", append: true))
            {

                for (int i = 0; i < npoints; i++)
                {
                    double f1 = 1.0 / Sqrt(dr) * eigenvect[i, 1];
                    double f2 = 1.0 / Sqrt(dr) * eigenvect[i, 2];
                    double f3 = 1.0 / Sqrt(dr) * eigenvect[i, 3];
                    double f4 = 1.0 / Sqrt(dr) * eigenvect[i, 4];
                    double f5 = 1.0 / Sqrt(dr) * eigenvect[i, 5];
                    file.WriteLine($"{r[i]} {f1} {f2} {f3} {f4} {f5}");
                }
                file.WriteLine("\n\n");

            }
        }
        return 0;

    }

}