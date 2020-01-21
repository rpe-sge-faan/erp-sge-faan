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
        public static DataTable dataT;
        public static int id;
        public VentanaAñadir()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
            dpFecha.SelectedDate = DateTime.Today;
            dataT = new DataTable();
            dataT.Columns.Add("Id_Articulo");
            dataT.Columns.Add("Id_Empleado");
            dataT.Columns.Add("Id_Elemento");
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
                SqlDataAdapter da = new SqlDataAdapter("SELECT Articulos.Id_Articulo, ProveedorArticulo.Id_Elemento, Nombre, Descripcion, PVP, Stock " +
                                                        "FROM ProveedorArticulo, Articulos " +
                                                        "WHERE ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo AND Stock > 0", con);
                DataTable dt = new DataTable();

                ds.Clear();
                da.Fill(dt);
                this.DatosAnadir.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                this.DatosAnadir.Columns[0].Visibility = Visibility.Collapsed;

                udStock.minvalue = 1;
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
            if (dd != null)
            {
                int stock = dd.Row.Field<int>("Stock");
                udStock.maxvalue = stock;
                udStock.Value = 1;
            }
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
            this.cbCliente.ItemsSource = null;
            //comboBox1.Items.Clear();
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Cliente, Nombre FROM Clientes ORDER BY Nombre ASC", con);
            DataTable dt = new DataTable();

            da.Fill(dt);
            con.Open();
            con.Close();

            this.cbCliente.ItemsSource = dt.DefaultView;

            cbCliente.DisplayMemberPath = dt.Columns["Nombre"].ToString();
            cbCliente.SelectedValuePath = dt.Columns["Id_Cliente"].ToString();

            cbCliente.InvalidateArrange();

            cbCliente.SelectedIndex = 0;
        }

        private void cbCliente_Loaded(object sender, RoutedEventArgs e)
        {
            nombreCombo();
        }

        decimal totalFinal = 0;
        int guardarCantidad = 0;
        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            if (DatosAnadir.SelectedItem != null)
            {
                //Tabla de arriba
                DataTable dta = new DataTable();
                dta = ((DataView)DatosAnadir.ItemsSource).ToTable();
                //Tabla de abajo
                DataRowView drv = (DataRowView)DatosAnadir.SelectedItem;
                int rowIndex = DatosAnadir.Items.IndexOf(DatosAnadir.SelectedItem);
                int idArticulo = drv.Row.Field<int>("Id_Articulo");
                int idEmpleado = MainWindow.idEmpleado;
                int idElemento = drv.Row.Field<int>("Id_Elemento");
                int stock = (int)udStock.Value;
                String nombre = drv.Row.Field<String>("Nombre");
                decimal pvp = drv.Row.Field<decimal>("PVP");
                decimal totalM = 0;
                totalM = pvp * stock;

               

                Boolean encontrado = false;

                int index = -1;

                for (int i = 0; i < dataT.Rows.Count; i++)
                {
                    int id = Convert.ToInt32(dataT.Rows[i]["Id_Articulo"]);
                    if (id == idArticulo)
                    {
                        encontrado = true;
                        index = i;
                        break;
                    }

                }
                if (Convert.ToInt32(dta.Rows[rowIndex]["Stock"]) > 0)
                {


                    if (encontrado)
                    {
                        dataT.Rows[index]["Unidades"] = Convert.ToInt32(dataT.Rows[index]["Unidades"]) + stock;
                        dataT.Rows[index]["PVP"] = Convert.ToDecimal(dataT.Rows[index]["PVP"]) + (pvp * stock);
                    }
                    else
                    {
                        DataRow dr = null;
                        dr = dataT.NewRow();
                        dr["Id_Articulo"] = idArticulo;
                        dr["Id_Empleado"] = idEmpleado;
                        dr["Id_Elemento"] = idElemento;
                        dr["Nombre"] = nombre;
                        dr["PVP"] = totalM;
                        dr["Unidades"] = stock;

                        dataT.Rows.Add(dr);
                        dgFinal.ItemsSource = dataT.DefaultView;
                    }


                    dta.Rows[rowIndex]["Stock"] = Convert.ToInt32(dta.Rows[rowIndex]["Stock"]) - stock;
                    DatosAnadir.ItemsSource = dta.DefaultView;
                    DatosAnadir.SelectedIndex = rowIndex;

                    totalFinal += totalM;
                    lbTotalFin.Content = $"{totalFinal}€";

                    guardarCantidad += stock;
                }
                else
                {
                    
                    Mensajes.Mostrar("No existe stock", Mensajes.Tipo.Info);
                }



            }
        }

        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            if (dgFinal.Items.Count != 0)
            {
                int idCliente = (int)cbCliente.SelectedValue;
                int idEmpl = MainWindow.idEmpleado;
                DateTime fecha = dpFecha.SelectedDate.Value;
                decimal precio = totalFinal;


                try
                {
                    string bd = MetodosGestion.db;
                    using (SqlConnection conn = new SqlConnection(bd))
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [Ventas] (Id_Empleado, Id_Cliente, FechaVentas, PrecioTotal)  " +
                            "OUTPUT INSERTED.Id_Ventas " +
                            "VALUES (@idEmpleado, @idCliente, @fechaVentas, @precioTotal)";

                        command.Parameters.AddWithValue("@idEmpleado", idEmpl);
                        command.Parameters.AddWithValue("@idCliente", idCliente);
                        command.Parameters.AddWithValue("@fechaVentas", fecha);
                        command.Parameters.AddWithValue("@precioTotal", precio);

                        conn.Open();
                        id = (int)command.ExecuteScalar();

                        if (id != 0)
                        {

                            Mensajes.Mostrar("Vendido", Mensajes.Tipo.Info);
                            conn.Close();
                        }
                        else
                        {

                            Mensajes.Mostrar("ERROR", Mensajes.Tipo.Error);
                        }
                    }

                    for (int i = 0; i < dataT.Rows.Count; i++)
                    {

                        DataRow row = dataT.Rows[i];

                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand comando = con.CreateCommand())
                        {
                            con.Open();
                            comando.CommandText = @"INSERT INTO [VentasArticulos] (Id_Ventas, Id_Elemento, Cantidad)" +
                                "VALUES (@idVentas, @idElemento, @cantidad)";

                            comando.Parameters.AddWithValue("@idVentas", id);
                            comando.Parameters.AddWithValue("@idElemento", row[2]);
                            comando.Parameters.AddWithValue("@cantidad", row[5]);


                            int a = comando.ExecuteNonQuery();

                            if (a != 0)
                            {

                                con.Close();
                            }
                            else
                            {
                                Mensajes.Mostrar("ERROR", Mensajes.Tipo.Error);
                            }
                        }
                        // INSERT EN LA TABLA DE MOVIMIENTOS
                        string idArt = "";
                        using (SqlConnection conex = new SqlConnection(MetodosGestion.db))
                        using (SqlCommand command = conex.CreateCommand())
                        {
                            command.CommandText = "SELECT Id_Articulo FROM ProveedorArticulo WHERE Id_Elemento = @idele";
                            command.Parameters.AddWithValue("@idele", row[2]);
                            conex.Open();
                            using (var reader = command.ExecuteReader())
                            {

                                if (reader.Read())
                                {
                                    idArt = reader.GetInt32(reader.GetOrdinal("Id_Articulo")).ToString();
                                    //MessageBox.Show(idArt);
                                }
                                else Mensajes.Mostrar("ERROR", Mensajes.Tipo.Error);
                            }
                        }
                        int stock = 0;
                        using (SqlConnection conex = new SqlConnection(MetodosGestion.db))
                        using (SqlCommand command = conex.CreateCommand())
                        {
                            command.CommandText = "SELECT Stock FROM Articulos WHERE Id_Articulo = @idele";
                            command.Parameters.AddWithValue("@idele", idArt);
                            conex.Open();
                            using (var reader = command.ExecuteReader())
                            {

                                if (reader.Read())
                                {
                                    stock = reader.GetInt32(reader.GetOrdinal("Stock"));
                                    //MessageBox.Show(stock.ToString());
                                }
                                else Mensajes.Mostrar("ERROR", Mensajes.Tipo.Error);
                            }
                        }
                        String nombreCli = "";
                        using (SqlConnection conex = new SqlConnection(MetodosGestion.db))
                        using (SqlCommand command = conex.CreateCommand())
                        {
                            command.CommandText = "SELECT Nombre FROM Clientes WHERE Id_Cliente = @idcli";
                            command.Parameters.AddWithValue("@idcli", idCliente);
                            conex.Open();
                            using (var reader = command.ExecuteReader())
                            {

                                if (reader.Read())
                                {
                                    nombreCli = reader.GetString(reader.GetOrdinal("Nombre"));
                                    //MessageBox.Show(nombreCli);
                                }
                                else Mensajes.Mostrar("ERROR", Mensajes.Tipo.Error);
                            }
                        }
                        using (SqlConnection conection = new SqlConnection(MetodosGestion.db))
                        using (SqlCommand command = conection.CreateCommand())
                        {
                            command.CommandText = "INSERT INTO Movimientos OUTPUT INSERTED.Id_Movimiento VALUES(@fecha, @idarticulo, @origen, " +
                                "@nomprov, @cantidad, @stock)";

                            command.Parameters.AddWithValue("@fecha", DateTime.Today.ToString("dd/MM/yyyy"));
                            command.Parameters.AddWithValue("@idarticulo", idArt);
                            command.Parameters.AddWithValue("@origen", "Cliente");
                            command.Parameters.AddWithValue("@nomprov", nombreCli);
                            command.Parameters.AddWithValue("@cantidad", row[5]);
                            command.Parameters.AddWithValue("@stock", stock);

                            conection.Open();
                            int a = command.ExecuteNonQuery();

                            if (a != 0)
                            {
                                conection.Close();
                            }
                            else
                            {
                                Mensajes.Mostrar("ERROR", Mensajes.Tipo.Error);
                            }
                        }
                    }


                    dgFinal.Columns.Clear();
                    dgFinal.ItemsSource = null;
                    dgFinal.Items.Refresh();

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                guardarCantidad = 0;
                lbTotalFin.Content = 0;
                totalFinal = 0;
                dataT.Clear();
            }
        }

        ClientesEdicion c;
        private void bAddClientes_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(c))
            {
                c = new ClientesEdicion(0);
                c.Title = "Añadir Cliente";
                c.Owner = System.Windows.Application.Current.MainWindow;
                c.Show();
            }
            nombreCombo();
            cbCliente.Items.Refresh();

        }

        private void cbCliente_DropDownClosed(object sender, EventArgs e)
        {
            nombreCombo();
        }

        private void cbCliente_DropDownOpened(object sender, EventArgs e)
        {

        }

        private void filtarNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)DatosAnadir.ItemsSource).ToTable();
                dt.TableName = "Articulos";
                view.Table = dt;
            }

            view.RowFilter = $"Nombre LIKE '%{filtarNom.Text}%'";

            dt = view.ToTable();
            DatosAnadir.ItemsSource = null;
            DatosAnadir.ItemsSource = dt.DefaultView;
        }


        DataView view = null;
        DataTable dt;

    }
}
