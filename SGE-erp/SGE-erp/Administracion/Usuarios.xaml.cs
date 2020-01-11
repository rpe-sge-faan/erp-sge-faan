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

                dataGridUsuarios.Columns[0].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[1].Visibility = Visibility.Collapsed;
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
            tbPobla.Text = "";
            tbProv.Text = "";
            Actualizar();
        }


        /*
private void bAnadir_Click(object sender, RoutedEventArgs e)
{
   try
   {
       DataRowView dv = (DataRowView)dataGridUsuarios.SelectedItem;
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
*/
    }
}
