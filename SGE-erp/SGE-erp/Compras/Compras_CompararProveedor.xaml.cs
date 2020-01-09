using SGE_erp.Gestion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace SGE_erp.Compras
{
    /// <summary>
    /// Lógica de interacción para Compras_CompararProveedor.xaml
    /// </summary>
    public partial class Compras_CompararProveedor : Window
    {
        public Compras_CompararProveedor(String idArt)
        {
            InitializeComponent();
            cargarDatos(idArt);
        }

        public void cargarDatos(String idArt)
        {
            //try
            //{
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProveedorArticulo WHERE Id_Articulo='" + idArt + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataColumn nombreProveedor = new DataColumn("Nombre Proveedor", typeof(string));
                dt.Columns.Add(nombreProveedor);
                dt.Columns["Nombre Proveedor"].SetOrdinal(3);

                DataColumn nombreArticulo = new DataColumn("Nombre Articulo", typeof(string));
                dt.Columns.Add(nombreArticulo);
                dt.Columns["Nombre Articulo"].SetOrdinal(4);

            for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlDataAdapter da2 = new SqlDataAdapter("SELECT Nombre FROM Proveedores WHERE Id_Proveedor='" + dt.Rows[i]["Id_Proveedor"] + "'", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow row = dt2.Rows[j];
                            String nProveedor = Convert.ToString(row["Nombre"]);
                            dt.Rows[i]["Nombre Proveedor"] = nProveedor;
                        }
                    }

                    SqlDataAdapter da3 = new SqlDataAdapter("SELECT Nombre FROM Articulos WHERE Id_Articulo='" + dt.Rows[i]["Id_Articulo"] + "'", con);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    if (dt3.Rows.Count > 0)
                    {

                            DataRow row = dt3.Rows[0];
                            String nArticulo = Convert.ToString(row["Nombre"]);
                            dt.Rows[i]["Nombre Articulo"] = nArticulo;

                    }
                }
                

                this.compararProveedores.ItemsSource = dt.DefaultView;
                /*this.compararProveedores.Columns[0].Visibility = Visibility.Collapsed;
                this.compararProveedores.Columns[1].Visibility = Visibility.Collapsed;
                this.compararProveedores.Columns[2].Visibility = Visibility.Collapsed;*/
           // }
            //catch
            //
//return;
           // }
            
        }
    }
}
