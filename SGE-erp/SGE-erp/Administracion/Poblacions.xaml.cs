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

namespace SGE_erp.Administracion
{
    /// <summary>
    /// Lógica de interacción para Poblacions.xaml
    /// </summary>
    public partial class Poblacions : UserControl
    {
        public Poblacions()
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
                codPos.Text = "";
                tbPobla.Text = "";
                tbProv.Text = "";

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            ActualizarTabla();
        }

    }
}
