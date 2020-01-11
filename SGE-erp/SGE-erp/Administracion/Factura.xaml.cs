using SGE_erp.Gestion;
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

namespace SGE_erp.Administracion
{
    /// <summary>
    /// Interaction logic for Factura.xaml
    /// </summary>
    public partial class Factura : Window
    {
        int id;
        int tipo; // 1 VENTAS 2 COMPRAS
        public Factura(int id, int tipo)
        {
            InitializeComponent();
            this.id = id;
            this.tipo = tipo;
        }

        private void addToTable(params string[] args)
        {
            Grid gr = new Grid();
            gr.HorizontalAlignment = HorizontalAlignment.Stretch;
            gr.Margin = new Thickness(2);
            gr.Width = 480;

            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(2, GridUnitType.Star);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol3 = new ColumnDefinition();
            gridCol3.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol4 = new ColumnDefinition();
            gridCol4.Width = new GridLength(1, GridUnitType.Star);

            gr.ColumnDefinitions.Add(gridCol1);
            gr.ColumnDefinitions.Add(gridCol2);
            gr.ColumnDefinitions.Add(gridCol3);
            gr.ColumnDefinitions.Add(gridCol4);

            TextBlock tb1 = new TextBlock();
            tb1.Text = args[0];     // <------------
            TextBlock tb2 = new TextBlock();
            tb2.Text = args[1];       // <------------
            tb2.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb3 = new TextBlock();
            tb3.Text = args[2];       // <------------
            tb3.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb4 = new TextBlock();
            tb4.Text = args[3];      // <------------
            tb4.HorizontalAlignment = HorizontalAlignment.Center;
            //tb4.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF62013C"));

            //tb1.SetValue(Grid.ColumnProperty, 0);
            //tb2.SetValue(Grid.ColumnProperty, 1);
            //tb3.SetValue(Grid.ColumnProperty, 2);
            //tb4.SetValue(Grid.ColumnProperty, 3);
            gr.Children.Add(tb1);
            gr.Children.Add(tb2);
            gr.Children.Add(tb3);
            gr.Children.Add(tb4);

            Grid.SetColumn(tb1, 0);
            Grid.SetColumn(tb2, 1);
            Grid.SetColumn(tb3, 2);
            Grid.SetColumn(tb4, 3);



            listaArticulos.Items.Add(gr);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addToTable("1111", "2222", "33333", "44444");

            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void cargarFactura()
        {
            if (tipo == 2)
            {
                string variable;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT Compra.FechaCompra, Compra.PrecioTotal, CompraArticulos.Cantidad,  " +
                        "Proveedores.Nombre, Proveedores.Direccion, Empleados.Nombre," +
                        "Poblaciones.CodPostal, Poblaciones.Poblacion, Poblaciones.Provincia, " +
                        "Articulos.Nombre, Articulos.Descripcion, ProveedorArticulo.PrecioCompra " +
                        "FROM Compra, CompraArticulos, Proveedores, Poblaciones, ProveedorArticulo, Articulos, Empleados " +
                        "WHERE Compra.Id_Compra = CompraArticulos.Id_Compra AND Compra.Id_Proveedor = Proveedores.Id_Proveedor AND Compra.Id_Empleado = Empleados.Id_Empleado AND " +
                        "CompraArticulos.Id_Elemento = ProveedorArticulo.Id_Elemento AND ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo " +
                        "AND Compra.Id_Compra = @id";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime dd = reader.GetDateTime(reader.GetOrdinal("FechaCompra"));
                            fecha.Text = String.Format("{0:dddd, d MMMM, yyyy}", dd);
                            total.Text = (reader.GetDecimal(reader.GetOrdinal("PrecioTotal"))).ToString() + "€";

                        }
                    }
                }
            }
            else
            {
                string variable;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT Ventas.FechaVentas, Ventas.PrecioTotal, VentasArticulos.Cantidad,  " +
                        "Clientes.Nombre as Cliente, Clientes.Direccion, Empleados.Nombre as Empleado," +
                        "Poblaciones.CodPostal, Poblaciones.Poblacion, Poblaciones.Provincia, " +
                        "Articulos.Nombre as Articulo, Articulos.Descripcion, Articulos.PVP " +
                        "FROM Ventas, VentasArticulos, Clientes, Poblaciones, ProveedorArticulo, Articulos, Empleados " +
                        "WHERE Ventas.Id_Ventas = VentasArticulos.Id_Ventas AND Ventas.Id_Empleado = Empleados.Id_Empleado AND " +
                        "Ventas.Id_Cliente = Clientes.Id_Cliente AND " +
                        "VentasArticulos.Id_Elemento = ProveedorArticulo.Id_Elemento AND ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo " +
                        "AND Ventas.Id_Ventas = @id";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();
                    Boolean primera = false;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!primera)
                            {
                                DateTime dd = reader.GetDateTime(reader.GetOrdinal("FechaVentas"));
                                fecha.Text = String.Format("{0:dddd, d MMMM, yyyy}", dd);
                                total.Text = (reader.GetDecimal(reader.GetOrdinal("PrecioTotal"))).ToString() + "€";
                                nombre.Text = reader.GetString(reader.GetOrdinal("Cliente"));
                                direccion.Text = reader.GetString(reader.GetOrdinal("Direccion"));
                                poblacion.Text = $"{reader.GetString(reader.GetOrdinal("CodPostal"))} {reader.GetString(reader.GetOrdinal("Poblacion"))}, {reader.GetString(reader.GetOrdinal("Provincia"))}";
                                empleado.Text = (reader.GetString(reader.GetOrdinal("Empleado"))).Trim();
                                primera = true;
                            }
                            string articulo = reader.GetString(reader.GetOrdinal("Articulo"));
                            string cantidad = (reader.GetInt32(reader.GetOrdinal("Cantidad"))).ToString();
                            string precio = (reader.GetDecimal(reader.GetOrdinal("PVP"))).ToString();
                            //addToTable();
                        }
                    }
                }
            }
        }

        private void ventanaFacturas_Loaded(object sender, RoutedEventArgs e)
        {
            cargarFactura();
        }
    }
}
