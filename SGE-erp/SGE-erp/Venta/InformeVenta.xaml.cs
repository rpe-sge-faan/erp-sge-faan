

using Microsoft.Win32;
using SGE_erp.Gestion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace SGE_erp.Venta
{
    /// <summary>
    /// Lógica de interacción para InformeVenta.xaml
    /// </summary>
    public partial class InformeVenta : Window
    {
        int[] ids;
        public InformeVenta(int[] ids2)
        {
            InitializeComponent();
            this.ids = ids2;
            Actualizar();
        }

        string rutaPdf = "";

        private void ToPdf()
        {
            MemoryStream lMemoryStream = new MemoryStream();
            Package package = Package.Open(lMemoryStream, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            writer.Write(print);
            doc.Close();
            package.Close();
            var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);

            String f = String.Format("{0:yyyyMMdd}", DateTime.Now);

            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = $"InformeVentas{f}",
                Filter = "Text Files(*.pdf)|*.pdf|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, dialog.FileName, 0);
            }

            rutaPdf = dialog.FileName;
        }

        private void Actualizar()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            DataTable dt = new DataTable();

            for (int i = 0; i < ids.Length; i++)
            {


                SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Ventas.Id_Ventas AS Id, CONVERT(VARCHAR(10), [FechaVentas], 103) AS Fecha, Clientes.Nombre AS Cliente, Clientes.NIF, " +
                                                                    "CAST((PrecioTotal-((PrecioTotal*Porcentaje_Iva)/100)) AS DECIMAL(7, 2)) AS Base, " +
                                                                    "(PrecioTotal - (CAST((PrecioTotal-((PrecioTotal*Porcentaje_Iva)/100)) AS DECIMAL(7, 2)))) AS Cuota, " +
                                                                    "Ventas.PrecioTotal AS Total " +
                                                                    "FROM Ventas, VentasArticulos, Clientes, ProveedorArticulo, Articulos, Iva " +
                                                                    "WHERE Ventas.Id_Cliente = Clientes.Id_Cliente AND Ventas.Id_Ventas = VentasArticulos.Id_Ventas AND " +
                                                                    "Articulos.Id_Iva = Iva.Id_Iva AND " +
                                                                    "VentasArticulos.Id_Elemento = ProveedorArticulo.Id_Elemento AND " +
                                                                     $"ProveedorArticulo.Id_Articulo = Articulos.Id_Articulo AND Ventas.Id_Ventas = {ids[i]}", con);

                da.Fill(dt);

            }

            double baseV = 0;
            double cuota = 0;
            double total = 0;
            this.ventasDataGrid.ItemsSource = dt.DefaultView;
            con.Open();
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                baseV += double.Parse(dr[4].ToString());
                cuota += double.Parse(dr[5].ToString());
                total += double.Parse(dr[6].ToString());
            }

            tbBase.Text = $"{baseV.ToString()} €";
            tbCuota.Text = $"{cuota.ToString()} €";
            tbTotal.Text = $"{total.ToString()} €";
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "ventas");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void Descargar_Click(object sender, RoutedEventArgs e)
        {
            ToPdf();
        }
    }
}



//SqlDataAdapter da = new SqlDataAdapter("SELECT Ventas.Id_Ventas, CONVERT(VARCHAR(10), [FechaVentas], 103) AS Fecha, Clientes.Nombre, Clientes.NIF, " +
//                                                    "CAST((PrecioTotal-((PrecioTotal*Porcentaje_Iva)/100)) AS DECIMAL(7, 2)) AS Base, " +
//                                                    "(PrecioTotal - (CAST((PrecioTotal-((PrecioTotal*Porcentaje_Iva)/100)) AS DECIMAL(7, 2)))) AS Cuota, Ventas.PrecioTotal AS Total " +
//                                                    "FROM Ventas, Clientes, Iva " +
//                                                    "WHERE Ventas.Id_Cliente = Clientes.Id_Cliente ", con);
