using static System.Console;
using System;
using System.IO;
using System.Threading;

class main
{
    public class data { public int a, b; public double sum; } // data structure (a= start, b= stop)
    public static void harm(object obj) // partial sum of harmonic series
    {
        var arg = (data)obj; // cast object to data
        arg.sum = 0; // initialize sum
        for (int i = arg.a; i < arg.b; i++) arg.sum += 1.0 / i; // sum
    }

    public static int Main(string[] args)
    {
        /*
            1. Read from the command-line two parameters:
            - Number of threads to be created
            - Number of terms in the harmonic sum to calculate
        */
        int argc = args.Length;
        int nthreads = 1, nterms = (int)1e8; /* default values */
        for (int i = 0; i < argc; i++) // read command line arguments
        {
            string arg = args[i];
            if (arg == "-threads" && i + 1 < argc) nthreads = int.Parse(args[i + 1]);
            if (arg == "-terms" && i + 1 < argc) nterms = (int)double.Parse(args[i + 1]);
        }
        Error.WriteLine($"# threads = {nthreads}, # terms = {nterms}");

        /*
            2. Prepare data-objects to be used locally in separate threads
        */
        var threads = new System.Threading.Thread[nthreads]; // array of threads
        var param = new data[nthreads]; // data structure for threads
        for (int i = 0; i < nthreads; i++) // initialize data structure
        {
            param[i].a = i * nterms / nthreads + 1; // start
            param[i].b = (i + 1) * nterms / nthreads + 1; // stop
        }
        for (int i = 0; i < nthreads; i++)
        {
            threads[i] = new System.Threading.Thread(harm); // create thread
            threads[i].Start(param[i]); // run it with params[i] as argument to "harm"
        }

        foreach (var t in threads) t.Join(); // wait for all threads to finish
        double sum = 0;
        foreach (var p in param) sum += p.sum; // sum partial sums
        WriteLine($"Sum = {sum}");


        return 0;

    }
}