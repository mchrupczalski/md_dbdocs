using md_dbdocs.app.ViewModels;
using System.Windows.Controls;

namespace md_dbdocs.app.Views
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : UserControl
    {
        private readonly DetailsViewModel _viewModel;

        public DetailsView(DetailsViewModel viewModel)
        {
            InitializeComponent();
            this._viewModel = viewModel;
            this.DataContext = _viewModel;
        }
    }
}
