using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Boxing
{
    struct Struct
    {
        public int Value { get; set; }
    }

    struct StructWithSpecializedEquals
    {
        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is StructWithSpecializedEquals))
            {
                return false;
            }

            return ((StructWithSpecializedEquals)obj).Value == Value;
        }
    }

    struct StructEquatable : IEquatable<StructEquatable>
    {
        public int Value { get; set; }

        public bool Equals(StructEquatable other)
        {
            return Value == other.Value;
        }
    }

    #region class

    class Class
    {
        public int Value { get; set; }
    }

    class ClassWithReferenceSpecializedEquals
    {
        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }
    }

    class ClassWithSpecializedEquals
    {
        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            ClassWithSpecializedEquals other = obj as ClassWithSpecializedEquals;
            if (other == null)
                return false;

            return other.Value == Value;
        }
    }

    class ClassEquatable : IEquatable<ClassEquatable>
    {
        public int Value { get; set; }

        public bool Equals(ClassEquatable other)
        {
            return Value == other.Value;
        }
    }

    #endregion 

    public class Test
    {
        private const int N = 10000;

        private readonly List<Struct> structs;
        private readonly List<StructWithSpecializedEquals> structWithSpecializedEqualses;
        private readonly List<StructEquatable> structEquatables;

        #region class
        private readonly List<Class> classes;
        private readonly List<ClassWithReferenceSpecializedEquals> classWithReferenceSpecializedEqualses;
        private readonly List<ClassWithSpecializedEquals> classWithSpecializedEqualses;
        private readonly List<ClassEquatable> classEquatables;
        #endregion

        public Test()
        {
            structs = Enumerable.Range(0, N).Select(v => new Struct { Value = v }).ToList();
            structWithSpecializedEqualses = Enumerable.Range(0, N).Select(v => new StructWithSpecializedEquals { Value = v }).ToList();
            structEquatables = Enumerable.Range(0, N).Select(v => new StructEquatable { Value = v }).ToList();

            #region class
            classes = Enumerable.Range(0, N).Select(v => new Class { Value = v }).ToList();
            classWithReferenceSpecializedEqualses = Enumerable.Range(0, N).Select(v => new ClassWithReferenceSpecializedEquals { Value = v }).ToList();
            classWithSpecializedEqualses = Enumerable.Range(0, N).Select(v => new ClassWithSpecializedEquals { Value = v }).ToList();
            classEquatables = Enumerable.Range(0, N).Select(v => new ClassEquatable { Value = v }).ToList();
            #endregion
        }

        [Benchmark]
        public bool SearchStruct()
        {
            return structs.Contains(structs.Last());
        }

        [Benchmark]
        public bool SearchStructWithSpecializedEquals()
        {
            return structWithSpecializedEqualses.Contains(structWithSpecializedEqualses.Last());
        }

        [Benchmark]
        public bool SearchStructEquatable()
        {
            return structEquatables.Contains(structEquatables.Last());
        }

        #region class

        [Benchmark]
        public bool SearchClass()
        {
            return classes.Contains(classes.Last());
        }

        [Benchmark]
        public bool SearchClassWithReferenceSpecializedEquals()
        {
            return classWithReferenceSpecializedEqualses.Contains(classWithReferenceSpecializedEqualses.Last());
        }

        [Benchmark]
        public bool SearchClassWithSpecializedEquals()
        {
            return classWithSpecializedEqualses.Contains(classWithSpecializedEqualses.Last());
        }

        [Benchmark]
        public bool SearchClassEquatable()
        {
            return classEquatables.Contains(classEquatables.Last());
        }

        #endregion
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Test>();
        }
    }
}
