﻿using SGE_erp.Gestion;
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

           consultar();

        }

        public void consultar()
        {
            string user;
            string nombre;
            string password;
            int idEmpleado;
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
                        // DESCOMENTAR PARA ACTIVAR LOS INICIOS DE SESION

                        user = reader.GetString(reader.GetOrdinal("Email"));
                        nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                        password = reader.GetString(reader.GetOrdinal("Password"));                        
                        idEmpleado = reader.GetInt32(reader.GetOrdinal("Id_Empleado"));                        

                        if (user.Equals(tbUser.Text) && password.Equals(tbPassword.Password))
                        {
                            MainWindow.acceso = true;
                            MainWindow.idEmpleado = idEmpleado;
                            MainWindow.userEmpleado = user;
                            MainWindow.nombreEmpleado = nombre;

                            this.Close();
                        }
                        else
                        {
                            //MessageBox.Show("TextBox: " + tbUser.Text + "\n" + "PasswordBox: " + tbPassword.Password + "\n" + "user: " + user + "password: " + password);
                        }

                    }
                    else
                    {
                        labelError.Content = "Usuario o contraseña incorrectos";
                        labelError.Visibility = Visibility.Visible;
                        tbPassword.Password = "";
                        

                        // ELIMINAR CUANDO SE ACTIVE EL INICIO DE SESION
                        /*MainWindow.acceso = true;
                        this.Close();*/
                    }
                }
            }
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                consultar();
            }
        }

        private void tbUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && String.IsNullOrEmpty(tbPassword.Password))
            {
                labelError.Content = "Inserte contraseña";
                labelError.Visibility = Visibility.Visible;
            }
        }
    }
}
