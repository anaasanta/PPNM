/*
Calculate using System.Math class [<cmath>] √2, 21/5, eπ, πe.
*/
using static System.Math;

public static class ex1
{
    static public int Main()
    {
        double sqrt2 = Sqrt(2.0);
        double fraction = System.Math.Pow(2, 1.0 / 5);
        double epi = Exp(PI);
        double pie = System.Math.Pow(PI, Exp(1.0));

        System.Console.WriteLine($"\nSqrt(2) = {sqrt2}\n");
        System.Console.WriteLine($"2^1/5 = {fraction}\n");
        System.Console.WriteLine($"e^pi = {epi}\n");
        System.Console.WriteLine($"pi^e = {pie}\n");

        for (int i = 1; i <= 10; i++)
        {
            double gammaValue = sfuns.fgamma(i);
            System.Console.WriteLine($"fgamma({i}) = {gammaValue:F6}");
        }

        return 0;
    }
}