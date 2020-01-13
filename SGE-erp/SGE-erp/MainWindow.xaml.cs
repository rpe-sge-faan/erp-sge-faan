using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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


namespace SGE_erp
{

    public partial class MainWindow : Window
    {
        public static bool acceso;
        public static string userEmpleado;
        public static string nombreEmpleado;
        public static int idEmpleado = 1;
        private bool idUser = false;

        public MainWindow()
        {

            InitializeComponent();
            //LogInWindow liw = new LogInWindow();
            //liw.ShowDialog();
            //if (acceso == false)
            //{
            //    this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Usuario: " + userEmpleado + "\n" + "Nombre: " + nombreEmpleado );
            //    lblNombre.Content = nombreEmpleado;
            //    lblUser.Content = userEmpleado;
            //}

        }

        public delegate void DoSomethingEvent();

        public void actuar()
        {
            ucCompras.tablita();
        }

        private void lblNombre_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            

            if (idUser)
            {
                lblNombre.Content = nombreEmpleado;
                idUser = false;
            }
            else
            {
                lblNombre.Content = "Id de empleado: " + idEmpleado;
                idUser = true;
            }
                                          
        }

        private void lblUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Payaso");
        }
    }
}
