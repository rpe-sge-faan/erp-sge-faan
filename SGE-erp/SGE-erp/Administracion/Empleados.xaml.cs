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
using SGE_erp.Gestion;

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

        EmpleadosEditar ed = null;

        private void anadir_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(ed))
            {
                ed = new EmpleadosEditar();
                ed.Owner = System.Windows.Application.Current.MainWindow;
                ed.Show();
            }
        }

        
        private void editar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(ed))
            {
                ed = new EmpleadosEditar();
                ed.Owner = System.Windows.Application.Current.MainWindow;
                ed.Show();
            }
            
        }

        private void borrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("¿Desea eliminar a este empleado?");
        }

        EmpleadosEditar eE = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // No cargue datos en tiempo de diseño.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Cargue los datos aquí y asigne el resultado a CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void anadirEmpl_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(eE))
            {
                eE = new EmpleadosEditar();
                eE.Owner = System.Windows.Application.Current.MainWindow;
                eE.Show();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(eE))
            {
                eE = new EmpleadosEditar();
                eE.Owner = System.Windows.Application.Current.MainWindow;
                eE.Show();
            }
        }
    }
}
