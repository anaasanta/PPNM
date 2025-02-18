/*
Calculate √-1, √i, ei, eiπ, ii, ln(i), sin(iπ) and compare (using our "approx" function) 
with manually calculated results (check them please before using):
*/

using static System.Console;
using static System.Math;
using complex = System.Numerics.Complex;
using Functions;

class main
{
    static int Main()
    {

        functions f = new functions();

        // 1. Calculate √-1
        complex a = complex.Sqrt(-1);
        WriteLine($"√-1 = {a}");
        WriteLine($"approximation = {complex.ImaginaryOne}");
        // compare using approx
        WriteLine($" {f.approx(a, complex.ImaginaryOne)}\n\n");

        // 2. Calculate √i
        complex b = complex.Sqrt(complex.ImaginaryOne);
        WriteLine($"√i = {b}");
        // ​≈0.707+0.707i
        WriteLine($"approximation = {0.70710678118 + 0.70710678118 * complex.ImaginaryOne}");
        // compare using approx
        WriteLine($" {f.approx(b, 0.70710678118 + 0.70710678118 * complex.ImaginaryOne)}\n\n");

        // 3. Calculate e^i
        complex c = complex.Exp(complex.ImaginaryOne);
        WriteLine($"e^i = {c}");
        // e^i = cos(1) + i sin(1) ≈ 0.540 + 0.841i
        WriteLine($"approximation = {0.540302305868 + 0.841470984807 * complex.ImaginaryOne}");
        // compare using approx
        WriteLine($" {f.approx(c, 0.540302305868 + 0.841470984807 * complex.ImaginaryOne)}\n\n");

        // 4. Calculate e^iπ
        complex d = complex.Exp(complex.ImaginaryOne * PI);
        WriteLine($"e^(iπ) = {d}");
        // e^(iπ) = cos(π) + i sin(π) = -1
        WriteLine($"approximation = {-1}");
        // compare using approx
        WriteLine($" {f.approx(d, -1)}\n\n");

        // 5. Calculate i^i
        complex e = complex.Pow(complex.ImaginaryOne, complex.ImaginaryOne);
        WriteLine($"i^i = {e}");
        // i^i = E^i^i = e^i*ln(i) = e^i*i*π/2 = e^(-π/2) ≈ 0.208
        WriteLine($"approximation = {0.208}");
        // compare using approx
        WriteLine($" {f.approx(e, 0.208)}\n\n");

        // 6. Calculate ln(i)
        complex g = complex.Log(complex.ImaginaryOne);
        WriteLine($"ln(i) = {g}");
        // ln(i) = ln(1) + i*π/2 = i*π/2 
        WriteLine($"approximation = {complex.ImaginaryOne * PI / 2}");
        // compare using approx
        WriteLine($" {f.approx(g, complex.ImaginaryOne * PI / 2)}\n\n");

        // 7. Calculate sin(iπ)
        complex h = complex.Sin(complex.ImaginaryOne * PI);
        WriteLine($"sin(iπ) = {h}");
        //sin(iπ)=isinh(π)≈11.548i        
        WriteLine($"approximation = {11.5487393 * complex.ImaginaryOne}");
        // compare using approx
        WriteLine($" {f.approx(h, 11.5487393 * complex.ImaginaryOne)}\n\n");


        return 0;
    }
}
