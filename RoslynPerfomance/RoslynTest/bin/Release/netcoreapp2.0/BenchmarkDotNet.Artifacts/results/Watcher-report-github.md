``` ini

BenchmarkDotNet=v0.10.13, OS=Windows 10.0.17134
Intel Pentium CPU G3240 3.10GHz, 1 CPU, 2 logical cores and 2 physical cores
Frequency=3020362 Hz, Resolution=331.0861 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT


```
| Method |      Mean |     Error |    StdDev |
|------- |----------:|----------:|----------:|
| Roslyn | 247.15 ns | 3.3848 ns | 3.0005 ns |
|   Emit |  50.18 ns | 0.6188 ns | 0.5486 ns |
