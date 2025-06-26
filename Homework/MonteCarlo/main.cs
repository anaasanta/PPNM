using static System.Console;
using static System.Math;
using System;
using System.IO;

class main
{
    static int Main(string[] args)
    {
        // EXERCISE A: calculate the area of a unit circle

        int N = 10000; // number of sampling points
        double r = 1.0; // radius of the unit circle

        Func<vector, double> f = (x) => (x[0] * x[0] + x[1] * x[1] <= r * r) ? 1.0 : 0.0; // function to integrate

        vector a = new vector(2); // lower limit of integration
        vector b = new vector(2); // upper limit of integration

        a[0] = -r; a[1] = -r; // set lower limit
        b[0] = r; b[1] = r; // set upper limit

        using (var file = new StreamWriter("out_area_uc.txt"))
        {
            for (int i = 0; i < N; i += 100)
            {
                var result = mc.plainmc(f, a, b, i); // perform Monte Carlo integration
                double area = result.Item1; // estimated area
                double error = result.Item2; // estimated error
                double actual_error = Abs(area - PI); // actual error compared to the known value of PI

                file.WriteLine($"{i} {area} {error} {actual_error} {1 / Sqrt(i)}");

            }
        }

        // second function to calculate 
        f = (x) => 1.0 / (1.0 - Cos(x[0]) * Cos(x[1]) * Cos(x[2])); // function to integrate

        a = new vector(3); // lower limit of integration
        b = new vector(3); // upper limit of integration

        a[0] = 0; a[1] = 0; a[2] = 0; // set lower limit
        b[0] = PI; b[1] = PI; b[2] = PI; // set upper limit

        using (var file = new StreamWriter("out_exA_integral.txt"))
        {
            file.WriteLine("N \t\tVolume \t\t\tError \t\t\t\tActualError");
            var result = mc.plainmc(f, a, b, 1000000); // perform Monte Carlo integration with 0 points to get the volume
            double volume = result.Item1 / (PI * PI * PI); // volume of the integration domain
            double error = result.Item2 / (PI * PI * PI); // estimated error
            file.WriteLine($"{N} {volume} {error} {Abs(volume - 1.3932039296856768591842462603255)}");


        }

        // EXERCISE B and C

        f = (x) => (x[0] * x[0] + x[1] * x[1] <= r * r) ? 1.0 : 0.0; // function to integrate
        a = new vector(2); // lower limit of integration
        b = new vector(2); // upper limit of integration
        a[0] = -r; a[1] = -r; // set lower limit
        b[0] = r; b[1] = r; // set upper limit

        using (var file = new StreamWriter("out_exB.txt"))
        {
            for (int i = 0; i < N; i += 100)
            {
                var resultQ = mc.quasirandmc(f, a, b, i); // perform quasi-random Monte Carlo integration
                var resultP = mc.plainmc(f, a, b, i); // perform pseudo-random Monte Carlo integration
                double areaP = resultP.Item1; // estimated area
                double errorP = resultP.Item2; // estimated error
                double actual_errorP = Abs(areaP - PI); // actual error compared to the known value of PI
                double areaQ = resultQ.Item1; // estimated area
                double errorQ = resultQ.Item2; // estimated error
                double actual_errorQ = Abs(areaQ - PI); // actual error compared to the known value of PI

                file.WriteLine($"{i} {areaP} {errorP} {actual_errorP} {areaQ} {errorQ} {actual_errorQ} {1 / Sqrt(i)}");

            }

        }

        using (var file = new StreamWriter("out_pseuVquaVstra.txt"))
        {
            var resultQ = mc.quasirandmc(f, a, b, 10000); // perform quasi-random Monte Carlo integration
            var resultP = mc.plainmc(f, a, b, 10000); // perform pseudo-random Monte Carlo integration
            var resultS = mc.strata(f, a, b); // perform stratified Monte Carlo integration
            double areaS = resultS.Item1;
            double errS = resultS.Item2;
            double areaP = resultP.Item1; // estimated area
            double errorP = resultP.Item2; // estimated error
            double actual_errorP = Abs(areaP - PI); // actual error compared to the known value of PI
            double areaQ = resultQ.Item1; // estimated area
            double errorQ = resultQ.Item2; // estimated error
            double actual_errorQ = Abs(areaQ - PI); // actual error compared to the known value of PI
            file.WriteLine($"Quasi-random: Area = {areaQ}, Error = {errorQ}, Actual Error = {actual_errorQ}");
            file.WriteLine($"Pseudo-random: Area = {areaP}, Error = {errorP}, Actual Error = {actual_errorP}");
            file.WriteLine($"Stratified: Area = {areaS}, Error = {errS}, Actual Error = {Math.Abs(areaS - PI)}");
        }




        return 0;
    }


}