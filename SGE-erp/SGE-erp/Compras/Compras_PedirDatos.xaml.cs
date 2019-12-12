using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para Compras_PedirDatos.xaml
    /// </summary>
    public partial class Compras_PedirDatos : Window
    {
        public static int cantidadRecogida = 0;
        public Compras_PedirDatos()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            cantidadRecogida = int.Parse(txtCantidad.Text.ToString());
            this.Close();
        }
    }
}
