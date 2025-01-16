using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace QuanLyTourDuLich
{
    internal class KetNoi
    {
        //public SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-8HLF4UKP\SQLEXPRESS;Initial Catalog=QL_TOUR;Integrated Security=True;TrustServerCertificate=True;");
        public SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["QL_TOUR"].ConnectionString);
    }
}
