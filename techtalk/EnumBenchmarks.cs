using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace techtalk
{

    enum TestEnum { ALPHA, BETA, GAMMA, DELTA, UHOH};

    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 5)]
    [MemoryDiagnoser]
    public class EnumBenchmarks
    {
        TestEnum[] testEnums;
        const int TEST_SIZE = 1000;
        public EnumBenchmarks()
        {
            testEnums = new TestEnum[TEST_SIZE];
            for (int i = 0; i < TEST_SIZE; i++)
            {
                testEnums[i] = (TestEnum)(i % 4);
            }
        }

        [Benchmark]
        // 67us 39KB
        public List<string> enumToStringReflection()
        {
            var result = new List<string>();
            foreach (var e in testEnums)
            {
                //ToString uses reflection under the hood
                result.Add(e.ToString());
            }
            return result;
        }

        [Benchmark]
        // 5us 16.2KB
        public List<string> enumToStringSwitch()
        {
            var result = new List<string>();
            foreach (var e in testEnums)
            {
                switch (e)
                {
                    case TestEnum.ALPHA:
                        result.Add("ALPHA");
                        break;
                    case TestEnum.BETA:
                        result.Add("BETA");
                        break;
                    case TestEnum.GAMMA:
                        result.Add("GAMMA");
                        break;
                    case TestEnum.DELTA:
                        result.Add("DELTA");
                        break;
                }                
            }
            return result;
        }

        [Benchmark]
        //4us 7.8KB
        public List<string> enumToStringSwitchPreAllocatedList()
        {
            var result = new List<string>(testEnums.Length);
            foreach (var e in testEnums)
            {
                switch (e)
                {
                    case TestEnum.ALPHA:
                        result.Add("ALPHA");
                        break;
                    case TestEnum.BETA:
                        result.Add("BETA");
                        break;
                    case TestEnum.GAMMA:
                        result.Add("GAMMA");
                        break;
                    case TestEnum.DELTA:
                        result.Add("DELTA");
                        break;
                }                
            }
            return result;
        }
    }


}

