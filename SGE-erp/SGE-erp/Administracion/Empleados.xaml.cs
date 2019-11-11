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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGE_erp.Administracion
{
    /// <summary>
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : UserControl
    {
        public Empleados()
        {
            InitializeComponent();
        }

        private void anadir_Click(object sender, RoutedEventArgs e)
        {
            EmpleadosAñadir añadir = new EmpleadosAñadir();
            añadir.Show();
        }

        private void editar_Click(object sender, RoutedEventArgs e)
        {
            EmpleadosEditar editar = new EmpleadosEditar();
            editar.Show();
        }

        private void borrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("¿Desea eliminar a este empleado?");
        }
    }
}
