using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SGE_erp.Database;


namespace SGE_erp.Articulos
{
    class InventarioRepo
    {
        internal static dsInventario ObtenerDatos(int idInventario)
        {
            dsInventario dsInventarioObj = new dsInventario();
            using (SqlConnection cn = Gestion.MetodosGestion.Conectar(""))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * " +
                        "FROM Inventario WHERE Id = " + idInventario;
                    da.SelectCommand = cmd;
                    da.Fill(dsInventarioObj, "Inventario");
                }
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IdArticulo, NombreArticulo, UnidadesContadas, UnidadesStock, PrecioCompra, PrecioVenta " +
                        "FROM InventarioArticulos WHERE IdInventario = " + idInventario;
                    da.SelectCommand = cmd;
                    dsInventarioObj.EnforceConstraints = false;
                    da.Fill(dsInventarioObj, "InventarioArticulos");
                }
                
            }
            return dsInventarioObj;
        }
    }
}
