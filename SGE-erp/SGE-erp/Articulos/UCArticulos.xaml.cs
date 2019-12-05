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
using SGE_erp;
using SGE_erp.Gestion;
using SGE_erp.SetaDataTableAdapters;

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Interaction logic for UCArticulos.xaml
    /// </summary>
    public partial class UCArticulos : UserControl
    {
        public UCArticulos()
        {
            InitializeComponent();
            Actualizar();
        }

        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;
        EditaArticulos a = null;

        private void RefreshListView()
        {
            Actualizar();
        }

        private void Actualizar()
        {
            try
            {
                //string bd = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Datos.mdf;Integrated Security=True";
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Articulos]", con);
                DataTable dt = new DataTable(); ;

                //ds.Clear();
                da.Fill(dt);

                this.articulosDataGrid.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                articulosDataGrid.Columns[0].Visibility = Visibility.Collapsed;

            }
            catch
            {
                return;
            }
        }

        EditaArticulos ea = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void anadirArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(ea))
            {

                ea = new EditaArticulos(0);
                RefreshListEvent += new RefreshList(RefreshListView); // event initialization
                ea.Title = "Añadir Articulo";
                ea.Owner = System.Windows.Application.Current.MainWindow;
                ea.ActualizarLista = RefreshListEvent; // assigning event to the Delegate
                ea.Show();
            }

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
            //MessageBox.Show("REFRESCADO");
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(a))
            {
                if (articulosDataGrid.SelectedItem != null)
                {
                    DataRowView dd = (DataRowView)articulosDataGrid.SelectedItem;
                    int id = dd.Row.Field<int>("Id_Articulo");

                    a = new EditaArticulos(id);
                    RefreshListEvent += new RefreshList(Actualizar);
                    a.Title = "Editar Articulo";
                    a.Owner = System.Windows.Application.Current.MainWindow;
                    a.ActualizarLista = RefreshListEvent;
                    a.Show();
                }
            }
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (articulosDataGrid.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)articulosDataGrid.SelectedItem;
                int id = dd.Row.Field<int>("Id_Articulo");
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro?", "Confirmacion Borrado", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        string bd = MetodosGestion.db;
                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand command = con.CreateCommand())
                        {
                            command.CommandText = "DELETE FROM Articulos WHERE Id_Articulo = @id";

                            command.Parameters.AddWithValue("@id", id);

                            con.Open();
                            int a = command.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                MessageBox.Show("Error al borrar articulo");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Actualizar();
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            BuscarArticulos bArt = new BuscarArticulos();
            bArt.ShowDialog();
        }

    }
}


