using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TasksAndState
{
    public class Test
    {
        private const int Elements = 200000;
        private static int globalSum = 0;
        private static void AddFunction()
        {
            globalSum += Data.Default.Value;
        }

        [Benchmark]
        public void CaptureState()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                TaskStub.StartNew(() => { globalSum += data.Value; });
            }
        }

        [Benchmark]
        public void PassStateAsParameter()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                TaskStub.StartNew(d => { globalSum += (d as Data).Value; }, data);
            }
        }

        [Benchmark]
        public void NoCapturedState()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                TaskStub.StartNew(() => { globalSum += Data.Default.Value; });
            }
        }

        [Benchmark]
        public void NoStateAndNoLambda()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                TaskStub.StartNew(AddFunction);
            }
        }


        [Benchmark]
        public void SCaptureState()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                STaskStub.StartNew(() => { globalSum += data.Value; });
            }
        }

        [Benchmark]
        public void SPassStateAsParameter()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                STaskStub.StartNew(d => { globalSum += (d as Data).Value; }, data);
            }
        }

        [Benchmark]
        public void SNoCapturedState()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                STaskStub.StartNew(() => { globalSum += Data.Default.Value; });
            }
        }

        [Benchmark]
        public void SNoStateAndNoLambda()
        {
            globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                STaskStub.StartNew(AddFunction);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Test>();
        }
    }
}
