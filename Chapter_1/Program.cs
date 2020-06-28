using System;
using System.Threading;

namespace Chapter_1
{
    class Program
    {
        public static void ThreadMethod(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                Console.WriteLine("ThreadProc:{0}", i);
                Thread.Sleep(0);
            }

        }

        [ThreadStatic]
        public static int _field;
        
        static void Main(string[] args)
        {
            new Thread(new ThreadStart(() =>
            {
                for(int i=0; i<10; i++)
                {
                    _field++;
                    Console.WriteLine("Thread A: {0}", _field);
                }
            })).Start();
            new Thread(new ThreadStart(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _field++;
                    Console.WriteLine("Thread B: {0}", _field);
                }
            })).Start();
            Console.ReadKey();
        }
    }
}
