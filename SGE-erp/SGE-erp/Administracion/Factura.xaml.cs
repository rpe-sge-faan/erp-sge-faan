using SGE_erp.Gestion;
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
            gridCol1.Width = new GridLength(2.4, GridUnitType.Star);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(0.7, GridUnitType.Star);
            ColumnDefinition gridCol3 = new ColumnDefinition();
            gridCol3.Width = new GridLength(0.5, GridUnitType.Star);
            ColumnDefinition gridCol4 = new ColumnDefinition();
            gridCol4.Width = new GridLength(0.8, GridUnitType.Star);
            ColumnDefinition gridCol5 = new ColumnDefinition();
            gridCol5.Width = new GridLength(0.5, GridUnitType.Star);
            ColumnDefinition gridCol6 = new ColumnDefinition();
            gridCol6.Width = new GridLength(1, GridUnitType.Star);

            gr.ColumnDefinitions.Add(gridCol1);
            gr.ColumnDefinitions.Add(gridCol2);
            gr.ColumnDefinitions.Add(gridCol3);
            gr.ColumnDefinitions.Add(gridCol4);
            gr.ColumnDefinitions.Add(gridCol5);
            gr.ColumnDefinitions.Add(gridCol6);

            // articulo -  sin iva - cantidad - subtotal - IVA - total

            TextBlock tb1 = new TextBlock();
            tb1.Text = args[0];
            TextBlock tb2 = new TextBlock();
            tb2.Text = $"{args[1]}€";
            tb2.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb3 = new TextBlock();
            tb3.Text = $"{args[2]}";
            tb3.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb4 = new TextBlock();
            tb4.Text = $"{args[3]}€";
            tb4.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb5 = new TextBlock();
            tb5.Text = $"{args[4]}%";
            tb5.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb6 = new TextBlock();
            tb6.Text = $"{args[5]}€";
            tb6.HorizontalAlignment = HorizontalAlignment.Center;

            gr.Children.Add(tb1);
            gr.Children.Add(tb2);
            gr.Children.Add(tb3);
            gr.Children.Add(tb4);
            gr.Children.Add(tb5);
            gr.Children.Add(tb6);

            Grid.SetColumn(tb1, 0);
            Grid.SetColumn(tb2, 1);
            Grid.SetColumn(tb3, 2);
            Grid.SetColumn(tb4, 3);
            Grid.SetColumn(tb5, 4);
            Grid.SetColumn(tb6, 5);

            listaArticulos.Items.Add(gr);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
                //string variable;
                //string bd = MetodosGestion.db;
                //using (SqlConnection con = new SqlConnection(bd))
                //using (SqlCommand command = con.CreateCommand())
                //{
                //    command.CommandText = "SELECT Compra.FechaCompra, Compra.PrecioTotal, CompraArticulos.Cantidad,  " +
                //        "Proveedores.Nombre, Proveedores.Direccion, Empleados.Nombre," +
                //        "Poblaciones.CodPostal, Poblaciones.Poblacion, Poblaciones.Provincia, " +
                //        "Articulos.Nombre, Articulos.Descripcion, ProveedorArticulo.PrecioCompra " +
                //        "FROM Compra, CompraArticulos, Proveedores, Poblaciones, ProveedorArticulo, Articulos, Empleados " +
                //        "WHERE Compra.Id_Compra = CompraArticulos.Id_Compra AND Compra.Id_Proveedor = Proveedores.Id_Proveedor AND Compra.Id_Empleado = Empleados.Id_Empleado AND " +
                //        "CompraArticulos.Id_Elemento = ProveedorArticulo.Id_Elemento AND ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo " +
                //        "AND Compra.Id_Compra = @id";
                //    command.Parameters.AddWithValue("@id", id);
                //    con.Open();

                //    using (var reader = command.ExecuteReader())
                //    {
                //        if (reader.Read())
                //        {
                //            DateTime dd = reader.GetDateTime(reader.GetOrdinal("FechaCompra"));
                //            fecha.Text = String.Format("{0:dddd, d MMMM, yyyy}", dd);
                //            tbTotal.Text = (reader.GetDecimal(reader.GetOrdinal("PrecioTotal"))).ToString() + "€";

                //        }
                //    }
                //}
            }
            else
            {
                decimal totalSin = 0;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT VentasArticulos.Id_Elemento, Articulos.Nombre as Articulo, Ventas.FechaVentas, Ventas.PrecioTotal, VentasArticulos.Cantidad,  " +
                            "Clientes.Nombre as Cliente, Clientes.Direccion, Empleados.Nombre as Empleado," +
                            "Poblaciones.CodPostal, Poblaciones.Poblacion, Poblaciones.Provincia, " +
                            "Articulos.Descripcion, Articulos.PVP,  Iva.Porcentaje_Iva " +
                        "FROM Ventas, VentasArticulos, Clientes, Poblaciones, ProveedorArticulo, Articulos, Empleados, Iva " +
                        "WHERE Ventas.Id_Ventas = VentasArticulos.Id_Ventas AND " +
                            "Ventas.Id_Empleado = Empleados.Id_Empleado AND " +
                            "Ventas.Id_Cliente = Clientes.Id_Cliente AND " +
                            "Poblaciones.CodPostal = Clientes.CodPostal AND " +
                            "Articulos.Id_Iva = Iva.Id_Iva AND " +
                            "VentasArticulos.Id_Elemento = ProveedorArticulo.Id_Elemento AND " +
                            "ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo " +
                        "AND Ventas.Id_Ventas = @id ";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();
                    Boolean primera = false;
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // articulo -  sin iva - cantidad - subtotal - IVA - total
                            if (!primera)
                            {
                                DateTime dd = reader.GetDateTime(reader.GetOrdinal("FechaVentas"));
                                fecha.Text = String.Format("{0:dddd, d MMMM, yyyy}", dd);
                                tbTotal.Text = (reader.GetDecimal(reader.GetOrdinal("PrecioTotal"))).ToString() + "€";
                                nombre.Text = reader.GetString(reader.GetOrdinal("Cliente"));
                                direccion.Text = reader.GetString(reader.GetOrdinal("Direccion"));
                                poblacion.Text = $"{reader.GetString(reader.GetOrdinal("CodPostal"))} {reader.GetString(reader.GetOrdinal("Poblacion"))}, {reader.GetString(reader.GetOrdinal("Provincia"))}";
                                empleado.Text = (reader.GetString(reader.GetOrdinal("Empleado"))).Trim();
                                primera = true;
                            }
                            string articulo = $"{reader.GetString(reader.GetOrdinal("Articulo"))} - {reader.GetString(reader.GetOrdinal("Descripcion"))}";
                            int iva = reader.GetInt32(reader.GetOrdinal("Porcentaje_Iva"));
                            int cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad"));
                            decimal precio = reader.GetDecimal(reader.GetOrdinal("PVP"));
                            decimal iva2 = iva;
                            decimal cant = cantidad;
                            decimal dividir = iva2 / 100;
                            decimal precioSin = precio - (precio * dividir);
                            decimal subtotal = precioSin * cant;
                            decimal total = precio * cant;
                            totalSin += subtotal;

                            addToTable(articulo, String.Format("{0:0.00}", precioSin), cantidad.ToString(), String.Format("{0:0.00}", subtotal), iva.ToString(), String.Format("{0:0.00}", total));
                        }
                        tbTotalSin.Text = String.Format("{0:0.00}€", totalSin); 
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
