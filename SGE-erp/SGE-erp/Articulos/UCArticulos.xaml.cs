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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SGE_erp;
using SGE_erp.Gestion;
using SGE_erp.SetaDataTableAdapters;

namespace SGE_erp.Articulos
{

    public partial class UCArticulos : UserControl
    {
        public UCArticulos()
        {
            InitializeComponent();
            ActualizarAsignar();
            Actualizar();
        }

        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;
        EditaArticulos a = null;

        private void OcultarColumnas()
        {
            articulosDataGrid.Columns[0].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[1].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[4].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[5].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[7].Visibility = Visibility.Collapsed;
            dataProv.Columns[0].Visibility = Visibility.Collapsed;
            dataArt.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Articulos, TipoArticulo, Iva WHERE Articulos.Id_Iva = Iva.Id_Iva AND Articulos.TipoArticulo = TipoArticulo.Id_Tipo", con);
                DataTable dt = new DataTable(); ;
                da.Fill(dt);

                this.articulosDataGrid.ItemsSource = dt.DefaultView;
                con.Open();
                con.Close();
                articulosDataGrid.Columns[6].Header = "Categoría";

                OcultarColumnas();
            }
            catch
            {
                return;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ActualizarAsignar();
            Actualizar();
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(a))
            {
                a = new EditaArticulos(0);
                RefreshListEvent += new RefreshList(Actualizar);
                a.Title = "Añadir Artículo";
                a.Owner = Application.Current.MainWindow;
                a.ActualizarLista = RefreshListEvent;
                a.Show();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ActualizarAsignar();
            Actualizar();
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
                    a.Title = "Editar Artículo";
                    a.Owner = System.Windows.Application.Current.MainWindow;
                    a.ActualizarLista = RefreshListEvent;
                    a.Show();
                }
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
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

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(a))
            {
                a = new EditaArticulos(-1);
                RefreshListEvent += new RefreshList(Filtrar);
                a.Title = "Buscar Artículo";
                a.Owner = Application.Current.MainWindow;
                a.ActualizarLista = RefreshListEvent;
                a.Show();
            }
        }

        DataView view = null;
        DataTable dt;
        private void Filtrar()
        {
            List<String> nombres = AccesoVentana();
            String[] campos = { "Nombre", "Descripcion", "Categoria", "PorcentajeIva" };

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)articulosDataGrid.ItemsSource).ToTable();
                dt.TableName = "Articulos";
                view.Table = dt;
            }

            view.RowFilter = $"Nombre LIKE '%{nombres[0]}%' AND Descripcion LIKE '%{nombres[1]}%' AND Id_Iva = {nombres[3]} " +
                $"AND TipoArticulo = {nombres[2]}";

            //view.Sort = "CompanyName DESC";
            dt = view.ToTable();
            articulosDataGrid.ItemsSource = null;
            articulosDataGrid.ItemsSource = dt.DefaultView;

            OcultarColumnas();
        }

        public List<String> AccesoVentana()
        {
            List<String> nombres = new List<String>();
            foreach (Window item in Application.Current.Windows)
            {
                if (item.Name == "EditarArticulos")
                {
                    String[] nombresArray = {
                        ((EditaArticulos)item).nombreTextBox1.Text,
                        ((EditaArticulos)item).descripcionTextBox1.Text,
                        (((EditaArticulos)item).tipoArticuloComboBox1.SelectedIndex + 1).ToString(),
                        (((EditaArticulos)item).id_IvaComboBox1.SelectedIndex +1).ToString()
                    };
                    nombres.AddRange(nombresArray);
                }
            }
            return nombres;
        }

        private void GenericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string nombre = ((sender as TextBox).Name).ToString();
            CheckAceptar();
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
                bAsignar.IsEnabled = true;
            }
            else
            {
                bAsignar.IsEnabled = false;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ActualizarAsignar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataTable dt = new DataTable();
                SqlDataAdapter articulos = new SqlDataAdapter("SELECT Id_Articulo, Nombre, Descripcion FROM Articulos", con);
                articulos.Fill(dt);
                dataArt.ItemsSource = dt.DefaultView;

                DataTable dt2 = new DataTable();
                SqlDataAdapter proveedores = new SqlDataAdapter("SELECT Id_Proveedor, Nombre FROM Proveedores", con);
                proveedores.Fill(dt2);
                dataProv.ItemsSource = dt2.DefaultView;

                con.Open();
                con.Close();

                dataProv.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                dataProv.Columns[0].MaxWidth = 0;
                dataProv.Columns[1].Width = new DataGridLength(6, DataGridLengthUnitType.Star);
                dataArt.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            catch
            {
                return;
            }
        }

        private void bAsignar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bDeselect_Click(object sender, RoutedEventArgs e)
        {
            dataProv.UnselectAll();
            dataArt.UnselectAll();
        }
    }
}