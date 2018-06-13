using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MongodbTest
{
    public class CSharpScriptEngine
    {
        public static ScriptRunner<T> CreateMethod<T>(string code)
        {

           /* using (var loader = new InteractiveAssemblyLoader())
            {
                var script = CSharpScript.Create<T>(code, ScriptOptions.Default.AddReferences(Assembly.GetEntryAssembly()), assemblyLoader: loader);
                script.Compile();
                return script.CreateDelegate();
                //do stuff 
            }*/
             var script = CSharpScript.Create<T>(code, ScriptOptions.Default.AddReferences(Assembly.GetEntryAssembly()),typeof(Action));
            script.Compile();
            return script.CreateDelegate();
        }
    }
}
