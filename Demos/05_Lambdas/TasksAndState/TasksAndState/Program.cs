using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TasksAndState
{
    public class Test
    {
        private const int Elements = 200000;
        private static int _globalSum;
        private static void AddFunction()
        {
            _globalSum += Data.Default.Value;
        }

        [Benchmark]
        public void CaptureState()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                TaskStub.StartNew(() => { _globalSum += data.Value; });
            }
        }

        [Benchmark]
        public void PassStateAsParameter()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                TaskStub.StartNew(d => { _globalSum += (d as Data).Value; }, data);
            }
        }

        [Benchmark]
        public void NoCapturedState()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                TaskStub.StartNew(() => { _globalSum += Data.Default.Value; });
            }
        }

        [Benchmark]
        public void NoStateAndNoLambda()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                TaskStub.StartNew(AddFunction);
            }
        }

        #region struct

        [Benchmark]
        public void SCaptureState()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                STaskStub.StartNew(() => { _globalSum += data.Value; });
            }
        }

        [Benchmark]
        public void SPassStateAsParameter()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                var data = new Data { Value = i };
                STaskStub.StartNew(d => { _globalSum += (d as Data).Value; }, data);
            }
        }

        [Benchmark]
        public void SNoCapturedState()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                STaskStub.StartNew(() => { _globalSum += Data.Default.Value; });
            }
        }

        [Benchmark]
        public void SNoStateAndNoLambda()
        {
            _globalSum = 0;
            for (int i = 0; i < Elements; ++i)
            {
                STaskStub.StartNew(AddFunction);
            }
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
