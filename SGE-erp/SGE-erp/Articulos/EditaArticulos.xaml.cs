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
    /// Lógica de interacción para EditaArticulos.xaml
    /// </summary>
    public partial class EditaArticulos : Window
    {
        public EditaArticulos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            SGE_erp.SetaData setaData = ((SGE_erp.SetaData)(this.FindResource("setaData")));
            // Cargar datos en la tabla Articulos. Puede modificar este código según sea necesario.
            SGE_erp.SetaDataTableAdapters.ArticulosTableAdapter setaDataArticulosTableAdapter = new SGE_erp.SetaDataTableAdapters.ArticulosTableAdapter();
            setaDataArticulosTableAdapter.Fill(setaData.Articulos);
            System.Windows.Data.CollectionViewSource articulosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("articulosViewSource")));
            articulosViewSource.View.MoveCurrentToFirst();
        }
    }
}
