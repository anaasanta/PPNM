using static System.Math;
using static System.Console;

class main
{

    static int Main()
    {
        // Get two vectors made out of random numbers
        var rand = new System.Random(); // Crea una instancia de Random
        var u = new vec(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
        var v = new vec(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());


        // Print the vectors
        System.Console.WriteLine("\n======= Vectors:===========\n Should show:\n");
        System.Console.WriteLine($"u = {u}\n");
        System.Console.WriteLine($"v = {v}\n");
        // Verify print vec.cs
        System.Console.WriteLine("Shows:\n");
        u.print("u");
        v.print("v");

        vec t;

        // Calculate the sum of the vectors
        System.Console.WriteLine("\n======= Sum:===========\n Should show:\n");
        t = new vec(u.x + v.x, u.y + v.y, u.z + v.z);
        System.Console.WriteLine($"u + v = {t}\n");
        // Verify sum vec.cs
        System.Console.WriteLine("Shows:\n");
        (u + v).print("u + v");
        if (t.approx(u + v))
            System.Console.WriteLine("The sum is correct\n");
        else
            System.Console.WriteLine("The sum is incorrect\n");


        // Calculate the sum of a vector and a scalar
        System.Console.WriteLine("\n======= Sum with scalar:===========\n Should show:\n");
        var i = new System.Random().NextDouble();
        t = new vec(u.x + i, u.y + i, u.z + i);
        t.print("u + i");
        // Verify sum with scalar vec.cs
        System.Console.WriteLine("Shows:\n");
        (u + i).print("u + i");
        if (t.approx(u + i))
            System.Console.WriteLine("The sum with scalar is correct\n");
        else
            System.Console.WriteLine("The sum with scalar is incorrect\n");

        // Calculate the difference of the vectors
        System.Console.WriteLine("\n======= Difference:===========\n Should show:\n");
        t = new vec(u.x - v.x, u.y - v.y, u.z - v.z);
        t.print("u - v");
        // Verify difference vec.cs
        System.Console.WriteLine("Shows:\n");
        (u - v).print("u - v");
        if (t.approx(u - v))
            System.Console.WriteLine("The difference is correct\n");
        else
            System.Console.WriteLine("The difference is incorrect\n");

        // Calculate the negation of a vector
        System.Console.WriteLine("\n======= Negation:===========\n Should show:\n");
        t = new vec(-u.x, -u.y, -u.z);
        t.print("-u");
        // Verify negation vec.cs
        System.Console.WriteLine("Shows:\n");
        (-u).print("-u");
        if (t.approx(-u))
            System.Console.WriteLine("The negation is correct\n");
        else
            System.Console.WriteLine("The negation is incorrect\n");

        // Calculate the difference of a vector and a scalar
        System.Console.WriteLine("\n======= Difference with scalar:===========\n Should show:\n");
        i = new System.Random().NextDouble();
        t = new vec(u.x - i, u.y - i, u.z - i);
        t.print("u - i");
        // Verify difference with scalar vec.cs
        System.Console.WriteLine("Shows:\n");
        (u - i).print("u - i");
        if (t.approx(u - i))
            System.Console.WriteLine("The difference with scalar is correct\n");
        else
            System.Console.WriteLine("The difference with scalar is incorrect\n");


        //Calulate the dot product of two vectors
        System.Console.WriteLine("\n======= Dot product:===========\n Should show:\n");
        var a = u.x * v.x + u.y * v.y + u.z * v.z;
        System.Console.WriteLine($"u dot v = {a}\n");
        // Verify dot product vec.cs
        System.Console.WriteLine("Shows:\n");
        System.Console.WriteLine($"u dot v = {vec.dot(u, v)}\n");
        if (vec.approx(a, vec.dot(u, v)))
            System.Console.WriteLine("The dot product is correct\n");
        else
            System.Console.WriteLine("The dot product is incorrect\n");

        // Calculate the division of two vectors
        System.Console.WriteLine("\n======= Division:===========\n Should show:\n");
        t = new vec(u.x / v.x, u.y / v.y, u.z / v.z);
        t.print("u / v");
        // Verify division vec.cs
        System.Console.WriteLine("Shows:\n");
        (u / v).print("u / v");
        if (t.approx(u / v))
            System.Console.WriteLine("The division is correct\n");
        else
            System.Console.WriteLine("The division is incorrect\n");

        // Calculate the division of a vector by a scalar
        System.Console.WriteLine("\n======= Division with scalar:===========\n Should show:\n");
        i = new System.Random().NextDouble();
        t = new vec(u.x / i, u.y / i, u.z / i);
        t.print("u / i");
        // Verify division with scalar vec.cs
        System.Console.WriteLine("Shows:\n");
        (u / i).print("u / i");
        if (t.approx(u / i))
            System.Console.WriteLine("The division with scalar is correct\n");
        else
            System.Console.WriteLine("The division with scalar is incorrect\n");












        return 0;
    }
}