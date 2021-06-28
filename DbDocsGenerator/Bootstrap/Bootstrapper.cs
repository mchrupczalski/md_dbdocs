using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator.Bootstrap
{
    internal static class Bootstrapper
    {
        internal static string CheckApps()
        {
            const string cmdNode = "node -v";
            const string cmdNpm = "npm -v";
            const string cmdMdi = "npm list -g markdown-include";

            string outMsg = "Checking prerequisites...\n" +
                            "\t Apps installed: \n" +
                            "\t |        App       | Inst. | Version info \n" +
                            "\t |------------------|-------|--------------\n";
            //\t | markdown-include |

            // check if Node.js, npm and markdown-include are installed
            string node = GetAppInfo("Node.js", cmdNode);
            string npm = GetAppInfo("npm", cmdNpm);
            string mdi = GetAppInfo("markdown-include", cmdMdi);

            outMsg = $"{ outMsg }" +
                     $"\t { node }\n" +
                     $"\t { npm } \n" +
                     $"\t { mdi } \n" +
                     $"\t ------------------------------------------\n";

            return outMsg;
        }

        private static string GetAppInfo(string appName, string cmdCheckParameter)
        {
            const int appLen = 16;
            const int instLen = 5;

            while (appName.Length < appLen) { appName += " "; }

            string appV = GetCmdOutput(cmdCheckParameter);
            bool gotApp = !string.IsNullOrEmpty(appV);
            if (gotApp && appV.Length > 10)
            {
                gotApp = (string.Compare("(empty)", appV) > 0);
            }
            string gotAppS = gotApp.ToString().ToUpper();

            while (gotAppS.Length < instLen) { gotAppS += " "; }

            string output = $"| { appName } | { gotAppS } | { appV }";
            return output;
        }

        private static string GetCmdOutput(string cmdCheckParameter)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c { cmdCheckParameter } /c",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            process.Start();
            // Now read the value, parse to int and add 1 (from the original script)
            string output = process.StandardOutput.ReadToEnd().Replace("`--", "").Replace(System.Environment.NewLine, "").Replace("\n", "");

            process.WaitForExit();

            return output;
        }
    }
}
