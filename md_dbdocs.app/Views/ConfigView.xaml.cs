using md_dbdocs.app.ViewModels;
using System.Windows.Controls;

namespace md_dbdocs.app.Views
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class ConfigView : UserControl
    {
        private readonly ConfigViewModel viewModel;

        public ConfigView()
        {
            InitializeComponent();

            this.viewModel = new ConfigViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
