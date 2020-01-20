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


namespace SGE_erp.Articulos
{

    public partial class UCArticulos : UserControl
    {
        public UCArticulos()
        {
            InitializeComponent();
        }

        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;
        EditaArticulos a = null;

        private void OcultarColumnas()
        {
            articulosDataGrid.Columns[0].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[1].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[4].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[7].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[9].Visibility = Visibility.Collapsed;
        }

        private void ActualizarCategorias()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TipoArticulo", con);
                DataTable dt = new DataTable(); ;
                da.Fill(dt);

                tipoArtdata.ItemsSource = dt.DefaultView;
                con.Open();
                con.Close();

                //tipoArtdata.Columns[0].Visibility = Visibility.Hidden;
            }
            catch
            {
                return;
            }
            nombreTb.Text = "";
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
                articulosDataGrid.Columns[8].Header = "Categoría";

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
            ActualizarCategorias();
            Actualizar();
            bAsignar.IsEnabled = false;
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
                bool resul = Mensajes.Mostrar("¿Borrar artículo?", Mensajes.Tipo.Confirmacion);
                if (resul)
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
                                Mensajes.Mostrar("Error al borrar artículo", Mensajes.Tipo.Error);
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
                a.FiltrarLista = RefreshListEvent;
                a.Show();
            }
        }

        DataView view = null;
        DataTable dt;
        private void Filtrar()
        {
            List<String> nombres = AccesoVentana();

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)articulosDataGrid.ItemsSource).ToTable();
                dt.TableName = "Articulos";
                view.Table = dt;
            }
            if (nombres[2].Equals("0") && nombres[3].Equals("0"))
            {
                view.RowFilter = $"Nombre LIKE '%{nombres[0]}%' AND Descripcion LIKE '%{nombres[1]}%' AND [PVP] >= {nombres[4]} AND [Stock] >= {nombres[5]}";
            }
            else if (nombres[2].Equals("0"))
            {
                view.RowFilter = $"Nombre LIKE '%{nombres[0]}%' AND Descripcion LIKE '%{nombres[1]}%' AND Id_Iva = {nombres[3]} AND PVP >= {nombres[4]} AND Stock >= {nombres[5]}";
            }
            else if (nombres[3].Equals("0"))
            {
                view.RowFilter = $"Nombre LIKE '%{nombres[0]}%' AND Descripcion LIKE '%{nombres[1]}%' AND TipoArticulo = {nombres[2]} AND PVP >= {nombres[4]} AND Stock >= {nombres[5]}";
            }
            else
            {
                view.RowFilter = $"Nombre LIKE '%{nombres[0]}%' AND Descripcion LIKE '%{nombres[1]}%' AND Id_Iva = {nombres[3]} AND TipoArticulo = {nombres[2]} AND PVP >= {nombres[4]} AND Stock >= {nombres[5]}";
            }

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
                    String stock;
                    String pvp;
                    String tipo;
                    if ((((EditaArticulos)item).txtBoxNUEVOstock.Text).Equals(""))
                    {
                        stock = "0";
                    }
                    else
                    {
                        stock = ((EditaArticulos)item).txtBoxNUEVOstock.Text;
                    }
                    if (((EditaArticulos)item).txtBoxNUEVOPVP.Text.Equals(""))
                    {
                        pvp = "0";
                    }
                    else
                    {
                        pvp = ((EditaArticulos)item).txtBoxNUEVOPVP.Text;
                    }
                    if (((EditaArticulos)item).tipoArticuloComboBox1.SelectedIndex == -1)
                    {
                        tipo = "0";
                    }
                    else
                    {
                        tipo = (((EditaArticulos)item).tipoArticuloComboBox1.SelectedValue).ToString();
                    }

                    String[] nombresArray = {
                        ((EditaArticulos)item).nombreTextBox1.Text,
                        ((EditaArticulos)item).descripcionTextBox1.Text,
                        tipo,
                        (((EditaArticulos)item).id_IvaComboBox1.SelectedIndex).ToString(),
                        pvp,
                        stock
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
                    if (String.IsNullOrWhiteSpace(txt.Text))
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
            if (dataArt.SelectedItem != null && dataProv.SelectedItem != null)
            {
                DataRowView dArt = (DataRowView)dataArt.SelectedItem;
                DataRowView dPro = (DataRowView)dataProv.SelectedItem;
                int idArticulo = dArt.Row.Field<int>("Id_Articulo");
                int idProveedor = dPro.Row.Field<int>("Id_Proveedor");

                SqlConnection con = new SqlConnection(MetodosGestion.db);
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM [ProveedorArticulo] WHERE ([Id_Proveedor] = @prov AND [Id_Articulo] = @art)";

                    command.Parameters.AddWithValue("@prov", idProveedor);
                    command.Parameters.AddWithValue("@art", idArticulo);

                    con.Open();
                    int existe = (int)command.ExecuteScalar();

                    if (existe > 0)
                    {
                        bool resul = Mensajes.Mostrar("Relación existente, ¿desea modificarla?", Mensajes.Tipo.Confirmacion);
                        if (resul)
                        {
                            using (SqlCommand editar = con.CreateCommand())
                            {
                                editar.CommandText = "UPDATE ProveedorArticulo SET PrecioCompra =@precio WHERE Id_Articulo = @idA AND Id_Proveedor = @idP";

                                editar.Parameters.AddWithValue("@precio", decimal.Parse(preCompraTextBox.Text));
                                editar.Parameters.AddWithValue("@idP", idProveedor);
                                editar.Parameters.AddWithValue("@idA", idArticulo);

                                int a = editar.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        using (SqlCommand anadir = con.CreateCommand())
                        {
                            String elemento = idArticulo.ToString() + idProveedor.ToString();

                            anadir.CommandText = "INSERT INTO ProveedorArticulo (Id_Elemento, PrecioCompra, Id_Articulo, Id_Proveedor) VALUES (@elemento, @precio, @idA, @idP)";

                            anadir.Parameters.AddWithValue("@precio", decimal.Parse(preCompraTextBox.Text));
                            anadir.Parameters.AddWithValue("@idP", idProveedor);
                            anadir.Parameters.AddWithValue("@idA", idArticulo);
                            anadir.Parameters.AddWithValue("@elemento", int.Parse(elemento));

                            int a = anadir.ExecuteNonQuery();
                        }
                    }
                }
                con.Close();
                preCompraTextBox.Text = "";
            }
        }

        private void bDeselect_Click(object sender, RoutedEventArgs e)
        {
            dataProv.UnselectAll();
            dataArt.UnselectAll();
            bAsignar.IsEnabled = false;
        }

        private void tipoArtdata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tipoArtdata.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)tipoArtdata.SelectedItem;
                int id = dd.Row.Field<int>("Id_Tipo");
                string variable;
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    //"SELECT * FROM Articulos, TipoArticulo, Iva WHERE (Articulos.Id_Iva = Iva.Id_Iva AND Articulos.TipoArticulo = TipoArticulo.Id_Tipo) AND Id_Articulo = @id"
                    command.CommandText = "SELECT * FROM [TipoArticulo] WHERE Id_Tipo = @id";
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            variable = reader.GetString(reader.GetOrdinal("Descripcion"));
                            nombreTb.Text = variable;
                        }
                    }
                }
            }
        }

        private void BorrarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (tipoArtdata.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)tipoArtdata.SelectedItem;
                int id = dd.Row.Field<int>("Id_Tipo");
                bool resul = Mensajes.Mostrar("¿Borrar esta categoría", Mensajes.Tipo.Confirmacion);

                if (resul)
                {
                    try
                    {
                        string bd = MetodosGestion.db;
                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand command = con.CreateCommand())
                        {
                            command.CommandText = "DELETE FROM TipoArticulo WHERE Id_Tipo = @id";

                            command.Parameters.AddWithValue("@id", id);

                            con.Open();
                            int a = command.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                MessageBox.Show("Error al borrar categoria");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    ActualizarCategorias();
                }
            }
        }

        private void EditarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (tipoArtdata.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)tipoArtdata.SelectedItem;
                int id = dd.Row.Field<int>("Id_Tipo");
                //MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro?", "Confirmacion Edicion", System.Windows.MessageBoxButton.YesNo);
                bool messageBoxResult = Mensajes.Mostrar("¿Borrar esta categoría", Mensajes.Tipo.Confirmacion);

                if (messageBoxResult)
                {
                    try
                    {
                        string bd = MetodosGestion.db;
                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand command = con.CreateCommand())
                        {
                            command.CommandText = "UPDATE TipoArticulo SET Descripcion = @descr WHERE Id_Tipo = @id";

                            command.Parameters.AddWithValue("@descr", nombreTb.Text);
                            command.Parameters.AddWithValue("@id", id);

                            con.Open();
                            int a = command.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                Mensajes.Mostrar("Error al editar categoria", Mensajes.Tipo.Error);

                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    ActualizarCategorias();
                }
            }
        }

        private void AnadirCategoria_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dTipo = (DataRowView)tipoArtdata.SelectedItem;
                //int idT = dTipo.Row.Field<int>("Id_Tipo");
                int id = -1;
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [TipoArticulo] WHERE ([Descripcion] = @tipo)";
                    command.Parameters.AddWithValue("@tipo", nombreTb.Text);
                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id_Tipo"));
                        }
                    }

                    //int existe = (int)command.ExecuteScalar();

                    if (id != -1)
                    {
                        Mensajes.Mostrar("Esta categoria ya existe",Mensajes.Tipo.Info); //MessageBox.Show("Esta categoria ya existe");
                    }
                    else
                    {
                        using (SqlCommand anadir = con.CreateCommand())
                        {

                            anadir.CommandText = "INSERT INTO TipoArticulo (Descripcion) VALUES (@descripcion)";

                            anadir.Parameters.AddWithValue("@descripcion", nombreTb.Text);

                            //con.Open();
                            int a = anadir.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                MessageBox.Show("Categoria ERROR");
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
            ActualizarCategorias();
        }

        private void bRefreshh_Click(object sender, RoutedEventArgs e)
        {
            ActualizarAsignar();
        }

        private void articulosDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (articulosDataGrid.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)articulosDataGrid.SelectedItem;
                String idArt = dato.Row.Field<int>("Id_Articulo").ToString();

                InfoArticulos ia = new InfoArticulos(idArt);
                ia.Show();
            }

        }

        private void Informe_Click(object sender, RoutedEventArgs e)
        {
            if (articulosDataGrid.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)articulosDataGrid.SelectedItem;
                String idArt = dato.Row.Field<int>("Id_Articulo").ToString();
                
                InformeArticulo ina = new InformeArticulo(idArt);
                ina.Show();
            }
        }


    }
}