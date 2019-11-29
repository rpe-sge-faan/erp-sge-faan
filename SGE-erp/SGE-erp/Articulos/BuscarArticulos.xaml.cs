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
using System.Windows.Shapes;

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Interaction logic for BuscarArticulos.xaml
    /// </summary>
    public partial class BuscarArticulos : Window
    {
        public BuscarArticulos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           /* SGE_erp.SetaData setaData = ((SGE_erp.SetaData)(this.FindResource("setaData")));
            // Load data into the table Articulos. You can modify this code as needed.
            SGE_erp.SetaDataTableAdapters.ArticulosTableAdapter setaDataArticulosTableAdapter = new SGE_erp.SetaDataTableAdapters.ArticulosTableAdapter();
            setaDataArticulosTableAdapter.Fill(setaData.Articulos);
            System.Windows.Data.CollectionViewSource articulosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("articulosViewSource")));
            articulosViewSource.View.MoveCurrentToFirst();*/

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
           /* if (id == 0)
            {
                nuevo();
            }
            else
            {
                editar();
            }
*/        }
    }
}
