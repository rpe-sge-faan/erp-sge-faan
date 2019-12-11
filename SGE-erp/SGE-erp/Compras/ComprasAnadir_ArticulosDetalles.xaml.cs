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
        public Compras_ArticulosDetalles()
        {
            InitializeComponent();
        }

        public void cargarDatos(String idArt)
        {
            SqlConnection con = new SqlConnection(ComprasAnadir.direccionbbdd);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Articulos WHERE Id_Articulo='" + idArt + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            
            this.detallesArticulos.ItemsSource = dt.DefaultView;
        }
    }
}
