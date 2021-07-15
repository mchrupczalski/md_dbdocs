using System;

namespace md_dbdocs.console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            md_dbdocs.core.Config.AppConfigProvider.LoadFromFile();
        }
    }
}
