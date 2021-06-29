namespace md_dbdocs.app.Helpers
{
    public static class CmdHelper
    {
        public static string GetCmdOutput(string cmdCheckParameter)
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
