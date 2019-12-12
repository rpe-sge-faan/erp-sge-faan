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
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProveedorArticulo WHERE Id_Articulo='" + idArt + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            this.compararProveedores.ItemsSource = dt.DefaultView;
        }
    }
}
