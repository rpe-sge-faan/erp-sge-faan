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
    /// Interaction logic for ClientesMain.xaml
    /// </summary>
    public partial class ClientesMain : UserControl
    {
        public ClientesMain()
        {
            InitializeComponent();
            //Actualizar();
        }

        public static Delegate FiltrarLista;
        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;

        public delegate void FilterList();
        public event FilterList FilterListEvent;

        ClientesEdicion p = null;

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Clientes]", con);
                DataTable dt = new DataTable(); ;

                // ds.Clear();
                da.Fill(dt);

                DataColumn tipoPro = new DataColumn("TipoP", typeof(string));
                dt.Columns.Add(tipoPro);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Tipo"].Equals(1))
                    {
                        dt.Rows[i]["TipoP"] = "Particular";
                    }
                    else
                    {
                        dt.Rows[i]["TipoP"] = "Empresa";
                    }
                }

                dt.Columns["TipoP"].SetOrdinal(3);

                this.dataGridClientes.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                dataGridClientes.Columns[0].Visibility = Visibility.Collapsed;
                dataGridClientes.Columns[1].Visibility = Visibility.Collapsed;
            }
            catch
            {
                return;
            }
        }

        private void SetColumnsOrder(DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                p = new ClientesEdicion(0);
                RefreshListEvent += new RefreshList(Actualizar);
                p.Title = "Añadir Cliente";
                p.Owner = System.Windows.Application.Current.MainWindow;
                p.ActualizarLista = RefreshListEvent;
                p.Show();
            }
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        DataView view = null;
        DataTable dt;
        public void Filtrar()
        {
            List<String> nombres = AccesoVentana();

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)dataGridClientes.ItemsSource).ToTable();
                dt.TableName = "Clientes";
                view.Table = dt;
            }

            view.RowFilter = $"Nombre LIKE '%{nombres[0]}%' AND NIF LIKE '%{nombres[5]}%' AND Telefono LIKE '%{nombres[1]}%' " +
                $"AND Email LIKE '%{nombres[2]}%' AND Direccion LIKE '%{nombres[4]}%' AND PersonaContacto LIKE '%{nombres[3]}%' " +
                $"AND TipoP LIKE '%{nombres[6]}%'";

            //view.Sort = "CompanyName DESC";
            dt = view.ToTable();
            dataGridClientes.ItemsSource = null;
            dataGridClientes.ItemsSource = dt.DefaultView;
            dataGridClientes.Columns[0].Visibility = Visibility.Collapsed;
            dataGridClientes.Columns[1].Visibility = Visibility.Collapsed;

        }
        // params string[] nombres

        private List<String> AccesoVentana()
        {
            List<String> nombres = new List<String>();
            foreach (Window item in Application.Current.Windows)
            {
                //  0        1         2       3       4         5    6    
                // NOMBRE, TELEFONO, EMAIL, CONTACTO, DIRECCION, NIF, TIPO
                if (item.Name == "EdicionClientes")
                {
                    ((ClientesEdicion)item).personaContactoTextBox.IsEnabled = true;
                    int tipo = ((ClientesEdicion)item).tipoComboBox.SelectedIndex;
                    String t;
                    if (tipo == 0)
                    {
                        t = "Particular";
                    }
                    else
                    {
                        t = "Empresa";
                    }

                    String[] nombresArray = {
                        ((ClientesEdicion)item).nombreTextBox.Text,
                        ((ClientesEdicion)item).telefonoTextBox.Text,
                        ((ClientesEdicion)item).emailTextBox.Text,
                        ((ClientesEdicion)item).personaContactoTextBox.Text,
                        ((ClientesEdicion)item).direccionTextBox.Text,
                        ((ClientesEdicion)item).nifTextBox.Text,
                        t
                    };
                    nombres.AddRange(nombresArray);
                }
            }
            return nombres;
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                if (dataGridClientes.SelectedItem != null)
                {
                    DataRowView dd = (DataRowView)dataGridClientes.SelectedItem;
                    int id = dd.Row.Field<int>("Id_Cliente");


                    p = new ClientesEdicion(id);
                    RefreshListEvent += new RefreshList(Actualizar);
                    p.Title = "Editar Cliente";
                    p.Owner = System.Windows.Application.Current.MainWindow;
                    p.ActualizarLista = RefreshListEvent;
                    p.Show();
                }
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridClientes.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)dataGridClientes.SelectedItem;
                int id = dd.Row.Field<int>("Id_Cliente");
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro?", "Confirmacion Borrado", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        string bd = MetodosGestion.db;
                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand command = con.CreateCommand())
                        {
                            command.CommandText = "DELETE FROM Clientes WHERE Id_Cliente = @id";

                            command.Parameters.AddWithValue("@id", id);

                            con.Open();
                            int a = command.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                MessageBox.Show("Cliente ERROR al borrar");
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
            if (!MetodosGestion.IsOpen(p))
            {
                FilterListEvent += new FilterList(Filtrar);
                p = new ClientesEdicion(-1);
                p.Title = "Buscar Cliente";
                p.Owner = Application.Current.MainWindow;
                p.FiltrarLista = FilterListEvent;
                p.Show();
            }
            else
            {
                Filtrar();
            }
        }
    }
}
