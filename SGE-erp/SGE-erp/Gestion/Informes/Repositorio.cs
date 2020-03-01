using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGE_erp.Database;

namespace SGE_erp.Gestion.Informes
{
    class Repositorio
    {
        internal static dsInventario ObtenerEmpleados()
        {
            dsInventario _dsEmpleados = new dsInventario();
            using (SqlConnection cn = Gestion.MetodosGestion.Conectar())
            {
                cn.Open();
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Empleados";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_dsEmpleados, "Empleados");
                }
            }
            return _dsEmpleados;
        }
    }
}
