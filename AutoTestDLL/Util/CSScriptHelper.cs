using CSScriptLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Util
{
    public static class CSScriptHelper
    {
        public static object RunScript(string scriptCode, string className, string methodName, object[] paramList)
        {
            try
            {
                object result = null;
                AsmHelper helper = new AsmHelper(CSScript.LoadCode(scriptCode, null, false));
                result = helper.Invoke(className + "." + methodName, paramList);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object RunScriptFile(string filename, string className, string methodName, object[] paramList)
        {
            try
            {
                object result = null;
                AsmHelper helper = new AsmHelper(CSScript.LoadFile(filename));
                result = helper.Invoke(className + "." + methodName, paramList);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
