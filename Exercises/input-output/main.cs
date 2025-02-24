using static System.Console;
using static System.Math;
using System;
using System.IO;

class main
{
    public static void Main(string[] args)
    {
        string infile = null, outfile = null;

        foreach (var arg in args)
        {
            var words = arg.Split(':');
            if (words[0] == "-input") infile = words[1];
            if (words[0] == "-output") outfile = words[1];
        }

        if (infile != null && outfile != null)
        {
            var instream = new StreamReader(infile);
            var outstream = new StreamWriter(outfile, append: false);
            for (string line = instream.ReadLine(); line != null; line = instream.ReadLine())
            {   // TODO: I added split delimiters to divide each number, because the I was having problems with output.txt 
                var numbers = line.Split(' ', '\t', '\n');
                foreach (var number in numbers)
                {
                    double x = double.Parse(number);
                    outstream.WriteLine($"{x} {Sin(x)} {Cos(x)}");
                }
            }
            instream.Close();
            outstream.Close();
        }
        else
        {
            foreach (var arg in args)
            {
                var words = arg.Split(':');
                if (words[0] == "-numbers")
                {
                    var numbers = words[1].Split(',');
                    foreach (var number in numbers)
                    {
                        double x = double.Parse(number);
                        WriteLine($"{x} {Sin(x)} {Cos(x)}");
                    }
                }
            }

            char[] split_delimiters = { ' ', '\t', '\n' };
            var split_options = StringSplitOptions.RemoveEmptyEntries;
            for (string line = ReadLine(); line != null; line = ReadLine())
            {
                var numbers = line.Split(split_delimiters, split_options);
                foreach (var number in numbers)
                {
                    double x = double.Parse(number);
                    Error.WriteLine($"{x} {Sin(x)} {Cos(x)}");
                }
            }
        }
    }
}
