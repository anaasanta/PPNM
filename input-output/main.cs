using static System.Math;
using static System.Console;

class main
{

    public static void Main(string[] args)
    {
        foreach (var arg in args) // Loop over all arguments
        {
            var words = arg.Split(':'); // Split argument by ':'
            if (words[0] == "-numbers") // Check if first word is '-numbers'
            {
                var numbers = words[1].Split(','); // Split second word by ','
                foreach (var number in numbers) // Loop over all numbers
                {
                    double x = double.Parse(number);
                    WriteLine($"{x} {Sin(x)} {Cos(x)}");
                }
            }
        }
        /*
            char[] split_delimiters = { ' ', '\t', '\n' }; 
            var split_options = StringSplitOptions.RemoveEmptyEntries; // Remove empty entries from split result 
            for (string line = ReadLine(); line != null; line = ReadLine()) // Read lines from standard input
            {
                var numbers = line.Split(split_delimiters, split_options); 
                foreach (var number in numbers)
                {
                    double x = double.Parse(number);
                    Error.WriteLine($"{x} {Sin(x)} {Cos(x)}");
                }
            }
            */
    }
}