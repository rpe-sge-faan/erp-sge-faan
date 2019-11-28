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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGE_erp.Compras
{
    /// <summary>
    /// Lógica de interacción para ComprasGuay.xaml
    /// </summary>
    public partial class ComprasAnadir : UserControl
    {
        public ComprasAnadir()
        {
            InitializeComponent();
            Actualizar();
        }

        private void Actualizar()
        {
            try
            {
                string bd = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|DeBaseDatos.mdf;Integrated Security=True";
                SqlConnection con = new SqlConnection(bd);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
                DataTable dt = new DataTable(); ;

                ds.Clear();
                da.Fill(dt);
                this.proveedores.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();
            }
            catch
            {
                return;
            }
        }

        private void buscarArtProv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(proveedores.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)proveedores.SelectedItem;
                String idProv = dato.Row.Field<int>("Id_Proveedor").ToString();

                try
                {
                    string bd = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|DeBaseDatos.mdf;Integrated Security=True";
                    SqlConnection con = new SqlConnection(bd);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT PrecioCompra,PVP,Stock FROM [ProveedorArticulo] WHERE Id_Proveedor='" + idProv + "'", con);
                    DataTable dt = new DataTable();

                    ds.Clear();
                    da.Fill(dt);
                    this.articulos.ItemsSource = dt.DefaultView;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Articulos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(articulos.SelectedItem != null)
            {
                Compras_ArticulosDetalles cad = new Compras_ArticulosDetalles();
                cad.ShowDialog();
                articulos.SelectedItem = null;
            }            
        }
    }
}
