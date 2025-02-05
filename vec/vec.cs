/*!SECTION
1. Devise a class, called "vec", an instance of which should hold three double values: x,y,z.
2. Implement the relevant constructors. In C#: parametrized and default constructors; in C++: parametrized, default, copy, and move constructors, a destructor, and also copy and move assignment operators (hint: most of them can be declared "default" and then the compiler will create them for you).
3. Implement the relevant mathematical operators for 3D vectors.
4. Imlement a simple "print" method for debugging.
5. C#: override the "ToString" method. C++: overload "operator<<".
6. Make an "approx" method to compare two vec's with absolute precision "acc" and relative precision "eps".
*/

using static System.Math;
using static System.Console;

public class vec
{
    public double x, y, z;

    /**
     * Constructors
     */
    public vec(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public vec()
    {
        x = y = z = 0;
    }

    /**
     * Mathematical operators
     */
    public static vec operator +(vec u, vec v)
    { //Sum of two vectors
        return new vec(u.x + v.x, u.y + v.y, u.z + v.z);
    }

    public static vec operator +(vec v, double i)
    { //Sum of a vector and a scalar
        return new vec(v.x + i, v.y + i, v.z + i);
    }

    public static vec operator -(vec u, vec v)
    { //Difference of two vectors
        return new vec(u.x - v.x, u.y - v.y, u.z - v.z);
    }

    public static vec operator -(vec v)
    { // Negation of a vector
        return new vec(-v.x, -v.y, -v.z);
    }

    public static vec operator -(vec v, double i)
    { // Difference of a vector and a scalar
        return new vec(v.x - i, v.y - i, v.z - i);
    }

    public static vec operator *(vec v, double i)
    { // Multiplication of a vector by a scalar
        return new vec(v.x * i, v.y * i, v.z * i);
    }

    public static vec operator *(double i, vec v)
    { // Multiplication of a vector by a scalar
        return v * i;
    }

    public double dot(vec other)
    { // Dot product of two vectors
        return this.x * other.x + this.y * other.y + this.z * other.z;
    }

    public static double dot(vec v, vec w)
    { // Dot product of two vectors
        return v.x * w.x + v.y * w.y + v.z * w.z;
    }

    public static vec operator /(vec v, vec u)
    { // Division of two vectors
        return new vec(v.x / u.x, v.y / u.y, v.z / u.z);
    }

    public static vec operator /(vec v, double i)
    { // Division of a vector by a scalar
        return new vec(v.x / i, v.y / i, v.z / i);
    }

    /**
     * Print methods
     */

    public void print(string s = "")
    {
        Write(s); WriteLine($" = [{x},{y},{z}]\n");
    }

    public override string ToString()
    { // Override ToString 
        return $"[{x},{y},{z}]";
    }

    /**
     * Approx method
     */

    public static bool approx(double a, double b, double acc = 1e-9, double eps = 1e-9)
    { // Compare two doubles with absolute precision "acc" and relative precision "eps"
        if (Abs(a - b) < acc) return true;
        if (Abs(a - b) / (Abs(a) + Abs(b)) < eps / 2) return true;
        return false;
    }
    public bool approx(vec other)
    { // Compare two vec's with absolute precision "acc" and relative precision "eps"
        if (!approx(this.x, other.x)) return false;
        if (!approx(this.y, other.y)) return false;
        if (!approx(this.z, other.z)) return false;
        return true;
    }

    public bool approx(vec u, vec v)
    { // Compare two vec's with absolute precision "acc" and relative precision "eps"
        return u.approx(v);
    }




}