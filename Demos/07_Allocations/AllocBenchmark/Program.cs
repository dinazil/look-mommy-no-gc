using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AllocBenchmark
{
    public class ResponseBuilder
    {
        private const string ResponseHeader = "<div><ul>";
        private const string ResponseFooter = "</ul></div>";
        private const string EmptyResponse = ResponseHeader + ResponseFooter;

        private List<int> data = new List<int>(); // deliberately empty
        private byte[] emptyBytes = Encoding.UTF8.GetBytes(EmptyResponse);

        [Benchmark]
        public byte[] BuildResponse()
        {
            var sb = new StringBuilder(ResponseHeader);
            foreach (var item in data)
            {
                sb.Append("<li>" + item  + "</li>");
            }
            sb.Append(ResponseFooter);
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        [Benchmark]
        public byte[] BuildResponseOptimized()
        {
            if (data.Count == 0)
                return Encoding.UTF8.GetBytes(EmptyResponse);

            return BuildResponse();
        }

        [Benchmark]
        public byte[] BuildResponseOptimizedWithCachedBytes()
        {
            if (data.Count == 0)
                return emptyBytes;

            return BuildResponse();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ResponseBuilder>();
        }
    }
}
