using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Windows.Forms;

namespace DotSpatial.Python
{
    public class Script
    {
        private static ScriptEngine _engine = IronPython.Hosting.Python.CreateEngine();
        private static ScriptScope _scope = CreateScope();
        private static string _pythonFunctionsPath = "";

        public static void SetPythonFunctionsPath(string sPath)
        {
            _pythonFunctionsPath = sPath;
            _scope = CreateScope();
        }

        private static ScriptScope CreateScope()
        {
            ScriptScope ScriptScope = null;
            try
            {

                ScriptRuntime sr = IronPython.Hosting.Python.CreateRuntime();

                if (System.IO.File.Exists(_pythonFunctionsPath))
                    ScriptScope = sr.UseFile(_pythonFunctionsPath);
                else if (_engine != null)
                    ScriptScope = _engine.CreateScope();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
            return ScriptScope;
        }

        public static string Evaluate(string code)
        {

            try
            {
                ScriptSource source = _engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);

                var actual = source.Execute(_scope);
                Func<object> func = _scope.GetVariable<Func<object>>("Main");
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
                ScriptSource source = _engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);

                var actual = source.Execute(_scope);
                Func<object> func = _scope.GetVariable<Func<object>>("Main");
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
