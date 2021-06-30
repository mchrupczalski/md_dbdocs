using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Services
{
    public class DataService
    {
        private readonly SqlConnection sqlConnection;

        public DataService(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public bool IsUserDbo()
        {
            bool isDbo = false;

            string query = "SELECT CAST(COALESCE(IS_ROLEMEMBER ('db_owner'),0) AS BIT) AS IsDbo";



            SqlCommand sqlCommand = new SqlCommand(query, this.sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.Text;
            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                isDbo = dataReader.GetBoolean(0);
            }

            return isDbo;
        }

        public void CreateDbObjects()
        {
            //throw new NotImplementedException();
        }

        private void ClearDbObjects()
        {
            //throw new NotImplementedException();
        }
    }
}
