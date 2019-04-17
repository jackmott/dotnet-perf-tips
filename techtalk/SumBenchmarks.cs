using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace techtalk
{

    public class ItemClass
    {
        public int Qty;
        public float Price;

        public ItemClass(int Qty, float Price)
        {
            this.Qty = Qty;
            this.Price = Price;
        }
    }
    public readonly struct ItemStruct
    {
        public readonly int Qty;
        public readonly float Price;

        public ItemStruct(int Qty, float Price)
        {
            this.Qty = Qty;
            this.Price = Price;
        }
    }
    public struct ItemsStruct
    {
        public float[] prices;
        public float[] qtys;
    }

    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 5)]
    [MemoryDiagnoser]
    public class SumBenchmarks
    {
        public LinkedList<ItemClass> linkedListClass;
        public List<ItemClass> listClass;
        public ItemClass[] arrayClass;
        public ItemStruct[] arrayStruct;
        public ItemsStruct items;
        const int TEST_SIZE = 131072 * 2;


        public SumBenchmarks()
        {
            linkedListClass = new LinkedList<ItemClass>();
            listClass = new List<ItemClass>();
            arrayClass = new ItemClass[TEST_SIZE];
            arrayStruct = new ItemStruct[TEST_SIZE];
            items.prices = new float[TEST_SIZE];
            items.qtys = new float[TEST_SIZE];
            for (int i = 0; i < TEST_SIZE; i++)
            {
                int qty = i % 3;
                float price = (float)(i % 100) / 2.0f;
                var itemClass = new ItemClass(qty, price);
                var itemStruct = new ItemStruct(qty, price);
                linkedListClass.AddFirst(itemClass);
                listClass.Add(itemClass);
                arrayClass[i] = itemClass;
                arrayStruct[i] = itemStruct;
                items.prices[i] = price;
                items.qtys[i] = qty;
            }
        }
        [Benchmark]
        // 3,200us 48B
        public float LinkedListLinq()
        {
            return linkedListClass.Sum(item => item.Qty * item.Price);
        }

        [Benchmark]
        // 2,600us 40B
        public float ListLinq()
        {
            return listClass.Sum(item => item.Qty * item.Price);
        }
      
       
        [Benchmark]
        // 2,000us 40B
        public float IEnumerableListForEach()
        {
            float sum = 0.0f;
            IEnumerable<ItemClass> iel = listClass;
            foreach (var item in iel)
            {
                sum += item.Qty * item.Price;
            }
            return sum;
        }
        [Benchmark]
        // 1,300us 0B
        public float ListForEach()
        {
            float sum = 0.0f;
            foreach (var item in listClass)
            {
                sum += item.Qty * item.Price;
            }
            return sum;
        }

        [Benchmark]
        // 1,000us 0B
        public float ListForLoop()
        {
            float sum = 0.0f;
            for (int i = 0; i < listClass.Count; i++)
            {
                var item = listClass[i];
                sum += item.Qty * item.Price;
            }
            return sum;
        }

        [Benchmark]
        // 1,800us 32B
        public float ArrayLinq()
        {
            return arrayClass.Sum(item => item.Qty * item.Price);
        }

        [Benchmark]
        // 800us 0B
        public float ArrayForeach()
        {
            float sum = 0.0f;
            foreach (var item in arrayClass)
            {
                sum += item.Qty * item.Price;
            }
            return sum;
        }

        [Benchmark]
        // 850us 0B
        public float ArrayForLoop()
        {
            float sum = 0.0f;
            for (int i = 0; i < arrayClass.Length; i++)
            {
                var item = arrayClass[i];
                sum += item.Qty * item.Price;
            }
            return sum;
        }

        [Benchmark]
        // 800us 0B
        public float ArrayForLoopLocal()
        {
            var a = arrayClass;
            float sum = 0.0f;
            for (int i = 0; i < a.Length; i++)
            {
                var item = a[i];
                sum += item.Qty * item.Price;
            }
            return sum;
        }

        [Benchmark]
        // 246us 0B
        public float ArrayForLoopLocalStruct()
        {
            var a = arrayStruct;
            float sum = 0.0f;
            for (int i = 0; i < a.Length; i++)
            {
                var item = a[i];
                sum += item.Qty * item.Price;
            }
            return sum;
        }

        // 38us 0B
        [Benchmark]
        public float ArraySIMD()
        {
            var p = items.prices;
            var q = items.qtys;
            var sum = new Vector<float>(0.0f);
            for (int i = 0; i < p.Length; i += Vector<float>.Count)
            {
                var qty = new Vector<float>(q[i]);
                var price = new Vector<float>(p[i]);
                sum += qty * price;
            }
            var scalarSum = 0.0f;
            for (int i = 0; i < Vector<float>.Count; i++)
            {
                scalarSum += sum[i];
            }
            return scalarSum;

        }

    }
}
