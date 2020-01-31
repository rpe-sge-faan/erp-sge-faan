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

        public delegate void FilterList();
        public event FilterList FilterListEvent;

        VentasEdicion p = null;

        public VentasVista()
        {
            InitializeComponent();
        }


        private void Buscar_Click(object sender, RoutedEventArgs e)
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

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)dgVista.ItemsSource).ToTable();
                dt.TableName = "Empleados";
                view.Table = dt;
            }

            if (nombres[0].Equals("") && nombres[1].Equals(""))
            {
                view.RowFilter = $"FechaVentas >= '{nombres[2]}' AND FechaVentas <= '{nombres[3]}' AND PrecioTotal >= {nombres[4]}";
            }
            else if (nombres[0].Equals(""))
            {
                view.RowFilter = $"Id_Empleado = {nombres[1]} AND FechaVentas >= '{nombres[2]}' AND FechaVentas <= '{nombres[3]}' " +
                 $"AND PrecioTotal >= {nombres[4]}";
            }
            else if (nombres[1].Equals(""))
            {
                view.RowFilter = $"Id_Ventas = '{nombres[0]}' AND FechaVentas >= '{nombres[2]}' AND FechaVentas <= '{nombres[3]}' " +
                 $"AND PrecioTotal >= {nombres[4]}";
            }
            else
            {
                view.RowFilter = $"Id_Ventas = {nombres[0]} AND Id_Empleado = {nombres[1]} AND FechaVentas >= '{nombres[2]}' AND FechaVentas <= '{nombres[3]}' " +
                 $"AND PrecioTotal >= {nombres[4]}";
            }



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
                        ((VentasEdicion)item).tbFechaAnt.Text,
                         ((VentasEdicion)item).tbFechaPos.Text,
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
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter(@"SELECT Id_Ventas, Id_Empleado, [FechaVentas], PrecioTotal " +
                                                    "FROM Ventas ", con))
            {
                DataTable dt = new DataTable(); ;

                da.Fill(dt);
                this.dgVista.ItemsSource = dt.DefaultView;
            }
        }

        private void DgVista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BActualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void DgVista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgVista.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)dgVista.SelectedItem;
                String idVenta = dato.Row.Field<int>("Id_Ventas").ToString();

                VentasDetalles ccd = new VentasDetalles(idVenta);
                ccd.Show();
            }

        }

        Factura f;
        private void FacturaV_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(f))
            {
                if (dgVista.SelectedItem != null)
                {
                    DataRowView dato = (DataRowView)dgVista.SelectedItem;
                    int idVenta = dato.Row.Field<int>("Id_Ventas");

                    f = new Factura(idVenta, 1);
                    f.Show();
                }
            }
            else
            {
                Mensajes.Mostrar("Ya hay una factura abierta", Mensajes.Tipo.Info);
            }
        }

        private void informeV_Click(object sender, RoutedEventArgs e)
        {
            if (dgVista != null)
            {
                int[] ids = new int[dgVista.Items.Count];
                int cont = 0;

                foreach (DataRowView row in dgVista.Items)
                {
                    ids[cont++] = int.Parse(row["Id_Ventas"].ToString());
                }
                //sacar ids


                InformeVenta ina = new InformeVenta(ids);
                ina.Show();
            }
        }
    }
}
