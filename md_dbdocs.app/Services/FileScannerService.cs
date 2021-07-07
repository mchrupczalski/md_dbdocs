using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using md_dbdocs.app.Models.YamlModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace md_dbdocs.app.Services
{
    public class FileScannerService
    {
        private readonly ConfigModel _configModel;

        public FileScannerService(ConfigModel configModel)
        {
            _configModel = configModel;
        }

        /// <summary>
        /// Gets the project objects.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ProjectObjectModel> GetProjectObjects()
        {
            Dictionary<string, ProjectObjectModel> objDic = new Dictionary<string, ProjectObjectModel>();

            var objList = new List<ProjectObjectModel>();

            // get list of files
            DirectoryInfo sqlProjInfo = new DirectoryInfo(this._configModel.SqlProjectRootPath);
            FileInfo[] fileInfos = sqlProjInfo.GetFiles("*.sql", SearchOption.AllDirectories);

            foreach (FileInfo file in fileInfos)
            {
                // ignore files from ./obj/ & ./bin/ folders
                bool isDebugOrRelease = (file.FullName.ToLower().IndexOf("\\obj\\") >= 0) || (file.FullName.ToLower().IndexOf("\\bin\\") >= 0);

                // second filter on sql files required as GetFiles also returning sqlproject, etc
                if ((file.Extension == ".sql") && (file.Name.IndexOf(".publish.sql") == -1) && !isDebugOrRelease)
                {
                    var projFile = new ProjectObjectModel();
                    projFile.FileInfo = file;

                    // check if CREATE statement - get Obj Type, Schema and Name
                    bool isCreate = GetSqlCreateDetails(projFile);

                    // skip file if it is not creating anything
                    if (isCreate)
                    {
                        // check file again and look for tags
                        GetDbDocsTags(projFile);

                        // add to dictionary
                        objList.Add(projFile);

                        // it should not happen, but an error occurs if duplicate object definition found
                        string key = $"{ Helpers.SqlObjectTypeTranslator.GetObjectTypeId(projFile.CreateObjectType) }.{ projFile.CreateObjectSchema.ToLower() }.{ projFile.CreateObjectName.ToLower() }";
                        try
                        {
                            objDic.Add(key, projFile);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "An item with the same key has already been added.")
                            {
                                // find duplicate file and show both in an mbox
                                string fileOne = objDic[key].FileInfo.FullName;
                                string msg = $"Whooops. Duplicate object definition found.\n" +
                                             $"File one: { fileOne }\n\n" +
                                             $"File two: { projFile.FileInfo.FullName }\n\n" +
                                             $"The first file will be used for current run, the second file will be ignored.";
                                System.Windows.MessageBox.Show(msg, "Duplicate found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                            }
                        }
                    }                    
                }
            }

            return objDic;

        }



        /// <summary>
        /// Scans project file in search for dbdocs tags and serialize to class.
        /// </summary>
        /// <param name="projFile">The proj file.</param>
        private void GetDbDocsTags(ProjectObjectModel projFile)
        {
            /* 
             * read given file line by line until any of the know tags found
             * add tag to tags list
             * add current line to a string related to a last tag in the list
             * if closing tag found, remove from the list and switch to previous tag
             */

            // ToDo: add <dbdoc_col>

            // list of supported tags
            List<string> knownTags = new List<string>() { "dbdocs", "diagram", "change_log" };
            string dbdocs = string.Empty;
            string diagram = string.Empty;
            string change_log = string.Empty;

            // read file
            StreamReader reader = projFile.GetFileText();

            string line;
            string tag;
            List<string> Tags = new List<string>();

            while (!reader.EndOfStream)
            {
                // read line
                line = reader.ReadLine();

                // check for any known tags
                int tagOpen = 0;
                int tagClose = 0;
                foreach (var knownTag in knownTags)
                {
                    string findOpen = $"<{ knownTag }>";
                    tagOpen = line.IndexOf(findOpen);
                    // if opening tag found add to list and remove from the line
                    if (tagOpen > -1)
                    {
                        Tags.Add(knownTag);
                        line = line.Replace(findOpen, string.Empty);
                    }

                    string findClose = $"</{ knownTag }>";
                    tagClose = line.IndexOf(findClose);
                    // if closing tag found remove from the list and remove from the line
                    if (tagClose > -1)
                    {
                        Tags.Remove(knownTag);
                        line = line.Replace(findClose, string.Empty);
                    }

                    // remove new line and tab
                    //line = line.Replace("/n", string.Empty);
                    //line = line.Replace("/t", string.Empty);
                }

                // set last tag in the list as active
                tag = Tags.Count > 0 ? Tags[Tags.Count - 1] : string.Empty;

                // write line to string corresponding to tag
                switch (tag)
                {
                    case "dbdocs":
                        dbdocs += line + Environment.NewLine;
                        break;
                    case "diagram":
                        diagram += line + Environment.NewLine;
                        break;
                    case "change_log":
                        change_log += line + Environment.NewLine;
                        break;
                    default:
                        break;
                }

            }

            var headerCommentModel = new HeaderCommentModel();
            try
            {
                if (!string.IsNullOrEmpty(dbdocs))
                {
                    using (var ys = new YamlSerializer())
                    {
                        headerCommentModel = ys.DeSerialize<HeaderCommentModel>(dbdocs);
                    }
                }

                headerCommentModel.Diagram = diagram;
                headerCommentModel.ChangeLog = change_log;

                // if tags found assign properties to projFile model
                projFile.HasTagDbDocs = !string.IsNullOrEmpty(dbdocs);
                projFile.HasTagDiagram = !string.IsNullOrEmpty(diagram);
                projFile.HasTagChangeLog = !string.IsNullOrEmpty(change_log);

                projFile.HeaderModel = headerCommentModel;
            }
            catch (Exception ex)
            {
                projFile.ProcessingExceptions.Add(ex);
            }
        }

        /// <summary>
        /// Gets the sql CREATE details.
        /// </summary>
        /// <param name="projFile">The proj file.</param>
        private bool GetSqlCreateDetails(ProjectObjectModel projFile)
        {
            bool output = false;

            List<string> supportedObjTypes = new List<string>()
            {
                "FUNCTION","PROCEDURE","TABLE","TYPE","VIEW" /*,"ROLE","SCHEMA" --Commented out as CREATE signature differs from other types */
            };

            StreamReader reader = projFile.GetFileText();
            string line;
            int createPos;

            // loop line by line until CREATE found, then extract Object Type, Schema and Name
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                createPos = line.ToUpper().IndexOf("CREATE ");
                if (createPos > -1)
                {
                    // remove any double spacing sets of brackets "[]", "()"
                    line = ReplaceAllString("  ", " ", line);
                    line = ReplaceAllString("[", "", line);
                    line = ReplaceAllString("]", "", line);
                    line = ReplaceAllString("(", "", line);
                    line = ReplaceAllString(")", "", line);

                    string[] lineSplit = line.Split(' ');

                    bool isSupported = false;
                    string objType = lineSplit[1].ToUpper();
                    string objSchema = "";
                    string objName = lineSplit[2];

                    if (objType == "ROLE" || objType == "SCHEMA")
                    {
                        isSupported = true;
                        objSchema = objName;
                    }
                    else if (supportedObjTypes.Contains(objType))
                    {
                        isSupported = true;

                        // check how many part name (server.db.schema.name)
                        string[] nameSplit = objName.Split('.');
                        if (nameSplit.Length > 1)
                        {
                            objSchema = nameSplit[nameSplit.Length - 2];
                            objName = nameSplit[nameSplit.Length - 1];
                        }
                    }

                    // attach properties if file contains definition for supported object
                    if (isSupported)
                    {
                        projFile.CreateObjectType = objType;
                        projFile.CreateObjectSchema = objSchema;
                        projFile.CreateObjectName = objName;

                        // CREATE found - stop scanning
                        output = true;
                        return output;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Replaces all occurrences of given string.
        /// </summary>
        /// <param name="findOld">The find old.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <param name="searchIn">The search in.</param>
        /// <returns></returns>
        private string ReplaceAllString(string findOld, string replaceWith, string searchIn)
        {
            string output = searchIn;

            while (output.IndexOf(findOld) > -1)
            {
                output = output.Replace(findOld, replaceWith);
            }

            return output;
        }
    }
}
