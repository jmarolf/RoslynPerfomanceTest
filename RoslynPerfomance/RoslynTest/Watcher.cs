using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace RoslynTest
{
    public class Watcher
    {
        ScriptRunner<object> ScriptAction;
        Action CompileAction;
        Func<object> EmitAction;

        public Watcher()
        {
            ScriptAction = SetupScript();
            ScriptAction();

            CompileAction = SetupCompileAction();
            CompileAction();

            EmitAction = SetupEmitAction();
            EmitAction();
        }

        private ScriptRunner<object> SetupScript()
        {
            var script = CSharpScript.Create(@"using System;
using RoslynTest;
Watcher.Runner(""123"");
Watcher.Runner(""123"");",
            ScriptOptions.Default.AddReferences(Assembly.GetEntryAssembly()));
            script.Compile();
            return script.CreateDelegate();
        }

        private Action SetupCompileAction()
        {
            string text = @"using RoslynTest;
public class Evaluator
{
    public static void Evaluate()
    {
        Watcher.Runner(""123"");
        Watcher.Runner(""123"");
    } 
}";
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(text);
            CSharpCompilation compilation = CSharpCompilation.Create(
                "eval.dll",
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                                    MetadataReference.CreateFromFile(typeof(Watcher).Assembly.Location)});

            Assembly compiledAssembly;
            using (MemoryStream stream = new MemoryStream())
            {
                Microsoft.CodeAnalysis.Emit.EmitResult compileResult = compilation.Emit(stream);
                compiledAssembly = Assembly.Load(stream.GetBuffer());
            }

            Type evaluator = compiledAssembly.GetType("Evaluator");
            return (Action) evaluator.GetMethod("Evaluate").CreateDelegate(typeof(Action));
        }

        private Func<object> SetupEmitAction()
        {
            DynamicMethod method = new DynamicMethod("Advise", typeof(object), new Type[0]);
            ILGenerator il = method.GetILGenerator();
            var methods = typeof(Watcher).GetMethod("Runner", new Type[] { typeof(string) });
            il.Emit(OpCodes.Ldstr, "123");
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Call, methods);
            il.Emit(OpCodes.Call, methods);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
            return (Func<object>)method.CreateDelegate(typeof(Func<object>));
        }

        [Benchmark]
        public void Scripting()
        {
            ScriptAction();
        }
        [Benchmark]
        public void Compile()
        {
            CompileAction();
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
        }
    }
}
