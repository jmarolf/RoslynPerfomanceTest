using BenchmarkDotNet.Running;
using System;

namespace RoslynTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Watcher a = new Watcher();
            var summary = BenchmarkRunner.Run<Watcher>();
            Console.ReadKey();
        }
    }
}
