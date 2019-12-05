using SGE_erp.Gestion;
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

/*namespace SGE_erp.Articulos
{
    /// <summary>
    /// Lógica de interacción para EditaArticulos.xaml
    /// </summary>
    public partial class EditaArticulos : Window
    {
        private int id;
        public EditaArticulos(int num)
        {
            InitializeComponent();

            this.id = num;
        }

        public Delegate ActualizarLista;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (id != 0)
            {
                string variable;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [Articulos] where Id_Articulo = @id";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        string[] columnas = new string[] { "Id_Articulo", "Nombre", "Descripcion", "Tipo" };

                        if (reader.Read())
                        {
                            variable = reader.GetString(reader.GetOrdinal("Nombre"));
                            nombreTextBox1.Text = variable;
                            // id
                            id_ArticuloTextBox1.Text = id.ToString();
                            variable = reader.GetString(reader.GetOrdinal("Descripcion"));
                            descripcionTextBox1.Text = variable;

                            int tipo = reader.GetInt32(reader.GetOrdinal("Id_Iva"));
                            id_IvaComboBox1.SelectedIndex = tipo - 1;

                            //int tipo2 = reader.GetInt32(reader.GetOrdinal("Tipo"));
                            //tipoArticuloTextBox1.SelectedIndex = tipo2 - 1;

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
            else
            {
                bAceptar.IsEnabled = false;
            }
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            //int tipo;
            //if (tipoComboBox.SelectedValue.ToString() == "Particular") { tipo = 1; }
           // else { tipo = 2; }

            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Proveedores (Id_Iva, Nombre, Descripcion, TipoArticulo) " +
                        "VALUES (@Id_Iva, @nombre, @descripcion, @tipoArticulo)";

                    command.Parameters.AddWithValue("@Id_Iva", id_IvaComboBox1.SelectedIndex+1);
                    command.Parameters.AddWithValue("@nombre", id_ArticuloTextBox1.Text);
                    command.Parameters.AddWithValue("@descripcion", descripcionTextBox1.Text);
                    command.Parameters.AddWithValue("@tipoArticulo", tipoArticuloTextBox1.Text);

                    con.Open();
                    int a = command.ExecuteNonQuery();

                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("ERROR articulos ");
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

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}*/

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Lógica de interacción para ProveedoresEdicion.xaml
    /// </summary>
    public partial class EditaArticulos : Window
    {
        private int id;
        public EditaArticulos(int num)
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
                    command.CommandText = "SELECT * FROM [Articulos] where Id_Articulo = @id";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        string[] columnas = new string[] { "Id_Articulo", "Id_Iva", "Nombre", "Descripcion", "TipoArticulo" };

                        if (reader.Read())
                        {
                            variable = reader.GetString(reader.GetOrdinal("Nombre"));
                            nombreTextBox1.Text = variable;
                            // id
                            id_ArticuloTextBox1.Text = id.ToString();
                            variable = reader.GetString(reader.GetOrdinal("Descripcion"));
                            descripcionTextBox1.Text = variable;

                            int tipo = reader.GetInt32(reader.GetOrdinal("Id_Iva"));
                            id_IvaComboBox1.SelectedIndex = tipo - 1;

                            int tipo2 = reader.GetInt32(reader.GetOrdinal("TipoArticulo"));
                            tipoArticuloComboBox1.SelectedIndex = tipo2 - 1;

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
            else
            {
                bAceptar.IsEnabled = false;
            }
        }

        public Delegate ActualizarLista;

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (id == 0)
            {
                nuevo();
            }
            else
            {
                editar();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void editar()
        {
            int tipo = tipoArticuloComboBox1.SelectedIndex + 1;

            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Articulos (Id_Iva, Nombre, Descripcion, TipoArticulo) " +
                        "VALUES (@Id_Iva, @nombre, @descripcion, @tipoArticulo)";

                    command.Parameters.AddWithValue("@Id_Iva", id_IvaComboBox1.SelectedIndex + 1);
                    command.Parameters.AddWithValue("@nombre", id_ArticuloTextBox1.Text);
                    command.Parameters.AddWithValue("@descripcion", descripcionTextBox1.Text);
                    command.Parameters.AddWithValue("@tipoArticulo", tipo);

                    con.Open();
                    int a = command.ExecuteNonQuery();



                    //DataSet ds;
                    //SqlDataAdapter da;
                    //SqlCommandBuilder scb;
                    //DataTable dt;

                    //da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
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
                        MessageBox.Show("Articulo ERROR");
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

        private void nuevo()
        {

            int tipo;
            //if (tipoComboBox.SelectedValue.ToString() == "Particular") { tipo = 1; }
            //else { tipo = 2; }


            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Articulos (Id_Iva, Nombre, Descripcion, TipoArticulo) " +
                        "VALUES (@Id_Iva, @nombre, @descripcion, @tipoArticulo)";

                    command.Parameters.AddWithValue("@Id_Iva", id_IvaComboBox1.SelectedIndex + 1);
                    command.Parameters.AddWithValue("@nombre", id_ArticuloTextBox1.Text);
                    command.Parameters.AddWithValue("@descripcion", descripcionTextBox1.Text);
                    command.Parameters.AddWithValue("@tipoArticulo", tipoArticuloComboBox1.Text);

                    con.Open();
                    int a = command.ExecuteNonQuery();

                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Articulos ERROR");
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

        /*private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }*/

    }
}