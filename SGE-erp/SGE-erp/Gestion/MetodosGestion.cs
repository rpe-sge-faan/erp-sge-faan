using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SGE_erp.Gestion
{
    class MetodosGestion
    {
        public static String db = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Datos.mdf;Integrated Security=True;MultipleActiveResultSets=True";
        public static bool IsOpen (Window window){
            return Application.Current.Windows.Cast<Window>().Any(x => x == window);
        }

        public static SqlConnection Conectar()
        {
            try
            {
                String con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Datos.mdf;Integrated Security=True";
                SqlConnection cn = new SqlConnection(db);
                return cn;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error de conexión", ex);
            }

        }
    }
}
