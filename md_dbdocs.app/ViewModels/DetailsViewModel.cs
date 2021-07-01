using md_dbdocs.app.DataAccess;
using md_dbdocs.app.Models;
using md_dbdocs.app.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.ViewModels
{
    public class DetailsViewModel
    {
        private readonly ConfigModel _configModel;
        private readonly SqlConnection _sqlConnection;

        public DetailsViewModel(ConfigModel configModel, SqlConnection sqlConnection)
        {
            this._configModel = configModel;
            _sqlConnection = sqlConnection;

            LoadDetails();
        }

        public void LoadDetails()
        {
            /* create table in db and populate with object details
             * create list of all *.sql files in project folder and subfolders
             * check each file for tags and assign bool val if found
             * if dbdocs found, extract yaml and serialise to DetailsModel
             * find CREATE statement and determine object schema and name
             * send to sql server 
             */

            /*
             * scan all files in project directory
             * if CREATE statement found in file
             * extract schema and name of the created object
             * add to list of FileDetailsModel
             * check if tag dbdocs in file
             * if found deserialize to HeaderModel
             * get object details from the server and add to ObjectModel
             */


            var scanner = new FileScannerService(_configModel);
            scanner.PopulateProjectFilesInfoTable();
        }
    }
}
