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
        DataTable dataT;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
            dataT = new DataTable();
            dataT.Columns.Add("Id_Articulo");
            dataT.Columns.Add("Id_Empleado");
            dataT.Columns.Add("Nombre");
            dataT.Columns.Add("PVP");
            dataT.Columns.Add("Unidades");

        }

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT Articulos.Id_Articulo, Nombre, PVP, Stock " +
                                                        "FROM ProveedorArticulo, Articulos " +
                                                        "WHERE ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo AND Stock > 0", con);
                DataTable dt = new DataTable();

                ds.Clear();
                da.Fill(dt);
                this.DatosAnadir.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                this.DatosAnadir.Columns[0].Visibility = Visibility.Collapsed;

                udStock.Minimum = 1;
                udStock.Value = 1;

            }
            catch
            {
                return;
            }

        }

        public void ActualizaMaximo()
        {
            DataRowView dd = (DataRowView)DatosAnadir.SelectedItem;
            int stock = dd.Row.Field<int>("Stock");
            udStock.Maximum = (uint?)stock;
            udStock.Value = 1;
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

        public void nombreComboEmple()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Empleado, Nombre FROM Empleados ORDER BY Nombre ASC", con);
            DataTable dt = new DataTable();


            ds.Clear();
            da.Fill(dt);
            this.nombreComboBox1.ItemsSource = dt.DefaultView;

            nombreComboBox1.DisplayMemberPath = dt.Columns["Nombre"].ToString();
            nombreComboBox1.SelectedValuePath = dt.Columns["Id_Empleado"].ToString();

            con.Open();
            con.Close();

            nombreComboBox1.SelectedIndex = 0;
        }

        private void nombreComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            nombreCombo();
        }

        private void nombreComboBox1_Loaded(object sender, RoutedEventArgs e)
        {
            nombreComboEmple();
        }

        decimal totalFinal = 0;
        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            if (DatosAnadir.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)DatosAnadir.SelectedItem;
                int idArticulo = drv.Row.Field<int>("Id_Articulo");
                String idEmpleado = nombreComboBox1.SelectedValue.ToString();
                int stock = (int)udStock.Value;
                String nombre = drv.Row.Field<String>("Nombre");
                decimal pvp = drv.Row.Field<decimal>("PVP");
                decimal totalM = 0;
                totalM = pvp * stock;

                totalFinal += totalM;
                lbTotalFin.Content = totalFinal;

                DataRow dr = null;
                dr = dataT.NewRow();
                dr["Id_Articulo"] = idArticulo;
                dr["Id_Empleado"] = idEmpleado;
                dr["Nombre"] = nombre;
                dr["PVP"] = totalM;
                dr["Unidades"] = stock;
                dataT.Rows.Add(dr);
                dgFinal.ItemsSource = dataT.DefaultView;

            }

        }

        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)DatosAnadir.SelectedItem;
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlCommand command = con.CreateCommand())
            {
                //int idEmpleado = drv.Row.Field<int>("idEmpleado");

                command.CommandText = "INSERT INTO Ventas (Id_Empleado, FechaVentas, PrecioTotal) " +
                    "VALUES (@idEmpleado, @fechaVentas, @precioTotal)";

                command.Parameters.AddWithValue("@idEmpleado", nombreComboBox1.SelectedValue);
                command.Parameters.AddWithValue("@fechaVentas", dpFecha.SelectedDate.Value);
                command.Parameters.AddWithValue("@precioTotal", lbTotalFin.Content);

                con.Open();
                int a = command.ExecuteNonQuery();


                if (a != 0)
                {
                    con.Close();

                    MessageBox.Show("Proveedor CORRECTO");
                }
                else
                {
                    MessageBox.Show("Proveedor ERROR");
                }
            }


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
