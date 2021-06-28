using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator.Helpers
{
    internal static class WelcomeScreen
    {
        internal static void ShowMessage()
        {
            Console.WriteLine($"\t\t▓█████▄  ▄▄▄▄      ▓█████▄  ▒█████   ▄████▄    ██████ ");
            Console.WriteLine($"\t\t▒██▀ ██▌▓█████▄    ▒██▀ ██▌▒██▒  ██▒▒██▀ ▀█  ▒██    ▒ ");
            Console.WriteLine($"\t\t░██   █▌▒██▒ ▄██   ░██   █▌▒██░  ██▒▒▓█    ▄ ░ ▓██▄   ");
            Console.WriteLine($"\t\t░▓█▄   ▌▒██░█▀     ░▓█▄   ▌▒██   ██░▒▓▓▄ ▄██▒  ▒   ██▒");
            Console.WriteLine($"\t\t░▒████▓ ░▓█  ▀█▓   ░▒████▓ ░ ████▓▒░▒ ▓███▀ ░▒██████▒▒");
            Console.WriteLine($"\t\t ▒▒▓  ▒ ░▒▓███▀▒    ▒▒▓  ▒ ░ ▒░▒░▒░ ░ ░▒ ▒  ░▒ ▒▓▒ ▒ ░");
            Console.WriteLine($"\t\t ░ ▒  ▒ ▒░▒   ░     ░ ▒  ▒   ░ ▒ ▒░   ░  ▒   ░ ░▒  ░ ░");
            Console.WriteLine($"\t\t ░ ░  ░  ░    ░     ░ ░  ░ ░ ░ ░ ▒  ░        ░  ░  ░  ");
            Console.WriteLine($"\t\t   ░     ░            ░        ░ ░  ░ ░            ░  ");
            Console.WriteLine($"\t\t ░            ░     ░               ░                 ");
            Console.WriteLine("\n\n\tWelcome to Db Docs. Press any key to continue.");
            Console.ReadLine();
        }
    }
}
