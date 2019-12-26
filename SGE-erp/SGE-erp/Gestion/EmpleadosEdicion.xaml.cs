using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace SGE_erp.Gestion
{

    public partial class EmpleadosEdicion : Window
    {
        public int id;
        public Delegate ActualizarLista;
        public Delegate FiltrarLista;

        public EmpleadosEdicion(int num)
        {
            InitializeComponent();
            this.id = num;
            if (id == -1)
            {
                salarioTextBox.Text = "0";
                fechaDatePicker.SelectedDate = new DateTime(2000, 1, 1);
                ventasTextBox.Text = "0";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (id == 0)
            {
                bAceptar.IsEnabled = false;
            }
            else if (id == -1)
            {
                
            }
            else
            {
                string variable;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [Empleados] where Id_Empleado = @id";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            variable = reader.GetString(reader.GetOrdinal("Nombre"));
                            nombreTextBox.Text = variable;
                            id_ClienteTextBox.Text = id.ToString();
                            variable = reader.GetString(reader.GetOrdinal("NIF"));
                            nifTextBox.Text = variable;
                            variable = reader.GetString(reader.GetOrdinal("Email"));
                            emailTextBox.Text = variable;
                            variable = reader.GetString(reader.GetOrdinal("Telefono"));
                            telefonoTextBox.Text = variable;
                            variable = reader.GetString(reader.GetOrdinal("Direccion"));
                            direccionTextBox.Text = variable;

                            variable = (reader.GetInt32(reader.GetOrdinal("numVentas"))).ToString();
                            ventasTextBox.Text = variable;
                            variable = (reader.GetDecimal(reader.GetOrdinal("Salario"))).ToString();
                            salarioTextBox.Text = variable;
                            DateTime time = reader.GetDateTime(reader.GetOrdinal("FechaContratacion"));
                            fechaDatePicker.DisplayDate = time;
                            fechaDatePicker.SelectedDate = time;
                            cpBox.Text = reader.GetString(reader.GetOrdinal("CodPostal"));
                        }
                    }
                }
            }
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (id == 0)
            {
                Nuevo();
            }
            else if (id == -1)
            {
                FiltrarLista.DynamicInvoke();
            }
            else
            {
                Editar();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Editar()
        {
            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "UPDATE Empleados SET Nombre = @nombre, Telefono = @telefono, Email = @email, " +
                        "Direccion = @direccion, NIF = @nif, NumVentas = @ventas, Salario = @salario, FechaContratacion = @fecha, CodPostal = @codp WHERE Id_Empleado = @id";

                    command.Parameters.AddWithValue("@nombre", nombreTextBox.Text);
                    command.Parameters.AddWithValue("@telefono", telefonoTextBox.Text);
                    command.Parameters.AddWithValue("@email", emailTextBox.Text);
                    command.Parameters.AddWithValue("@direccion", direccionTextBox.Text);
                    command.Parameters.AddWithValue("@nif", nifTextBox.Text);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@ventas", int.Parse(ventasTextBox.Text));
                    command.Parameters.AddWithValue("@salario", decimal.Parse(salarioTextBox.Text));
                    command.Parameters.AddWithValue("@fecha", fechaDatePicker.DisplayDate);
                    command.Parameters.AddWithValue("@codp", cpBox.Text);

                    con.Open();
                    int a = command.ExecuteNonQuery();



                    //DataSet ds;
                    //SqlDataAdapter da;
                    //SqlCommandBuilder scb;
                    //DataTable dt;

                    //da = new SqlDataAdapter("SELECT * FROM [Empleados]", con);
                    //ds = new DataSet();
                    //dt = new DataTable();
                    //ds.Clear();
                    //da.Fill(dt);


                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Empleado ERROR");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            ActualizarLista.DynamicInvoke();

            this.Close();
        }

        private void Nuevo()
        {
            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Empleados (Nombre, Telefono, Email, Direccion, NIF, NumVentas, Salario, FechaContratacion, CodPostal) " +
                        "VALUES (@nombre, @telefono, @email, @direccion, @nif, @ventas, @salario, @fecha, @codp)";

                    command.Parameters.AddWithValue("@nombre", nombreTextBox.Text);
                    command.Parameters.AddWithValue("@telefono", telefonoTextBox.Text);
                    command.Parameters.AddWithValue("@email", emailTextBox.Text);
                    command.Parameters.AddWithValue("@direccion", direccionTextBox.Text);
                    command.Parameters.AddWithValue("@nif", nifTextBox.Text);
                    command.Parameters.AddWithValue("@ventas", ventasTextBox.Text);
                    command.Parameters.AddWithValue("@salario", salarioTextBox.Text);
                    command.Parameters.AddWithValue("@fecha", fechaDatePicker.DisplayDate);
                    command.Parameters.AddWithValue("@codp", cpBox.Text);

                    con.Open();
                    int a = command.ExecuteNonQuery();

                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Empleado ERROR");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            ActualizarLista.DynamicInvoke();

            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        int cont = 0;
        private void GenericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (id != -1)
            {
                //(sender as TextBox).SelectAll();
                //System.Diagnostics.Debug.WriteLine(txt.Name);
                //emailTextBox_TextChanged(sender, e);
                string nombre = ((sender as TextBox).Name).ToString();
                switch (nombre)
                {
                    case "emailTextBox":
                        try
                        {
                            var addr = new System.Net.Mail.MailAddress(emailTextBox.Text);
                            if (addr.Address == emailTextBox.Text)
                            {
                                emailTextBox.ClearValue(TextBox.BackgroundProperty);
                                labelInfo.Content = "";
                            }
                        }
                        catch
                        {
                            emailTextBox.Background = (Brush)new BrushConverter().ConvertFrom("#FFBDAF");
                            labelInfo.Content = "Error en el formato del email";
                        }
                        break;
                    case "nifTextBox":
                        Regex regex = new Regex("^[0-9]{8}[TtRrWwAaGgMmYyFfPpDdXxBbNnJjZzSsQqVvHhLlCcKkEe]$");
                        Regex regex2 = new Regex("^[TtRrWwAaGgMmYyFfPpDdXxBbNnJjZzSsQqVvHhLlCcKkEe][0-9]{8}$");
                        if (regex.IsMatch(nifTextBox.Text) || regex2.IsMatch(nifTextBox.Text))
                        {
                            nifTextBox.ClearValue(TextBox.BackgroundProperty);
                            labelInfo.Content = "";
                        }
                        else
                        {
                            nifTextBox.Background = (Brush)new BrushConverter().ConvertFrom("#FFBDAF");
                            labelInfo.Content = "Error en el formato del DNI";
                        }
                        break;
                }
                CheckAceptar();
                //EmpleadosMain.AccesoVentana();
            }
            else if (id == -1 && cont >1)
            {
                FiltrarLista.DynamicInvoke();
            }
            cont++;
        }

        private void CheckAceptar()
        {
            bool enable = true;
            var textBoxes = gridGeneral.Children.OfType<TextBox>();

            foreach (TextBox txt in textBoxes)
            {
                if (txt.IsEnabled)
                {
                    if (!txt.Background.ToString().Equals("#FFFFFFFF") || String.IsNullOrWhiteSpace(txt.Text))
                    {
                        enable = false;
                    }
                }
            }

            if (enable)
            {
                bAceptar.IsEnabled = true;
            }
            else
            {
                bAceptar.IsEnabled = false;
            }
        }

        private void cpBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox obj = sender as TextBox;
            string name = obj.Name;

            string variable;
            string bd = MetodosGestion.db;
            using (SqlConnection con = new SqlConnection(bd))
            using (SqlCommand command = con.CreateCommand())
            {
                //command.CommandText = "SELECT * FROM [Poblaciones] WHERE CodPostal LIKE @id OR Poblacion LIKE @pobla OR Provincia LIKE @prov";
                command.CommandText = "SELECT * FROM [Poblaciones] WHERE CodPostal LIKE @id";
                command.Parameters.AddWithValue("@id", cpBox.Text);
                command.Parameters.AddWithValue("@pobla", poblText.Text);
                command.Parameters.AddWithValue("@prov", provText.Text);
                con.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int cod = 0;
                        if (name.Equals("cpBox"))
                        {
                            variable = reader.GetString(reader.GetOrdinal("Poblacion"));
                            poblText.Text = variable;
                            variable = reader.GetString(reader.GetOrdinal("Provincia"));
                            provText.Text = variable;
                        }
                    }
                }
            }
        }
    }
}