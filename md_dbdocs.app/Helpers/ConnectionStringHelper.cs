using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string GetConnectionString(ConfigModel configModel)
        {
            string cnxString;

            if (configModel.UseWindowsAuth)
            {
                cnxString = $"Server={ configModel.Server };" +
                            $"Database={ configModel.DataBase };" +
                            $"Trusted_Connection=True;";
            }
            else
            {
                cnxString = $"Server={ configModel.Server };" +
                            $"Database={ configModel.DataBase };" +
                            $"User Id={ configModel.SqlUserName };" +
                            $"Password={ configModel.SqlPassword };";
            }

            return cnxString;
        }
    }
}
