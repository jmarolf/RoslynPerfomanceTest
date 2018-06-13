using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;

namespace RoslynTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Watcher>(DefaultConfig.Instance.With(Job.Clr));
            Console.ReadKey();
        }
    }
}
