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

namespace SGE_erp.Venta
{
    /// <summary>
    /// Lógica de interacción para VentasEdicion.xaml
    /// </summary>
    public partial class VentasEdicion : Window
    {

        public int id;
        public Delegate ActualizarLista;
        public Delegate FiltrarLista;

        public delegate void RefreshList();
        public VentasEdicion(int num)
        {
            InitializeComponent();
            this.id = num;
            tbFechaPos.SelectedDate = DateTime.Today;
            tbFechaAnt.SelectedDate = DateTime.Today;
            this.tbPrecioTotal.Text = "0";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (id == -1)
            {
                FiltrarLista.DynamicInvoke();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
