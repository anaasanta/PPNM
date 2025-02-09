using static System.Math;

public class functions
{
    public int i, j;
    public double x;
    public float y;

    /* Constructor */
    public functions()
    {
        i = 0;
        j = 0;
        x = 1;
        y = 1F;
    }

    /*  Calculate the maximum representable integer */
    public int maxInt()
    {
        while (i + 1 > i)
        {
            i++;
        }
        return i;
    }

    /*  Calculate the minimum representable integer */
    public int minInt()
    {
        while (j - 1 < j)
        {
            j--;
        }
        return j;
    }

    /*  Calculate the machine epsilon for double */
    public double machineEpsilonDouble()
    {
        while (x + 1 != 1)
        {
            x /= 2;
        }
        x *= 2;
        return x;
    }


    /*  Calculate the machine epsilon for float */
    public float machineEpsilonFloat()
    {
        while ((float)(1F + y) != 1F)
        {
            y /= 2F;
        }
        y *= 2F;
        return y;
    }

    public bool approx(double a, double b, double acc = 1e-9, double eps = 1e-9)
    {
        if (Abs(b - a) <= acc)
        {
            return true;
        }
        if (Abs(b - a) <= Max(Abs(a), Abs(b)) * eps)
        {
            return true;
        }
        return false;
    }


}