using md_dbdocs.app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace md_dbdocs.app.Views
{
    /// <summary>
    /// Interaction logic for EditSourceView.xaml
    /// </summary>
    public partial class EditSourceView : UserControl
    {
        public EditSourceViewModel ViewModel { get; }

        public EditSourceView(EditSourceViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
        }

    }
}
