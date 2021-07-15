using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.core.Helpers
{
    public static class DialogHelper
    {
        /// <summary>
        /// Opens dialog to select file.
        /// </summary>
        /// <returns>Selected file path</returns>
        public static string GetFilePath()
        {
            string path = "";
            var dialog = new OpenFileDialog();
            //dialog.Filter = "VS SQL Server Project | *.sqlproj";
            dialog.Title = "Select file";
            dialog.InitialDirectory = "C:\\";
            dialog.RestoreDirectory = true;

            Nullable<bool> results = dialog.ShowDialog();

            if ((bool)results)
            {
                path = dialog.FileName;
            }

            return path;
        }
    }
}
