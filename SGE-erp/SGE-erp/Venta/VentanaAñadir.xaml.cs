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
using SGE_erp.Gestion;
using System.Diagnostics;

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
            if (DatosAnadir.SelectedCells != null)
            {
                ActualizaMaximo();
            }
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
            int idArticulo = drv.Row.Field<int>("Id_Articulo");


            int idEmpl = (int)nombreComboBox1.SelectedValue;
            DateTime fecha = dpFecha.SelectedDate.Value;
            //decimal precio = (decimal)lbTotalFin.Content;

            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection conn = new SqlConnection(bd))
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [Ventas] (Id_Empleado, FechaVentas, PrecioTotal) VALUES (@idEmpleado, @fechaVentas, @precioTotal)";

                    command.Parameters.AddWithValue("@idEmpleado", 1);
                    command.Parameters.AddWithValue("@fechaVentas", fecha);
                    command.Parameters.AddWithValue("@precioTotal", 34.6);

                    conn.Open();
                    int a = command.ExecuteNonQuery();

                    if (a != 0)
                    {
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("ERROR");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
