using SGE_erp.Gestion;
using System;
using System.Collections.Generic;
using System.Data;
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


namespace SGE_erp.Articulos
{
    public partial class EditaArticulos : Window
    {
        private int id;
        public Delegate ActualizarLista;
        public Delegate FiltrarLista;
        public delegate void RefreshList();

        public EditaArticulos(int num)
        {
            InitializeComponent();
            this.id = num;
        }
        int categoria = 0;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (id == 0)
            {
                bAceptar.IsEnabled = false;
                id_IvaComboBox1.SelectedIndex = 1;
                tipoArticuloComboBox1.SelectedIndex = 1;
                txtBoxNUEVOPVP.Text = "0";
                txtBoxNUEVOstock.Text = "0";
            }
            else if (id == -1)
            {
                id_IvaComboBox1.SelectedIndex = 0;
                tipoArticuloComboBox1.SelectedIndex = 0;
                txtBoxNUEVOPVP.Text = "0";
                txtBoxNUEVOstock.Text = "0";
            }
            else
            {
                string variable;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    //"SELECT * FROM Articulos, TipoArticulo, Iva WHERE (Articulos.Id_Iva = Iva.Id_Iva AND Articulos.TipoArticulo = TipoArticulo.Id_Tipo) AND Id_Articulo = @id"
                    command.CommandText = "SELECT * FROM [Articulos] WHERE Id_Articulo = @id";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            variable = reader.GetString(reader.GetOrdinal("Nombre"));
                            nombreTextBox1.Text = variable;
                            // id
                            id_ArticuloTextBox1.Text = id.ToString();
                            variable = reader.GetString(reader.GetOrdinal("Descripcion"));
                            descripcionTextBox1.Text = variable;

                            int tipo = reader.GetInt32(reader.GetOrdinal("Id_Iva"));
                            id_IvaComboBox1.SelectedIndex = tipo;

                            int tipo2 = reader.GetInt32(reader.GetOrdinal("TipoArticulo"));
                            Console.Write(tipo2);
                            tipoArticuloComboBox1.SelectedItem = tipo2;
                            categoria = tipo2;

                            variable = (reader.GetDecimal(reader.GetOrdinal("PVP"))).ToString();
                            txtBoxNUEVOPVP.Text = variable;

                            variable = (reader.GetInt32(reader.GetOrdinal("Stock"))).ToString();
                            txtBoxNUEVOstock.Text = variable;

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
                using (SqlConnection con = new SqlConnection(MetodosGestion.db))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "UPDATE Articulos SET Id_Iva =@Id_Iva, Nombre =@nombre, Descripcion =@descripcion, TipoArticulo =@tipoArticulo, PVP =@pvp, Stock=@sTock WHERE Id_Articulo = @id";

                    command.Parameters.AddWithValue("@Id_Iva", id_IvaComboBox1.SelectedIndex);
                    command.Parameters.AddWithValue("@nombre", nombreTextBox1.Text);
                    command.Parameters.AddWithValue("@descripcion", descripcionTextBox1.Text);
                    command.Parameters.AddWithValue("@tipoArticulo", tipoArticuloComboBox1.SelectedValue);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@pvp", decimal.Parse(txtBoxNUEVOPVP.Text));
                    command.Parameters.AddWithValue("@sTock", int.Parse(txtBoxNUEVOstock.Text));

                    con.Open();
                    int a = command.ExecuteNonQuery();

                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Editar Articulo ERROR");
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
                    command.CommandText = "INSERT INTO Articulos (Id_Iva, Nombre, Descripcion, TipoArticulo, PVP, Stock) " +
                        "VALUES (@Id_Iva, @nombre, @descripcion, @tipoArticulo, @pvp, @sTock)";

                    command.Parameters.AddWithValue("@Id_Iva", id_IvaComboBox1.SelectedIndex);
                    command.Parameters.AddWithValue("@nombre", nombreTextBox1.Text);
                    command.Parameters.AddWithValue("@descripcion", descripcionTextBox1.Text);
                    command.Parameters.AddWithValue("@tipoArticulo", tipoArticuloComboBox1.SelectedIndex);
                    command.Parameters.AddWithValue("@pvp", txtBoxNUEVOPVP.Text);
                    command.Parameters.AddWithValue("@sTock", txtBoxNUEVOstock.Text);

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

        private void GenericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (id != -1)
            {
                string nombre = ((sender as TextBox).Name).ToString();
                CheckAceptar();
            }
            else if (id == -1)
            {
                FiltrarLista.DynamicInvoke();
            }
        }

        private void CheckAceptar()
        {
            bool enable = true;
            var textBoxes = gridGeneral.Children.OfType<TextBox>();

            foreach (TextBox txt in textBoxes)
            {
                if (txt.IsEnabled)
                {
                    if (String.IsNullOrWhiteSpace(txt.Text))
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tipoArticuloComboBox1_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TipoArticulo", con);
            DataTable dt = new DataTable();

            ds.Clear();
            da.Fill(dt);

            this.tipoArticuloComboBox1.ItemsSource = dt.DefaultView;

            tipoArticuloComboBox1.DisplayMemberPath = dt.Columns["Descripcion"].ToString();
            tipoArticuloComboBox1.SelectedValuePath = dt.Columns["Id_Tipo"].ToString();
            con.Open();
            con.Close();

            if (id == -1)
            {
                tipoArticuloComboBox1.SelectedIndex = -1;
            }
            else if (id == 0)
            {
                tipoArticuloComboBox1.SelectedIndex = 0;
            }
            else
            {
                tipoArticuloComboBox1.SelectedValue = categoria;
            }

        }

        private void bReset_Click(object sender, RoutedEventArgs e)
        {
            txtBoxNUEVOPVP.Text = "0";
            txtBoxNUEVOstock.Text = "0";
            nombreTextBox1.Text = "";
            descripcionTextBox1.Text = "";
            tipoArticuloComboBox1.SelectedIndex = -1;
            id_IvaComboBox1.SelectedIndex = 0;
        }
    }
}