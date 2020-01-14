﻿using System;
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
                SqlDataAdapter da = new SqlDataAdapter("SELECT Articulos.Id_Articulo, ProveedorArticulo.Id_Elemento, Nombre, PVP, Stock " +
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
            int stock = dd.Row.Field<int>("Stock");
            udStock.maxvalue = stock;
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
                DataRowView drv = (DataRowView)DatosAnadir.SelectedItem;
                int idArticulo = drv.Row.Field<int>("Id_Articulo");
                int idEmpleado = MainWindow.idEmpleado;
                int idElemento = drv.Row.Field<int>("Id_Elemento");
                int stock = (int)udStock.Value;
                String nombre = drv.Row.Field<String>("Nombre");
                decimal pvp = drv.Row.Field<decimal>("PVP");
                decimal totalM = 0;
                totalM = pvp * stock;

                totalFinal += totalM;
                lbTotalFin.Content = $"{totalFinal}€";

                guardarCantidad += stock;

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
        }

        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            if (dgFinal.Items.Count != 0)
            {
                DataRowView drv = (DataRowView)DatosAnadir.SelectedItem;

                int idElemento = drv.Row.Field<int>("Id_Elemento");
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
                            MessageBox.Show("Vendido");
                            conn.Close();
                        }
                        else
                        {
                            MessageBox.Show("ERROR");
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
                                MessageBox.Show("ERROR");
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


        private void cbFormaPago_Loaded(object sender, RoutedEventArgs e)
        {

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
    }
}
