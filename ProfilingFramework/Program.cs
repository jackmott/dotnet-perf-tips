using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Profiling
{
    class Program
    {
        //[MethodImpl(MethodImplOptions.NoInlining)]
        public static double doMathSlow(IEnumerable<double> numbers)
        {
            return numbers.Average();
        }

        public static double doMathFast(double[] numbers)
        {
            double sum = 0.0;
            foreach (var num in numbers)
            {
                sum += num;
            }
            return sum / numbers.Length;
        }

        static void Main(string[] args)
        {
            var rand = new Random();

            double slowTotal = 0.0;
            double fastTotal = 0.0;
            for (int i = 0; i < 10000000; i++)
            {
                var numbers = new double[10000];
                for (int j = 0; j < numbers.Length; j++)
                {
                    numbers[j] = rand.NextDouble();
                }
                slowTotal += doMathSlow(numbers);
                fastTotal += doMathFast(numbers);
            }
            Console.WriteLine("Slow:" + slowTotal + " Fast:" + fastTotal);
            Console.ReadLine();
        }
    }
 }
