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
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios : UserControl
    {
        public Usuarios()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Empleados]", con);
                DataTable dt = new DataTable(); ;

                // ds.Clear();
                da.Fill(dt);
                this.dataGridUsuarios.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                // MOSTRAR NOMBRE E ID
                //dataGridUsuarios.Columns[0].Visibility = Visibility.Collapsed;
                //dataGridUsuarios.Columns[1].Visibility = Visibility.Collapsed;

                dataGridUsuarios.Columns[2].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[3].Visibility = Visibility.Collapsed;


                dataGridUsuarios.Columns[5].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[6].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[7].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[8].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[9].Visibility = Visibility.Collapsed;

            }
            catch
            {
                return;
            }
        }

        private void bReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void reset()
        {
            dataGridUsuarios.UnselectAll();
            tbProv.Text = "";
            Actualizar();
        }

        private void dataGridPoblacion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dd = (DataRowView)dataGridUsuarios.SelectedItem;
            string email = dd.Row.Field<string>("Email");
            using (SqlConnection con = new SqlConnection(MetodosGestion.db))
            using (SqlCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Empleados WHERE Email = @cod";
                command.Parameters.AddWithValue("@cod", email);
                con.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tbProv.Text = reader.GetString(reader.GetOrdinal("Password"));
                       
                    }
                }
            }
        }

        private void bEditar_Click_1(object sender, RoutedEventArgs e)
        {
            DataRowView dd = (DataRowView)dataGridUsuarios.SelectedItem;
            string email ="";

            try
            {
                email = dd.Row.Field<string>("Email");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un email");
                
            }

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro que quieres usar '' " + tbProv.Text + " '' como nueva contraseña?", "Confirmacion Editado", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "UPDATE Empleados SET Password=@contra WHERE Email=@email";

                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@contra", tbProv.Text);

                    con.Open();
                    int a = command.ExecuteNonQuery();

                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        //MessageBox.Show("No puede editar el e-mail\r\nCree un elemento nuevo");
                    }
                }
                reset();
            }
        }

        private void bBorrar_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGridUsuarios.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)dataGridUsuarios.SelectedItem;
                string cod = dd.Row.Field<string>("Email");
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro?", "Confirmacion Borrado", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(MetodosGestion.db))
                    using (SqlCommand command = con.CreateCommand())
                    {
                        command.CommandText = "UPDATE Empleados SET Password=@contra WHERE Email=@cod";

                        command.Parameters.AddWithValue("@cod", cod);
                        command.Parameters.AddWithValue("@contra", 123456789);

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
