using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;

namespace SGE_erp.Venta
{
    /// <summary>
    /// Lógica de interacción para VentanaAñadir.xaml
    /// </summary>
    public partial class VentanaAñadir : UserControl
    {
        public VentanaAñadir()
        {
            InitializeComponent();
            Actualizar();
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
                SqlDataAdapter da = new SqlDataAdapter("SELECT Articulos.Id_Articulo, Nombre, PVP, Stock FROM ProveedorArticulo, Articulos WHERE ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo AND Stock > 0", con);
                DataTable dt = new DataTable(); 

                ds.Clear();
                da.Fill(dt);
                this.DatosAnadir.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                this.DatosAnadir.Columns[0].Visibility = Visibility.Collapsed;

                udStock.Minimum = 1;
                udStock.Value = 1;

                
            }catch
            {
                return;
            }
            
        }

        public void ActualizaMaximo()
        {
            DataRowView dd = (DataRowView)DatosAnadir.SelectedItem;
            int stock = dd.Row.Field<int>("Stock");
            udStock.Maximum = (uint?)stock;
        }

        private void DatosAnadir_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizaMaximo();
        }

        public void nombreCombo()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Cliente, Nombre FROM Clientes ORDER BY Nombre ASC", con);
            DataTable dt = new DataTable();
            

            ds.Clear();
            da.Fill(dt);
            this.nombreComboBox.ItemsSource = dt.DefaultView;

            nombreComboBox.DisplayMemberPath = dt.Columns["Nombre"].ToString();
            nombreComboBox.SelectedValuePath = dt.Columns["Id_Cliente"].ToString();

            con.Open();
            con.Close();

            nombreComboBox.SelectedIndex = 0;
        }

        private void nombreComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            nombreCombo();
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            
            

            /*int idArticulo;
            int idCliente;
            
           
            if (DatosAnadir.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)DatosAnadir.SelectedItem;
                idArticulo = drv.Row.Field<int>("Id_Articulo");


            }*/
            if (nombreComboBox.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)nombreComboBox.SelectedItem;
                int idCliente = drv.Row.Field<int>("Id_Cliente");

                

               // MessageBox.Show((drv.Row.Field<int>("Id_Cliente")).ToString());
            }   
            /*
            dgFinal.SelectedCells.Add(new string[] {
                Convert.ToString(DatosAnadir[0, DatosAnadir.C.Index].Value),
                Convert.ToString(DatosAnadir[1, DatosAnadir.CurrentRow.Index].Value)
            });
            */
        }
    }

    class MetodosGestion
    {
        public static String db = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Datos.mdf;Integrated Security=True";
        public static bool IsOpen(Window window)
        {
            return Application.Current.Windows.Cast<Window>().Any(x => x == window);
        }
    }
}
