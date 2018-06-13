using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace RoslynTest {
    class Program {
        static void Main(string[] args)
            => BenchmarkRunner.Run<Watcher>(DefaultConfig.Instance.With(Job.Clr));
    }
}
