using BenchmarkDotNet.Running;

namespace techtalk
{

    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<SumBenchmarks>();
            var summary = BenchmarkRunner.Run<EnumBenchmarks>();

        }
    }
}
