using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Boxing
{
    struct Number
    {
        public int Value { get; set; }
    }

    struct ENumber : IEquatable<ENumber>
    {
        public int Value { get; set; }
        public bool Equals(ENumber other)
        {
            return Value == other.Value;
        }
    }

    class CNumber
    {
        public int Value { get; set; }
    }

    class CENumber : IEquatable<CENumber>
    {
        public int Value { get; set; }
        public bool Equals(CENumber other)
        {
            return Value == other.Value;
        }
    }

    public class Test
    {
        private const int N = 1000000;
        private readonly List<Number> numbers;
        private readonly List<ENumber> enumbers;
        private readonly List<CNumber> cnumbers;
        private readonly List<CENumber> cenumbers;

        public Test()
        {
            numbers = Enumerable.Range(0, N).Select(v => new Number { Value = v }).ToList();
            enumbers = Enumerable.Range(0, N).Select(v => new ENumber { Value = v }).ToList();
            cnumbers = Enumerable.Range(0, N).Select(v => new CNumber { Value = v }).ToList();
            cenumbers = Enumerable.Range(0, N).Select(v => new CENumber { Value = v }).ToList();
        }

        [Benchmark]
        public bool SearchNumbers()
        {
            return numbers.Contains(numbers.Last());
        }

        [Benchmark]
        public bool SearchENumbers()
        {
            return enumbers.Contains(enumbers.Last());
        }

        //[Benchmark]
        public bool SearchCNumbers()
        {
            return cnumbers.Contains(cnumbers.Last());
        }

        //[Benchmark]
        public bool SearchCENumbers()
        {
            return cenumbers.Contains(cenumbers.Last());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Test>();
        }
    }
}
