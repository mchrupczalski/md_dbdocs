using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.ViewModels
{
    public class EditSourceViewModel
    {
        public SolutionObjectModel SolutionObject { get; }
        public string ObjectDetails { get; set; }

        public RelayCommand OpenADSCommand { get; set; }

        public EditSourceViewModel(SolutionObjectModel solutionObject)
        {
            SolutionObject = solutionObject;
            this.ObjectDetails = GetDetails(solutionObject);
            this.OpenADSCommand = new RelayCommand(OpenADSExecute, OpenADSCanExecute);
        }

        private void OpenADSExecute(object obj)
        {
            System.Diagnostics.Process.Start("azuredatastudio", SolutionObject.ProjectObjectModel.FileInfo.FullName);
        }

        private bool OpenADSCanExecute(object obj) => true;

        /// <summary>
        /// Gets details, about schema, object name and a list of fields/parameters
        /// </summary>
        /// <param name="solutionObject">The solution object.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetDetails(SolutionObjectModel solutionObject)
        {
            string module = solutionObject.ProjectObjectModel != null
                && solutionObject.ProjectObjectModel.HeaderModel != null
                ? solutionObject.ProjectObjectModel.HeaderModel.Module : string.Empty;
            
            bool isServerObj = solutionObject.ServerObjectModel != null;

            string schema = isServerObj ? solutionObject.ServerObjectModel.SchemaName : string.Empty;
            string objectName = isServerObj ? solutionObject.ServerObjectModel.ObjectName : string.Empty;
            string objectDesc = isServerObj ? solutionObject.ServerObjectModel.ExtendedDesc : string.Empty;
            string fieldsAndParameters = isServerObj ? GetFieldsAndParameters(solutionObject.ServerObjectModel.ChildColumns, solutionObject.ServerObjectModel.ChildParameters) : string.Empty;

            string output = "";
            output += $"\tModule: { module }";
            output += $"\n";
            output += $"\n\tSchema_Name: { schema }";
            output += $"\n\tObject_Name: { objectName }";
            output += $"\n\tDescription: { objectDesc }";
            output += $"\n";
            output += $"{ fieldsAndParameters }";

            return output;
        }

        private string GetFieldsAndParameters(List<ServerObjectChildColumnModel> childColumns, List<ServerObjectChildParameterModel> childParameters)
        {
            string output = "";

            // list fileds / columns
            if (childColumns != null && childColumns.Count > 0)
            {
                output += $"\tFields:";
                foreach (var item in childColumns)
                {
                    output += $"\n\t\t{ item.ColumnName }: { item.ExtendedDesc }";
                }
                output += "\n";
            }

            // list parameters
            if (childParameters != null && childParameters.Count > 0)
            {
                output += $"\tParameters:";
                foreach (var item in childParameters)
                {
                    string paramName = item.ParameterName.Replace("@", string.Empty);
                    output += $"\n\t\t{ paramName }: { item.ExtendedDesc }";
                }

                output += "\n";
                output += "\tReturn_Value: ";
                output += "\n";
            }

            return output;
        }
    }
}


/*
   Module: 

   Schema_Name: 
   Object_Name: WithNewHead
   Description: 

   Fields:
       Field1: Field1_Description
       Field2: Field2_Description

   Parameters:
       Param1: Param1_Description
       Param2: Param2_Description

   Return_Value: Returned_Value_Description
*/