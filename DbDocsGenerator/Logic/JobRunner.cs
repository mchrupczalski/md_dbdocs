//using DbDocsGenerator.Helpers;
//using DbDocsGenerator.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace DbDocsGenerator.Logic
//{
//    public class JobRunner
//    {
//        public JobRunner()
//        {

//        }

//        internal static void TaskSelector()
//        {
//            string options = "\nSelect task options: \n" +
//                             "\t d    - update database extended properties \n" +
//                             "\t i    - update documents for db objects \n" +
//                             "\t di   - update both \n" +
//                             "\t m    - merge docs \n" +
//                             "\t exit - to finish \n";

//            string userInput = "";
//            while (userInput != "exit")
//            {
//                Console.WriteLine(options);
//                Console.Write("Option: ");
//                userInput = Console.ReadLine();

//                // redirect job
//                bool updateDb = false;
//                bool updateDocs = false;

//                switch (userInput)
//                {
//                    case "d":
//                        updateDb = true;
//                        updateDocs = false;
//                        break;
//                    case "i":
//                        updateDb = false;
//                        updateDocs = true;
//                        break;
//                    case "di":
//                        updateDb = true;
//                        updateDocs = true;
//                        break;
//                    case "m":
//                        MergeDocs();
//                        break;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine($"\n Error: { userInput } - was not recognizes as a valid option. Please try again.\n");
//                        break;
//                }

//                if (updateDb || updateDocs)
//                {
//                    string taskResult = TaskExecutor(updateDb, updateDocs);
//                    Console.WriteLine(taskResult);
//                }
//            }

//        }

//        private static void MergeDocs()
//        {
//            throw new NotImplementedException();
//        }

//        private static string TaskExecutor(bool updateDb, bool updateDocs)
//        {
//            string output = "";

//            string cnxString;
//            bool isConnected = false;

//            string server = Program.configModel.Server;
//            string db = Program.configModel.DataBase;
//            bool winAuth = Program.configModel.UseWindowsAuth;
//            string sqlLogin = Program.configModel.SqlUserName;
//            string sqlPass = Program.configModel.SqlPassword;
//            string sqlProj = Program.configModel.SqlProjectRootPath;

//            // create connection string and test connection
//            Helpers.DbConnectionHelper cnxHelp;
//            if (winAuth)
//            {
//                Console.WriteLine("Opening connection with Windows Authorization.");
//                cnxHelp = new Helpers.DbConnectionHelper(server, db, winAuth);
//            }
//            else
//            {
//                Console.WriteLine("Opening connection with SQL Authorization.");
//                cnxHelp = new Helpers.DbConnectionHelper(server, db, sqlLogin, sqlPass);
//            }

//            try
//            {
//                Console.WriteLine("Testing connection...");
//                cnxString = cnxHelp.GetAndConnectionString();
//                isConnected = cnxHelp.IsConnectionValid(cnxString);
//            }
//            catch (Exception ex)
//            {
//                output = ex.Message;
//            }

//            if (isConnected)
//            {
//                Console.WriteLine("Connected.");
//                // create procedure in dbo schema to handle addition of extended properties


//                // loop on *.sql files in project root path and subfolders
//                DirectoryInfo sqlProjInfo = new DirectoryInfo(sqlProj);
//                FileInfo[] fileInfos = sqlProjInfo.GetFiles("*.sql", SearchOption.AllDirectories);

//                foreach (var file in fileInfos)
//                {
//                    if (file.Extension == ".sql")
//                    {
//                        try
//                        {
//                            Console.WriteLine($"Processing file: { file.Name }");
//                            HeaderCommentModel header = ReadHeader(file);
//                        }
//                        catch (Exception ex)
//                        {
//                            // continue processing other files or stop??
//                            throw;
//                        }
//                    }
//                }

//                // extract string between <dbdocs> & <\dbdocs> tags
//                // ignore <change_log> & <diagram> tags
//                // deserialise from json to HeaderCommentModel
//                // execute procedure on the server to update Ext Props
//            }

//            return output;
//        }

//        private static HeaderCommentModel ReadHeader(FileInfo file)
//        {
//            /* 
//             * read given file line by line until any of the know tags found
//             * add tag to tags list
//             * add current line to a string related to a last tag in the list
//             * if closing tag found, remove from the list and switch to previous
//             */

//            // list of supported tags
//            List<string> KnownTags = new List<string>() { "dbdocs", "diagram", "change_log" };
//            string dbdocs = string.Empty;
//            string diagram = string.Empty;
//            string change_log = string.Empty;

//            // read file
//            StreamReader reader = new StreamReader(file.FullName);

//            string line;
//            string tag;
//            List<string> Tags = new List<string>();

//            while (!reader.EndOfStream)
//            {
//                // read line
//                line = reader.ReadLine();

//                // check for any known tags
//                int tagOpen = 0;
//                int tagClose = 0;
//                foreach (var knownTag in KnownTags)
//                {
//                    string findOpen = $"<{ knownTag }>";
//                    tagOpen = line.IndexOf(findOpen);
//                    // if opening tag found add to list and remove from the line
//                    if (tagOpen > -1)
//                    {
//                        Tags.Add(knownTag);
//                        line = line.Replace(findOpen, string.Empty);
//                    }

//                    string findClose = $"</{ knownTag }>";
//                    tagClose = line.IndexOf(findClose);
//                    // if closing tag found remove from the list and remove from the line
//                    if (tagClose > -1)
//                    {
//                        Tags.Remove(knownTag);
//                        line = line.Replace(findClose, string.Empty);
//                    }

//                    // remove new line and tag
//                    line = line.Replace("/n", string.Empty);
//                    line = line.Replace("/t", string.Empty);
//                }

//                // set last tag in the list as active
//                tag = Tags.Count > 0 ? Tags[Tags.Count - 1] : string.Empty;

//                // write line to string corresponding to tag
//                switch (tag)
//                {
//                    case "dbdocs":
//                        dbdocs += line + Environment.NewLine;
//                        break;
//                    case "diagram":
//                        diagram += line + Environment.NewLine;
//                        break;
//                    case "change_log":
//                        change_log += line + Environment.NewLine;
//                        break;
//                    default:
//                        break;
//                }

//            }

//            HeaderCommentModel headerCommentModel = new HeaderCommentModel();
//            try
//            {
//                using (var ys = new YamlSerializer())
//                {
//                    headerCommentModel = ys.DeSerialize<HeaderCommentModel>(dbdocs);
//                }

//                headerCommentModel.Diagram = diagram;
//                headerCommentModel.ChangeLog = change_log;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"#ERROR: { ex.Message }" + Environment.NewLine);
//                Console.WriteLine($"#ERROR: { ex.InnerException.Message }" + Environment.NewLine);
//                if (ex.InnerException.Message.IndexOf("Invalid cast from 'System.String' to 'System.Collections.Generic.List`") > -1)
//                {
//                    Console.WriteLine("#FIX: List items should start with -" + Environment.NewLine);
//                }

//                if (ex.Message.IndexOf("Line") > 0)
//                {
//                    string answer = "";
//                    while (answer != "y" && answer != "n")
//                    {
//                        Console.WriteLine("Show processed file content? y/n");
//                        answer = Console.ReadLine();
//                    }

//                    if (answer == "y")
//                    {
//                        PrintFileExtract(dbdocs);
//                    }
//                }
//                throw;
//            }

//            return headerCommentModel;
//        }

//        private static void PrintFileExtract(string dbdocs)
//        {
//            int lineCount = 0;
//            StringReader reader = new StringReader(dbdocs);
//            string aLine;

//            Console.WriteLine("Start of file");
//            while (true)
//            {
//                aLine = reader.ReadLine();
//                lineCount++;
//                if (aLine != null)
//                {
//                    Console.WriteLine($"{ lineCount }: { aLine }");
//                }
//                else
//                {
//                    break;
//                }
//            }
//            Console.WriteLine("End of file");
//        }

//        private static int FindTagPosition(string text, string findTag, int startFrom = 0) => text.IndexOf(findTag, startFrom);

//        internal static void UpdateDB()
//        {
//        }
//    }
//}
