using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotSpatial.Python
{
    public class Script
    {
        public static string Evaluate(string code)
        {

            try
            {

                ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
                scope = engine.CreateScope();

                var actual = source.Execute(scope);
                Func<object> func = scope.GetVariable<Func<object>>("Main");
                return func().ToString();


            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
        }

        public static string EvaluateWithDialog(string code)
        {

            try
            {

                ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
                scope = engine.CreateScope();

                var actual = source.Execute(scope);
                Func<object> func = scope.GetVariable<Func<object>>("Main");
                return func().ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return "";
            }
        }
    }
}
