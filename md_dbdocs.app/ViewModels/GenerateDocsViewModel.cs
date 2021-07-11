using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.ViewModels
{
    public class GenerateDocsViewModel : BindableBase
    {
        private List<string> _modules;

        public ObservableCollection<SolutionObjectModel> SolutionObjects { get; }
        public List<string> Modules { get => _modules; set { _modules = value; base.OnPropertyChanged(); } }

        public GenerateDocsViewModel(ObservableCollection<SolutionObjectModel> solutionObjects)
        {
            SolutionObjects = solutionObjects;
            LoadDetails();
        }

        private void LoadDetails()
        {
            // extract list of modules
            //this.Modules = SolutionObjects.GroupBy(p => p.ProjectObjectModel.HeaderModel.Module).Select(m => m.First()).ToList();
        }
    }
}
