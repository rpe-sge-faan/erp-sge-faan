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


namespace SGE_erp
{

    public partial class MainWindow : Window
    {
        public static bool acceso;
        public static string userEmpleado;
        public static string passwordEmpleado;
        public MainWindow()
        {
            InitializeComponent();
            LogInWindow liw = new LogInWindow();
            liw.ShowDialog();
            if (acceso == false)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario: " + userEmpleado + "\n" + "Contraseña: " + passwordEmpleado);
            }

        }

        public delegate void DoSomethingEvent();

        public void actuar()
        {
            ucCompras.tablita();
        }

    }
}
