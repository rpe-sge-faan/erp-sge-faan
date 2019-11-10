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

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Lógica de interacción para ClientesEdicion.xaml
    /// </summary>
    public partial class ClientesEdicion : Window
    {
        public ClientesEdicion()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            SGE_erp.SetaData setaData = ((SGE_erp.SetaData)(this.FindResource("setaData")));
            // Cargar datos en la tabla Clientes. Puede modificar este código según sea necesario.
            SGE_erp.SetaDataTableAdapters.ClientesTableAdapter setaDataClientesTableAdapter = new SGE_erp.SetaDataTableAdapters.ClientesTableAdapter();
            setaDataClientesTableAdapter.Fill(setaData.Clientes);
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            clientesViewSource.View.MoveCurrentToFirst();
        }
    }
}
