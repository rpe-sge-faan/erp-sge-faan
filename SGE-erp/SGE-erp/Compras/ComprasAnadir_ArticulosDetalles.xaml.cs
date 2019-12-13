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
    /// Lógica de interacción para Compras_ArticulosDetalles.xaml
    /// </summary>
    public partial class Compras_ArticulosDetalles : Window
    {
        public Compras_ArticulosDetalles(String idArt)
        {
            InitializeComponent();
            cargarDatos(idArt);
        }

        public void cargarDatos(String idArt)
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Articulos WHERE Id_Articulo='" + idArt + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataColumn tipo = new DataColumn("Tipo Articulo", typeof(string));
            dt.Columns.Add(tipo);

            DataColumn iva = new DataColumn("Iva", typeof(string));
            dt.Columns.Add(iva);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Id_Iva"].Equals(1))
                {
                    dt.Rows[i]["Iva"] = "0%";
                }
                else if (dt.Rows[i]["Id_Iva"].Equals(2))
                {
                    dt.Rows[i]["Iva"] = "4%";
                }
                else if (dt.Rows[i]["Id_Iva"].Equals(3))
                {
                    dt.Rows[i]["Iva"] = "10%";
                }
                else if (dt.Rows[i]["Id_Iva"].Equals(2))
                {
                    dt.Rows[i]["Iva"] = "21%";
                }

                SqlDataAdapter da2 = new SqlDataAdapter("SELECT Descripcion FROM TipoArticulo WHERE Id_Tipo='" + dt.Rows[i]["TipoArticulo"] + "'", con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow row = dt2.Rows[j];
                        String Descripcion = Convert.ToString(row["Descripcion"]);
                        dt.Rows[i]["Tipo Articulo"] = Descripcion;
                    }
                }
            }

            this.detallesArticulos.ItemsSource = dt.DefaultView;
            /*detallesArticulos.Columns[0].Visibility = Visibility.Collapsed;
            detallesArticulos.Columns[1].Visibility = Visibility.Collapsed;*/
        }
    }
}
