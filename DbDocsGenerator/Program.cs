using DbDocsGenerator.DataAccess;
using DbDocsGenerator.Helpers;
using DbDocsGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // show welcome message
            WelcomeScreen.ShowMessage();

            // create generator model
            GeneratorModel generator = ConfigureAndConnect();

            // make sure app can connect to db
            if (generator.DataAccess.IsConnected)
            {
                Console.WriteLine("Will do the job.");
            }
        }

        /// <summary>
        /// Run configuration and connect to db
        /// </summary>
        private static GeneratorModel ConfigureAndConnect()
        {
            GeneratorModel generatorModel = new GeneratorModel();

            string userInput = "";
            bool dbConnected = false;

            while((userInput != "exit") && (!dbConnected))
            {
                // display current config, ask user if correct
                var configHelp = new ConfigHelper();
                generatorModel.ConfigModel = configHelp.GetConfigModel();

                // create and validate db connection
                var dbHelp = new DataAccessService(generatorModel.ConfigModel);
                dbConnected = dbHelp.IsConnected;
                generatorModel.DataAccess = dbHelp;

                // if connection failed, ask user if run config again, or exit?
                if (!dbConnected)
                {
                    Console.WriteLine("Connection failed. \nConfiguration will run again. \nHit [Enter] to continue or type exit to close app.");
                    userInput = Console.ReadLine();
                }
            }

            return generatorModel;
        }
    }

    
}
