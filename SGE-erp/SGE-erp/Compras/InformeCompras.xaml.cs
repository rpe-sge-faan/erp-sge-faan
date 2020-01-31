using Microsoft.Win32;
using SGE_erp.Compras;
using SGE_erp.Gestion;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace SGE_erp.Compras
{
    /// <summary>
    /// Lógica de interacción para InformeCompras.xaml
    /// </summary>
    public partial class InformeCompras : Window
    {
        public InformeCompras()
        {
            InitializeComponent();
            cargarlistado();
        }

        public void cargarlistado()
        {
            DataTable dt;
            double importeTotal = 0;
            double baseTotal = 0;
            double cuotaIvaTotal = 0;

            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter("select Compra.id_compra as 'Id Compra', compra.fechaCompra as 'Fecha Compra',Proveedores.Nombre as 'Nombre Proveedor', Proveedores.NIF, " +
                    "round(cast(Sum(CompraArticulos.Cantidad * (ProveedorArticulo.PrecioCompra / ( 1 + (convert(decimal,iva.Porcentaje_Iva)/100))))AS DECIMAL (8,2)),2) as 'Base Imponible', " +
                    "Sum(compraArticulos.Cantidad * ProveedorArticulo.PrecioCompra) - Sum(CompraArticulos.Cantidad * (ProveedorArticulo.PrecioCompra /" +
                    " ( 1 + (convert(decimal,iva.Porcentaje_Iva)/100)))) as 'Cuota Iva', " +
                    "Sum(compraArticulos.Cantidad * ProveedorArticulo.PrecioCompra) as 'Importe Total' " +
                    "from Compra " +
                    "inner join compraArticulos on Compra.Id_compra = CompraArticulos.Id_Compra " + 
                    "inner join ProveedorArticulo on CompraArticulos.id_Elemento = ProveedorArticulo.id_Elemento " +
                    "inner join Articulos on articulos.id_Articulo = ProveedorArticulo.id_articulo " +
                    "inner join iva on iva.id_iva = Articulos.Id_iva " +
                    "inner join Proveedores on Compra.id_Proveedor = Proveedores.id_proveedor " + ComprasVisualizar.sentenciaWhere + 
                    " group by Compra.id_compra, compra.fechaCompra,Proveedores.Nombre, Proveedores.NIF " + 
                    ComprasVisualizar.sentenciaOrder, con)){
                dt = new DataTable();
                da.Fill(dt);                

                comprasDataGrid.ItemsSource = dt.DefaultView;
                
            }

            this.lb_tituloFechas.Content = "Informe compras \n" + Compras_FiltroCompra.fechaDesde + " - " + Compras_FiltroCompra.fechaHasta;

            for(int i=0; i < dt.Rows.Count; i++)
            {
                DataRow drow = dt.Rows[i];
                importeTotal += Convert.ToDouble(drow["Importe Total"]);
                baseTotal += Convert.ToDouble(drow["Base Imponible"]);
                cuotaIvaTotal += Convert.ToDouble(drow["Cuota Iva"]);
            }

            tbBase.Text = baseTotal.ToString() + "€";
            tbCuota.Text = cuotaIvaTotal.ToString() + "€";
            tbTotal.Text = importeTotal.ToString() + "€";
        }
        private void BImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "informe");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        string rutaPdf = "";
        string variable = "prueba";

        private void BDescargar_Click(object sender, RoutedEventArgs e)
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
                String f = String.Format("{0:yyyyMMdd}", DateTime.Now);

                if (guardar)
                {
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        FileName = $"InformeCompra"+Compras_FiltroCompra.fechaDesde.Replace("/","_")+"-"+Compras_FiltroCompra.fechaHasta.Replace("/", "_"),
                        Filter = "Text Files(*.pdf)|*.pdf|All(*.*)|*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, dialog.FileName, 0);
                        rutaPdf = (dialog.FileName);
                        System.Diagnostics.Process.Start(rutaPdf);
                    }


                }
                else
                {
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Informe{variable}{f}");
                    PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, path, 0);
                    rutaPdf = path;
                }
            }
        }
    }
}
