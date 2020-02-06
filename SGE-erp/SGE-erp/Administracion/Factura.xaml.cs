using System.IO;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using SGE_erp.Gestion;
using System.Data.SqlClient;
using System.Data;
using System;
using Microsoft.Win32;
using System.Net.Mail;
using System.Net;
using System.Windows.Navigation;
using System.Diagnostics;

namespace SGE_erp.Administracion
{
    /// <summary>
    /// Interaction logic for Factura.xaml
    /// </summary>
    public partial class Factura : Window
    {
        readonly int id;
        readonly int tipo; // 1 VENTAS 2 COMPRAS
        public Factura(int id, int tipo)
        {
            InitializeComponent();
            this.id = id;
            this.tipo = tipo;
        }

        private void AddToTable(params string[] args)
        {
            Grid gr = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(2),
                Width = 480
            };

            ColumnDefinition gridCol1 = new ColumnDefinition
            {
                Width = new GridLength(2.4, GridUnitType.Star)
            };
            ColumnDefinition gridCol2 = new ColumnDefinition
            {
                Width = new GridLength(0.7, GridUnitType.Star)
            };
            ColumnDefinition gridCol3 = new ColumnDefinition
            {
                Width = new GridLength(0.5, GridUnitType.Star)
            };
            ColumnDefinition gridCol4 = new ColumnDefinition
            {
                Width = new GridLength(0.8, GridUnitType.Star)
            };
            ColumnDefinition gridCol5 = new ColumnDefinition
            {
                Width = new GridLength(0.5, GridUnitType.Star)
            };
            ColumnDefinition gridCol6 = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };

            gr.ColumnDefinitions.Add(gridCol1);
            gr.ColumnDefinitions.Add(gridCol2);
            gr.ColumnDefinitions.Add(gridCol3);
            gr.ColumnDefinitions.Add(gridCol4);
            gr.ColumnDefinitions.Add(gridCol5);
            gr.ColumnDefinitions.Add(gridCol6);

            // articulo -  sin iva - cantidad - subtotal - IVA - total

            TextBlock tb1 = new TextBlock
            {
                Text = args[0]
            };
            TextBlock tb2 = new TextBlock
            {
                Text = $"{args[1]}€",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            TextBlock tb3 = new TextBlock
            {
                Text = $"{args[2]}",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            TextBlock tb4 = new TextBlock
            {
                Text = $"{args[3]}€",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            TextBlock tb5 = new TextBlock
            {
                Text = $"{args[4]}%",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            TextBlock tb6 = new TextBlock
            {
                Text = $"{args[5]}€",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 10, 0)

            };

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

        private void CargarFactura()
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
                String dniT = "";
                string bd = MetodosGestion.db;
                int nFactura = 0;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT Ventas.Id_Ventas, VentasArticulos.Id_Elemento, Articulos.Nombre as Articulo, Ventas.FechaVentas, Ventas.PrecioTotal, VentasArticulos.Cantidad,  " +
                            "Clientes.Nombre as Cliente, Clientes.NIF, Clientes.Direccion, Empleados.Nombre as Empleado," +
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
                                tbTotal.Text = String.Format("{0:n}", reader.GetDecimal(reader.GetOrdinal("PrecioTotal"))) + "€";
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
                            //decimal subtotal = precioSin * cant;
                            decimal total = precio * cant;
                            decimal subtotal = total / (1 + dividir);
                            totalSin += subtotal;
                            dniT = reader.GetString(reader.GetOrdinal("NIF"));
                            nFactura = reader.GetInt32(reader.GetOrdinal("Id_Ventas"));
                            AddToTable(articulo, String.Format("{0:n}", precio), cantidad.ToString(), String.Format("{0:n}", subtotal), iva.ToString(), String.Format("{0:n}", total));
                        }
                        tbTotalSin.Text = String.Format("{0:n}€", totalSin);
                        dni.Text = dniT;
                        numFactura.Text = $"Nº factura: {nFactura.ToString()}";

                    }
                }
            }
        }


        private void VentanaFacturas_Loaded(object sender, RoutedEventArgs e)
        {
            CargarFactura();
        }

        string rutaPdf = "";
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ToPdf(true);
        }

        private void ToPdf(Boolean guardar)
        {
            MemoryStream lMemoryStream = new MemoryStream();
            Package package = Package.Open(lMemoryStream, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            writer.Write(print);
            doc.Close();
            package.Close();
            using (var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream))
            {
                DateTime parsedDate = DateTime.Parse(fecha.Text);
                String f = String.Format("{0:yyyyMMdd}", parsedDate);

                if (guardar)
                {
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        FileName = $"Factura{nombre.Text}{f}",
                        Filter = "Text Files(*.pdf)|*.pdf|All(*.*)|*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, dialog.FileName, 0);
                        rutaPdf = dialog.FileName;
                        System.Diagnostics.Process.Start(rutaPdf);
                    }

                    rutaPdf = dialog.FileName;

                }
                else
                {
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Factura{f}");
                    PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, path, 0);
                    rutaPdf = path;
                }

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ToPdf(false);
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("faan.erp@gmail.com");
                mail.To.Add("andrea.lobo93@gmail.com");
                mail.Subject = "Factura - FAAN";
                mail.Body = "Le adjuntamos la factura de su compra. Gracias por usar nuestros servicios.";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(rutaPdf);
                mail.Attachments.Add(attachment);

                using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587))
                {
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("faan.erp@gmail.com", "2FeArApN");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                Mensajes.Mostrar("Email enviado", Mensajes.Tipo.Info);
            }
            File.Delete(rutaPdf);
        }
    }
}
