using SGE_erp.Gestion;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Interaction logic for Poblaciones.xaml
    /// </summary>
    public partial class Poblaciones : UserControl
    {
        public Poblaciones()
        {
            InitializeComponent();
        }

        private void ActualizarTabla()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Poblaciones", con);
                DataTable dt = new DataTable(); ;
                da.Fill(dt);

                dataGridPoblacion.ItemsSource = dt.DefaultView;
                con.Open();
                con.Close();

                //tipoArtdata.Columns[0].Visibility = Visibility.Hidden;
            }
            catch
            {
                return;
            }
        }

        private void AdminPoblaciones_Loaded(object sender, RoutedEventArgs e)
        {
            ActualizarTabla();
        }

        private void bAnadir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)dataGridPoblacion.SelectedItem;
                //int idT = dTipo.Row.Field<int>("Id_Tipo");
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM [Poblaciones] WHERE ([CodPostal] = @tipo)";
                    command.Parameters.AddWithValue("@tipo", codPos.Text);
                    con.Open();

                    int existe = (int)command.ExecuteScalar();

                    if (existe > 0)
                    {
                        MessageBoxResult resultado = MessageBox.Show("Esta poblaición ya existe");
                    }
                    else
                    {
                        using (SqlCommand anadir = con.CreateCommand())
                        {

                            anadir.CommandText = "INSERT INTO Poblaciones VALUES (@id, @poblacion, @provincia)";

                            anadir.Parameters.AddWithValue("@id", codPos.Text);
                            anadir.Parameters.AddWithValue("@poblacion", tbPobla.Text);
                            anadir.Parameters.AddWithValue("@provincia", tbProv.Text);

                            //con.Open();
                            int a = anadir.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                MessageBox.Show("Poblacion ERROR");
                            }
                        }
                    }
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            reset();
        }

        private void bReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void reset()
        {
            dataGridPoblacion.UnselectAll();
            codPos.Text = "";
            tbPobla.Text = "";
            tbProv.Text = "";
            ActualizarTabla();
        }

        private void dataGridPoblacion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dd = (DataRowView)dataGridPoblacion.SelectedItem;
            string cod = dd.Row.Field<string>("CodPostal");
            using (SqlConnection con = new SqlConnection(MetodosGestion.db))
            using (SqlCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Poblaciones WHERE CodPostal = @cod";
                command.Parameters.AddWithValue("@cod", cod);
                con.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        codPos.Text = reader.GetString(reader.GetOrdinal("CodPostal"));
                        tbPobla.Text = reader.GetString(reader.GetOrdinal("Poblacion"));
                        tbProv.Text = reader.GetString(reader.GetOrdinal("Provincia"));
                    }
                }
            }
        }

        private void bEditar_Click(object sender, RoutedEventArgs e)
        {
            string bd = MetodosGestion.db;
            using (SqlConnection con = new SqlConnection(bd))
            using (SqlCommand command = con.CreateCommand())
            {
                command.CommandText = "UPDATE Poblaciones SET CodPostal=@cod, Poblacion = @pob, Provincia = @prov WHERE CodPostal = @cod";

                command.Parameters.AddWithValue("@cod", codPos.Text);
                command.Parameters.AddWithValue("@pob", tbPobla.Text);
                command.Parameters.AddWithValue("@prov", tbProv.Text);

                con.Open();
                int a = command.ExecuteNonQuery();

                if (a != 0)
                {
                    con.Close();
                }
                else
                {
                    MessageBox.Show("No puede editar el CP\r\nCree un elemento nuevo");
                }
            }
            reset();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPoblacion.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)dataGridPoblacion.SelectedItem;
                string cod = dd.Row.Field<string>("CodPostal");
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro?", "Confirmacion Borrado", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(MetodosGestion.db))
                    using (SqlCommand command = con.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM Poblaciones WHERE CodPostal = @id";

                        command.Parameters.AddWithValue("@id", cod);

                        con.Open();
                        int a = command.ExecuteNonQuery();

                        if (a != 0)
                        {
                            con.Close();
                        }
                        else
                        {
                            MessageBox.Show("Proveedor ERROR al borrar");
                        }
                    }
                    reset();
                }
            }
        }
    }
}
