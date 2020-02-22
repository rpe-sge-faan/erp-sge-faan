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

        public InformeInventario()
        {
            InitializeComponent();
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

        private void BuscarInv_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
