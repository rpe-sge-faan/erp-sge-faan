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
using SGE_erp.Administracion;

namespace SGE_erp.Venta
{
    /// <summary>
    /// Lógica de interacción para VentasVista.xaml
    /// </summary>
    public partial class VentasVista : UserControl
    {

        public static Delegate FiltrarLista;
        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;

        public delegate void FilterList();
        public event FilterList FilterListEvent;

        VentasEdicion p = null;

        public VentasVista()
        {
            InitializeComponent();
        }

        
        private void buscar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                p = new VentasEdicion(-1);
                FilterListEvent += new FilterList(Filtrar);
                p.FiltrarLista = FilterListEvent;
                p.Title = "Buscar Ventas";
                p.Owner = Application.Current.MainWindow;
                p.Show();
            }
        }

        DataView view = null;
        DataTable dt;
        public void Filtrar()
        {
            List<String> nombres = AccesoVentana();
            String[] campos = { "IdVentas", "IdEmpleado", "FechaVenta", "PrecioTotal"};

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)dgVista.ItemsSource).ToTable();
                dt.TableName = "Empleados";
                view.Table = dt;
            }

            view.RowFilter = $"Id_Ventas >= '{nombres[0]}' AND Id_Empleado = '{nombres[1]}' AND FechaVentas >= '{nombres[2]}' " +
                 $"AND PrecioTotal >= '{nombres[3]}'";

            
            dt = view.ToTable();
            dgVista.ItemsSource = null;
            dgVista.ItemsSource = dt.DefaultView;
            
        }
        

        public List<String> AccesoVentana()
        {
            List<String> nombres = new List<String>();
            foreach (Window item in Application.Current.Windows)
            {
            
                if (item.Name == "EdicionVenta")
                { 
                    String[] nombresArray = {
                        ((VentasEdicion)item).tbIdVentas.Text,
                        ((VentasEdicion)item).tbIdEmple.Text,
                        ((VentasEdicion)item).tbFecha.Text,
                        ((VentasEdicion)item).tbPrecioTotal.Text
                        
                    };
                    nombres.AddRange(nombresArray);
                }
            }
            return nombres;
        }
       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }
       

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT Id_Ventas, Id_Empleado, FechaVentas, PrecioTotal " +
                                                        "FROM Ventas ", con);
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

        private void bActualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();

        }

        private void dgVista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dato = (DataRowView)dgVista.SelectedItem;
            String idVenta = dato.Row.Field<int>("Id_Ventas").ToString();

            VentasDetalles ccd = new VentasDetalles(idVenta);
            ccd.Show();
        }

        private void facturaV_Click(object sender, RoutedEventArgs e)
        {
            Factura f = new Factura();
            f.Show();
        }
    }
}
