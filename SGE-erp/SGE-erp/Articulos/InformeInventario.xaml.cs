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
using SGE_erp.Gestion;

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Lógica de interacción para InformeInventario.xaml
    /// </summary>
    public partial class InformeInventario : UserControl
    {
        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;
        private AddInventario ai = null;
        public static Delegate FiltrarLista;

        public delegate void FilterList();
        public event FilterList FilterListEvent;

        public InformeInventario()
        {
            InitializeComponent();
            fechaAntes.SelectedDate = DateTime.Today;
            fechaDespues.SelectedDate = DateTime.Today;
        }

        public void RellenarTabla()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Inventario", con))
            {
                DataTable dt = new DataTable(); ;
                da.Fill(dt);

                dataGridInventario.ItemsSource = dt.DefaultView;
            }
            con.Open();
            con.Close();
        }

        
        private void AnadirInv_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(ai))
            {
                ai = new AddInventario();
                RefreshListEvent += new RefreshList(RellenarTabla);
                ai.Owner = System.Windows.Application.Current.MainWindow;
                ai.ActualizarLista = RefreshListEvent;
                ai.Show();
            }
        }

        private void InformeInv_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ActualizarStock_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ActualizarInvent_Click(object sender, RoutedEventArgs e)
        {
            RellenarTabla();
        }

        private void DataGridInventario_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridInventario.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)dataGridInventario.SelectedItem;
                int id = dd.Row.Field<int>("Id");
                DetalleInventario d = new DetalleInventario(id);
                d.ShowDialog();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RellenarTabla();
        }

        private void empleadoC_Loaded(object sender, RoutedEventArgs e)
        {
            this.empleadoC.ItemsSource = null;

            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT IdEmpleado FROM Inventario", con);
            DataTable dt = new DataTable();

            da.Fill(dt);
            con.Open();
            con.Close();

            empleadoC.DisplayMemberPath = dt.Columns["IdEmpleado"].ToString();
            this.empleadoC.ItemsSource = dt.DefaultView;
            empleadoC.InvalidateArrange();

            empleadoC.SelectedIndex = 0;
        }

        DataView view = null;
        DataTable dt;

        private void btFiltrar_Click(object sender, RoutedEventArgs e)
        {
            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)dataGridInventario.ItemsSource).ToTable();
                dt.TableName = "Inventario";
                view.Table = dt;
            }
            else
            {
                view.RowFilter = $"Fecha >= '{fechaAntes.SelectedDate}' AND Fecha <= '{fechaDespues.SelectedDate}' " +
                 $"AND IdEmpleado >= {empleadoC.SelectedIndex}";
            }
            dt = view.ToTable();
            dataGridInventario.ItemsSource = null;
            dataGridInventario.ItemsSource = dt.DefaultView;
        }
    }
}
