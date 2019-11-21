using System;
using System.Collections.Generic;
using System.Data;
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

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Lógica de interacción para ProveedoresEdicion.xaml
    /// </summary>
    public partial class ProveedoresEdicion : Window
    {
        private int id;
        public ProveedoresEdicion(int num)
        {
            InitializeComponent();

            this.id = num;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (id != 0)
            {
                string variable;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [Proveedores] where Id_Proveedor = @id";

                    command.Parameters.AddWithValue("@id", id);


                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        //Check the reader has data:
                        if (reader.Read())
                        {
                            variable = reader.GetString(reader.GetOrdinal("Nombre"));
                            nombreTextBox.Text = variable;
                            //MessageBox.Show(variable);
                        }
                        // If you need to use all rows returned use a loop:
                        while (reader.Read())
                        {
                            variable = reader.GetString(reader.GetOrdinal("Column"));
                            MessageBox.Show(variable);
                        }
                    }
                }
            }
        }

        public Delegate ActualizarLista;

        private void bAceptar_Click(object sender, RoutedEventArgs e)
        {
            int tipo;
            if (tipoComboBox.SelectedValue.ToString() == "Particular") { tipo = 1; }
            else { tipo = 2; }


            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Proveedores (Tipo, Nombre, Telefono, Email, Persona_Contacto, Direccion, NIF) " +
                        "VALUES (@tipo, @nombre, @telefono, @email, @persona, @direccion, @nif)";

                    command.Parameters.AddWithValue("@tipo", tipo);
                    command.Parameters.AddWithValue("@nombre", nombreTextBox.Text);
                    command.Parameters.AddWithValue("@telefono", telefonoTextBox.Text);
                    command.Parameters.AddWithValue("@email", emailTextBox.Text);
                    command.Parameters.AddWithValue("@persona", personaContactoTextBox.Text);
                    command.Parameters.AddWithValue("@direccion", direccionTextBox.Text);
                    command.Parameters.AddWithValue("@nif", nIFTextBox.Text);

                    con.Open();
                    int a = command.ExecuteNonQuery();



                    DataSet ds;
                    SqlDataAdapter da;
                    SqlCommandBuilder scb;
                    DataTable dt;

                    da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
                    ds = new DataSet();
                    dt = new DataTable();
                    ds.Clear();
                    da.Fill(dt);


                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Proveedor ERROR");
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

        private void bCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
