using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace md_dbdocs.app.Services
{
    public static class ViewNavigationService
    {
        public static MainWindow ViewNav;

        public static void Switch(UserControl newView)
        {
            ViewNav.Navigate(newView);
        }
    }
}
