using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace RoslynTest
{
    public class Watcher
    {
        ScriptRunner<object> RoslynAction;
        Func<object> EmitAction;

        public Watcher()
        {
            
            var script = CSharpScript.Create(@"using System;
using RoslynTest;
Watcher.Runner(""123"");
Watcher.Runner(""123"");", 
ScriptOptions.Default.AddReferences(Assembly.GetEntryAssembly()));
            script.Compile();
            RoslynAction = script.CreateDelegate();
            RoslynAction();

            DynamicMethod method = new DynamicMethod("Advise", typeof(object), new Type[0]);
            ILGenerator il = method.GetILGenerator();
            var methods = typeof(Watcher).GetMethod("Runner", new Type[] { typeof(string) });
            il.Emit(OpCodes.Ldstr, "123");
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Call, methods);
            il.Emit(OpCodes.Call, methods);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
            EmitAction = (Func<object>)method.CreateDelegate(typeof(Func<object>));
            EmitAction();
        }
        [Benchmark]
        public void Roslyn()
        {
            RoslynAction();
        }
        [Benchmark]
        public void Emit()
        {
            EmitAction();
        }

        public static void Runner(string value)
        {
            string result = "1";
            result += value;
            //Console.WriteLine(result);
        }
    }
}
