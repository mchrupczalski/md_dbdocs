namespace md_dbdocs.lib.Utilities
{
    public static class CmdUtils
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

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }
    }
}