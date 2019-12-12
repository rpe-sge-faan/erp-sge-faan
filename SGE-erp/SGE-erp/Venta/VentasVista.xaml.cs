using System;
using System.Collections.Generic;
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
using SGE_erp.Gestion;
using System.Data;

namespace SGE_erp.Venta
{
    /// <summary>
    /// Lógica de interacción para VentasVista.xaml
    /// </summary>
    public partial class VentasVista : UserControl
    {
        public VentasVista()
        {
            InitializeComponent();
            Actualizar();
        }


        private void buscar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                p = new VentaEdicion(-1);
                FilterListEvent += new FilterList(Filtrar);
                p.FiltrarLista = FilterListEvent;
                p.Title = "Buscar Empleado";
                p.Owner = Application.Current.MainWindow;
                p.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodoGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT VentasArticulos.Id_Ventas, Id_Empleado, FechaVentas, VentasArticulos.Cantidad, PrecioTotal " +
                                                        "FROM VentasArticulos, Ventas " +
                                                        "WHERE VentasArticulos.Id_Ventas = Ventas.Id_Ventas", con);
                DataTable dt = new DataTable(); ;

                ds.Clear();
                da.Fill(dt);
                this.dgVista.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();
            }
            catch
            {
                return;
            }

        }
    }

    class MetodoGestion
    {
        public static String db = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Datos.mdf;Integrated Security=True";
        public static bool IsOpen(Window window)
        {
            return Application.Current.Windows.Cast<Window>().Any(x => x == window);
        }
    }
}
