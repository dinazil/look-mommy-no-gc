using System;
using System.Linq;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

namespace LINQ
{
    public class Test
    {
        private const int Minimum = 1000;
        private const int Maximum = 9999 + 1;

        // Find average number of unique digits in numbers between Minimum and Maximum

        [Benchmark]
        public static double CalculateWithLoops()
        {
            int sum = 0;

            for (int i = Minimum; i < Maximum; ++i)
            {
                var digits = new int[10];
                var number = i;
                while (number > 0)
                {
                    digits[number % 10] += 1;
                    number /= 10;
                }

                for (int d = 0; d < digits.Length; ++d)
                    if (digits[d] == 1) ++sum;
            }

            return (double)sum / (Maximum - Minimum);
        }

        [Benchmark]
        public static double CalculateWithLoopsAndString()
        {
            int sum = 0;
            for (int i = Minimum; i < Maximum; ++i)
            {
                var digits = new int[10];
                var s = i.ToString();
                for (var k = 0; k < s.Length; ++k)
                    digits[s[k] - '0'] += 1;
                for (int d = 0; d < digits.Length; ++d)
                    if (digits[d] == 1) // then this is a unique digit
                        ++sum;
            }
            return (double)sum / (Maximum - Minimum);
        }

        [Benchmark]
        public static double CalculateWithLinq()
        {
            return Enumerable.Range(Minimum, Maximum - Minimum)
                .Select(i => i.ToString()
                                .AsEnumerable()
                                .GroupBy(
                                c => c,
                                c => c,
                                (k, g) => new
                                {
                                    Character = k,
                                    Count = g.Count()
                                })
                                .Count(g => g.Count == 1))
                .Average();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Test.CalculateWithLoops());
            //Console.WriteLine(Test.CalculateWithLoopsAndString());
            //Console.WriteLine(Test.CalculateWithLinq());

            BenchmarkRunner.Run<Test>();
        }
    }
}
