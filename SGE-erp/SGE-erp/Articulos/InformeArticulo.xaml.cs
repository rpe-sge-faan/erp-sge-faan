using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

namespace SGE_erp.Articulos
{
    public partial class InformeArticulo : Window
    {

        string variable;
        public InformeArticulo(String idArt)
        {
            InitializeComponent();
            DataContext = this;
            variable = "prueba";
            Actualizar(idArt);
        }


        private void Actualizar(string idArticulo)
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Movimientos WHERE Id_Articulo=" + idArticulo + " order by Fecha, Id_Movimiento", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                dt.Columns[0].ColumnName = "Mov";
                dt.Columns[2].ColumnName = "Id";

                this.articuloDataGrid.ItemsSource = dt.DefaultView;
            }

            con.Open();

            using (SqlCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT Nombre, Descripcion FROM Articulos WHERE Id_Articulo=@id";
                command.Parameters.AddWithValue("@id", idArticulo);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        variable = reader.GetString(reader.GetOrdinal("Nombre"));
                        variable += " " + reader.GetString(reader.GetOrdinal("Descripcion"));
                    }
                }
                con.Close();
            }
        }

        public string NArticulo
        {
            get { return variable; }
            set
            {
                variable = value;
            }
        }

        String f = String.Format("{0:dddd, d MMMM, yyyy}", DateTime.Now);
        public string Fecha
        {
            get { return f; }
            set
            {
                f = value;
            }
        }

        string rutaPdf = "";

        private void BImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
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
                        FileName = $"Informe{variable.Replace(" ", "")}{f}",
                        Filter = "Text Files(*.pdf)|*.pdf|All(*.*)|*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, dialog.FileName, 0);
                    }

                    rutaPdf = (dialog.FileName);
                    System.Diagnostics.Process.Start(rutaPdf);
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
