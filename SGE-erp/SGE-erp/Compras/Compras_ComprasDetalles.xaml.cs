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
    /// Lógica de interacción para Compras_ComprasDetalles.xaml
    /// </summary>
    public partial class Compras_ComprasDetalles : Window
    {
        public Compras_ComprasDetalles(String idCompra)
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM CompraArticulos WHERE Id_Compra=" + idCompra + ";", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataColumn nombreArt = new DataColumn("Nombre Articulo", typeof(string));
            dt.Columns.Add(nombreArt);
            dt.Columns["Nombre Articulo"].SetOrdinal(2);

            DataColumn precioArt = new DataColumn("Precio Articulo", typeof(string));
            dt.Columns.Add(precioArt);
            dt.Columns["Precio Articulo"].SetOrdinal(3);

            DataColumn precioArtTotal = new DataColumn("Precio Articulo Total", typeof(string));
            dt.Columns.Add(precioArtTotal);
            dt.Columns["Precio Articulo Total"].SetOrdinal(5);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String idElemento = Convert.ToString(row["Id_Elemento"]);
                    //MessageBox.Show(idArticulo);
                    SqlDataAdapter da2 = new SqlDataAdapter("SELECT Id_Articulo, PrecioCompra FROM [ProveedorArticulo] WHERE Id_Elemento='"
                        + idElemento + "'", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);

                    DataRow row2 = dt2.Rows[0];
                    String idArticuloDato = Convert.ToString(row2[0]);

                    SqlDataAdapter da3 = new SqlDataAdapter("SELECT Nombre FROM [Articulos] WHERE Id_Articulo='"
                        + idArticuloDato + "'", con);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);

                    DataRow row3 = dt3.Rows[0];
                    String nombreArticuloDato = Convert.ToString(row3["Nombre"]);
                    String precioArticuloDato = Convert.ToString(row2["PrecioCompra"]);
                    double precioTotalArticulos = double.Parse(precioArticuloDato) * Convert.ToDouble(row["Cantidad"]);
                    dt.Rows[i]["Nombre Articulo"] = nombreArticuloDato;
                    dt.Rows[i]["Precio Articulo"] = precioArticuloDato;
                    dt.Rows[i]["Precio Articulo Total"] = precioTotalArticulos;
                }
            this.detallesCompra.ItemsSource = dt.DefaultView;
            }
            /*this.detallesCompra.Columns[0].Visibility = Visibility.Collapsed;
            this.detallesCompra.Columns[1].Visibility = Visibility.Collapsed;*/
        }
    }
}
