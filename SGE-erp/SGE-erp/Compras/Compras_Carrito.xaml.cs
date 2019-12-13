using System;
using System.Collections.Generic;
using System.Data;
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

namespace SGE_erp.Compras
{
    /// <summary>
    /// Lógica de interacción para Compras_Carrito.xaml
    /// </summary>
    public partial class Compras_Carrito : Window
    {
        public int precioFinal;
        public Compras_Carrito()
        {
            InitializeComponent();
            this.carrito.ItemsSource = ComprasAnadir.carritoCompra.DefaultView;
            
            //this.carrito.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void BtnBorrarArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (carrito.SelectedItem != null)
            {
                if (MessageBox.Show("¿Borrar seleccionado?", "Confirm delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ComprasAnadir.carritoCompra.Rows.RemoveAt(carrito.SelectedIndex);
                }
            }                          
        }

        private void BtnVaciarCarro_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("¿Vaciar todo?", "Confirm delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int j = ComprasAnadir.carritoCompra.Rows.Count;
                for (int i = 0; i < j; i++)
                {
                    ComprasAnadir.carritoCompra.Rows.RemoveAt(0);
                }
                //this.carrito.ItemsSource = ComprasAnadir.carritoCompra.DefaultView;
            }            
        }

        private void BtnFinalizarCompra_Click(object sender, RoutedEventArgs e)
        {
            ComprasAnadir.guardarCompra();
            ComprasVisualizar cv = new ComprasVisualizar();
            cv.cargarDatos();
        }

        public void calPrecioFinal()
        {
            for (int i = 0; i < ComprasAnadir.carritoCompra.Rows.Count; i++)
            {
                DataRow row = ComprasAnadir.carritoCompra.Rows[i];
                precioFinal += Convert.ToInt32(row["Precio Total"]);
            }
        }
        
    }
}

