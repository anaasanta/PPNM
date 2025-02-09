/*
1. The maximum representable integer is the largest integer i for which i+1>i holds true. 
Using the while loop determine your maximum integer and compare it with "int.MaxValue". 

2. The minimum representable integer is the most negative integer i for which i-1<i holds true. 
Using the "while" loop determine your minimum integer and compare with "int.MinValue".

3. The machine epsilon is the difference between 1.0 and the next representable floating point number. 
Using the "while" loop calculate the machine epsilon for the types "float" and "double".

4. Suppose "tiny=epsilon/2". Calculate the two values,
    a=1+tiny+tiny;
    b=tiny+tiny+1;
    which should seemingly be the same and check whether "a==b", "a>1", "b>1"

5. Implement a function with the signature that returns "true" if the numbers "a" and "b" are equal either with absolute 
    precision "acc", or with relative precision "eps" and returns "false" otherwise. 
*/

using static System.Math;
using static System.Console;

class main
{
    static int Main()
    {
        var f = new functions();
        /*
         * Calculate the maximum representable integer
         */

        var i = f.maxInt();
        WriteLine($"My maximum representable integer is = {i}");
        WriteLine($"The maximum representable integer is {int.MaxValue}\n");
        if (i == int.MaxValue)
        {
            WriteLine($"The maximum representable integer is correct\n");
        }
        else
        {
            WriteLine($"The maximum representable integer is incorrect\n");
        }

        /*
         * Calculate the minimum representable integer
         */

        var j = f.minInt();
        WriteLine($"My minimum representable integer is = {j}");
        WriteLine($"The minimum representable integer is {int.MinValue}\n");
        if (j == int.MinValue)
        {
            WriteLine($"The minimum representable integer is correct\n");
        }
        else
        {
            WriteLine($"The minimum representable integer is incorrect\n");
        }

        /*
         * Calulate the machine epsilon for double and float
         */

        var x = f.machineEpsilonDouble();
        WriteLine($"My machine epsilon for double is = {x}");
        WriteLine($"The machine epsilon for double is {System.Math.Pow(2, -52)}\n");
        // See if both numbers are similar
        if (Abs(x - System.Math.Pow(2, -52)) < 0.00001)
        {
            WriteLine($"The machine epsilon for double is correct\n");

        }
        else
        {
            WriteLine($"The machine epsilon for double is incorrect\n");
        }


        var y = f.machineEpsilonFloat();
        WriteLine($"My machine epsilon for float is = {y}");
        WriteLine($"The machine epsilon for float is {System.Math.Pow(2, -23)}\n");
        // See if both numbers are similar
        if (Abs(y - System.Math.Pow(2, -23)) < 0.00001)
        {
            WriteLine($"The machine epsilon for float is correct\n");

        }
        else
        {
            WriteLine($"The machine epsilon for float is incorrect\n");
        }

        /*
         * Calculate the two values,
         * a=1+tiny+tiny;
         * b=tiny+tiny+1;
         * which should seemingly be the same and check whether "a==b", "a>1", "b>1"
         */
        double epsilon = System.Math.Pow(2, -52);
        double tiny = epsilon / 2;
        double a = 1 + tiny + tiny;
        double b = tiny + tiny + 1;
        WriteLine($"a = {a}, b = {b}\n");

        WriteLine($"a==b ? {a == b}\n");
        WriteLine($"a - b = {a - b}\n");

        WriteLine($"a>1  ? {a > 1}\n");
        WriteLine($"b>1  ? {b > 1}\n");

        /*
         * Absolute function
         */
        double acc = 1e-9;
        double eps = 1e-9;
        double a1 = 1.0;
        double b1 = 1.0 + acc / 2;
        double c1 = 1.0 + eps / 2;
        WriteLine($"a1 = {a1}, b1 = {b1}, c1 = {c1}\n");
        WriteLine($"a1==b1 ? {f.approx(a1, b1, acc, eps)}\n");
        WriteLine($"a1==c1 ? {f.approx(a1, c1, acc, eps)}\n");




        return 0;
    }
}