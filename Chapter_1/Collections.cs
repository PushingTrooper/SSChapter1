using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter_1
{
    class Collections
    {
        static void Main(string[] args)
        {
            StacksThatAreConcurrent();
            Console.WriteLine("trying to push");
        }

        private static void ABunchOfNumbersInParallel()
        {
            var numbers = Enumerable.Range(0, 10);
            var parallelResults = numbers.AsParallel()
                .Where(i => i % 2 == 0)
                .ToArray();

            foreach (int i in parallelResults)
            {
                Console.WriteLine(i);
            }
        }

        private static void ABunchOfNumbersInParallelButSequential()
        {
            var numbers = Enumerable.Range(0, 10);
            var parallelResults = numbers.AsParallel().AsOrdered()
                .Where(i => i % 2 == 0).AsSequential()
                .ToArray();

            foreach (int i in parallelResults)
            {
                Console.WriteLine(i);
            }
        }

        private static void ParallelIteration()
        {
            var numbers = Enumerable.Range(0, 20);
            var parallelResults = numbers.AsParallel()
                .Where(i => i % 2 == 0);

            parallelResults.ForAll(e => Console.WriteLine(e));
        }

        private static void ParallelIterationWithException()
        {
            var numbers = Enumerable.Range(0, 20);
            try
            {
                var parallelResults = numbers.AsParallel()
                .Where(i => i % 2 == 0);

                parallelResults.ForAll(e => Console.WriteLine(e));
            }
            catch (AggregateException e)
            {
                Console.WriteLine("There where {0} exceptions", e.InnerExceptions.Count);
            }

        }

        private static void BlockCollectionsAndTheirUses()
        {
            BlockingCollection<String> collection = new BlockingCollection<string>();

            Task read = Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(collection.Take());
                }
            });

            Task write = Task.Run(() =>
            {
                while (true)
                {
                    string s = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(s)) break;

                    collection.Add(s);
                }
            });

            write.Wait();
        }

        private static void BlockCollectionsTheBetterWay()
        {
            BlockingCollection<String> collection = new BlockingCollection<string>();

            Task read = Task.Run(() =>
            {
                foreach (string st in collection.GetConsumingEnumerable())
                    Console.WriteLine(st);
            });

            Task write = Task.Run(() =>
            {
                while (true)
                {
                    string s = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(s)) break;

                    collection.Add(s);
                }
            });

            write.Wait();
        }

        private static void ConcurrentingBags()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Task.Run(() => {
                bag.Add(42);
                Thread.Sleep(1000);
                bag.Add(21);
            });

            Task.Run(() =>
            {
                foreach (int i in bag)
                    Console.WriteLine(i);
            }).Wait();
        }

        private static void StacksThatAreConcurrent()
        {
            ConcurrentStack<int> stack = new ConcurrentStack<int>();
            stack.Push(42);
            int result;
            if (stack.TryPop(out result))
                Console.WriteLine("Popped: {0}", result);

            stack.PushRange(new int[] { 1, 2, 3 });
            int[] values = new int[2];
            stack.TryPopRange(values);

            foreach (int i in stack)
                Console.WriteLine(i);
        }
    }
}
