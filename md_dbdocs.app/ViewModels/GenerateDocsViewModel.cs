using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace md_dbdocs.app.ViewModels
{
    public class GenerateDocsViewModel : BindableBase
    {
        private List<string> _modules;

        public ObservableCollection<SolutionObjectModel> SolutionObjects { get; }
        public List<string> Modules { get => _modules; set { _modules = value; base.OnPropertyChanged(); } }
        public string OutputDir { get; set; }

        public GenerateDocsViewModel(ObservableCollection<SolutionObjectModel> solutionObjects)
        {
            SolutionObjects = solutionObjects;
            LoadDetails();
        }

        private void LoadDetails()
        {
            // extract list of modules
            this.Modules = (List<string>)SolutionObjects.GroupBy(p => p.ProjectObjectModel.HeaderModel.Module).Select(m => m.FirstOrDefault());
        }
    }
}

/*
 * SectionId - 1.0.0.0
 * SectionLevel
 * SectionTitle - General Info / Module: Core
 * SectionLink - [#link]
 * SectionFilePath
 * 
 * Form divided into 3 parts
 * 1 - general docs info
 * 2 - schemas & roles
 * 3 - modules
 * 3.1 - general info - desc + svg from dbml
 * 3.2 - functions
 * 3.3 - procedures
 * 3.4 - tables
 * 3.5 - types
 * 3.6 - views
 * 
 * ex:
 * 3.1.1.0 - Module.Core.General
 * 3.1.2.0 - Module.Core.Functions
 * 3.1.2.1 - Module.Core.Functions.FunctionName1
 * 3.1.2.2 - Module.Core.Functions.FunctionName2
 * 3.1.3.0 - Module.Core.Procedures
 * 3.1.3.1 - Module.Core.Procedures.Procedure1
 * 
 */
 