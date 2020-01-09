using SGE_erp.Gestion;
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
using System.Windows.Shapes;

namespace SGE_erp
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
            
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow.acceso = false;
            this.Close();

        }

        private void Entrar_Click(object sender, RoutedEventArgs e)
        {
            
            //MainWindow.acceso = true;
            MainWindow.userEmpleado = tbUser.Text;
            MainWindow.passwordEmpleado = tbPassword.Password;
            consultar();
            //this.Close();

        }

        public void consultar()
        {
            //string user;
            //string password;
            string bd = MetodosGestion.db;
            using (SqlConnection con = new SqlConnection(bd))
            using (SqlCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM [Empleados] WHERE Email = @email AND Password = @contr";
                command.Parameters.AddWithValue("@email", tbUser.Text);
                command.Parameters.AddWithValue("@contr", tbPassword.Password);
                con.Open();

                using (var reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        MainWindow.acceso = true;
                        this.Close();
                        /*user = reader.GetString(reader.GetOrdinal("Email"));
                        MessageBox.Show(user); // SI EXISTE ENTONCES LO MUESTRA
                        
                        password = reader.GetString(reader.GetOrdinal("Telefono"));
                        MessageBox.Show(password); // SI EXISTE ENTONCES LO MUESTRA*/
                    }
                    else
                    {
                        labelError.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
