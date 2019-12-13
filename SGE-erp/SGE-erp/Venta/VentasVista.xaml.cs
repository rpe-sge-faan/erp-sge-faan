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

        public static Delegate FiltrarLista;
        public delegate void RefreshList();

        public delegate void FilterList();

        VentaEdicion p = null;

        public VentasVista()
        {
            InitializeComponent();
            Actualizar();
        }

        
        private void buscar_Click(object sender, RoutedEventArgs e)
        {
           /* if (!MetodosGestion.IsOpen(p))
            {
                p = new VentaEdicion(-1);
                FilterListEvent += new FilterList(Filtrar);
                p.FiltrarLista = FilterListEvent;
                p.Title = "Buscar Ventas";
                p.Owner = Application.Current.MainWindow;
                p.Show();
            }*/
        }
/*
        DataView view = null;
        DataTable dt;
        public void Filtrar()
        {
            List<String> nombres = AccesoVentana();
            String[] campos = { "IdEmpleado", "FechaVenta", "Cantidad", "PrecioTotal"};

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)dgVista.ItemsSource).ToTable();
                dt.TableName = "Empleados";
                view.Table = dt;
            }

            DateTime date = DateTime.Parse(nombres[4]);
            // Console.WriteLine(dt.ToString("dd/MM/yyyy"));

            // [NumVentas] >= {nombres[5]} AND 
            view.RowFilter = $"Id_Empleado LIKE '%{nombres[0]}%' AND FechaVentas LIKE '%{nombres[1]}%' AND Cantidad LIKE '%{nombres[2]}%' " +
                 $"AND PrecioTotal LIKE '%{nombres[3]}%'";

            // view.Sort = "CompanyName DESC";
            dt = view.ToTable();
            dgVista.ItemsSource = null;
            dgVista.ItemsSource = dt.DefaultView;
            dgVista.Columns[0].Visibility = Visibility.Collapsed;
        }
        // params string[] nombres


        public List<String> AccesoVentana()
        {
            List<String> nombres = new List<String>();
            foreach (Window item in Application.Current.Windows)
            {
            
                if (item.Name == "EdicionVenta")
                { 
                    String[] nombresArray = {
                        ((VentaEdicion)item).nifTextBox.Text,
                        ((VentaEdicion)item).telefonoTextBox.Text,
                        ((VentaEdicion)item).emailTextBox.Text,
                        ((VentaEdicion)item).direccionTextBox.Text
                        
                    };
                    nombres.AddRange(nombresArray);
                }
            }
            return nombres;
        }
        */
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }
       

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodoGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT VentasArticulos.Id_Ventas, Id_Empleado, FechaVentas, VentasArticulos.Cantidad, PrecioTotal " +
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

        private void dgVista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
