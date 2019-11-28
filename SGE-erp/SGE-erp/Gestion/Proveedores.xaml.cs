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
using SGE_erp.SetaDataTableAdapters;
using System.IO;
using System.Diagnostics;

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Interaction logic for Proveedores.xaml
    /// </summary>
    public partial class ProveedoresMain : UserControl
    {
        public ProveedoresMain()
        {
            InitializeComponent();
            Actualizar();
        }

        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;
        ProveedoresEdicion p = null;

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
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

                this.dataGridProveedores.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                dataGridProveedores.Columns[0].Visibility = Visibility.Collapsed;
                dataGridProveedores.Columns[1].Visibility = Visibility.Collapsed;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                p = new ProveedoresEdicion(0);
                RefreshListEvent += new RefreshList(Actualizar);
                p.Title = "Añadir Proveedor";
                p.Owner = System.Windows.Application.Current.MainWindow;
                p.ActualizarLista = RefreshListEvent;
                p.Show();
            }
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                if (dataGridProveedores.SelectedItem != null)
                {
                    DataRowView dd = (DataRowView)dataGridProveedores.SelectedItem;
                    int id = dd.Row.Field<int>("Id_Proveedor");


                    p = new ProveedoresEdicion(id);
                    RefreshListEvent += new RefreshList(Actualizar);
                    p.Title = "Editar Proveedor";
                    p.Owner = System.Windows.Application.Current.MainWindow;
                    p.ActualizarLista = RefreshListEvent;
                    p.Show();
                }
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProveedores.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)dataGridProveedores.SelectedItem;
                int id = dd.Row.Field<int>("Id_Proveedor");
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro?", "Confirmacion Borrado", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        string bd = MetodosGestion.db;
                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand command = con.CreateCommand())
                        {
                            command.CommandText = "DELETE FROM Proveedores WHERE Id_Proveedor = @id";

                            command.Parameters.AddWithValue("@id", id);

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
                if (dataGridProveedores.SelectedItem != null)
                {
                    int id = -1;

                    p = new ProveedoresEdicion(id);
                    RefreshListEvent += new RefreshList(Filtrar);
                    p.Title = "Buscar Proveedor";
                    p.Owner = System.Windows.Application.Current.MainWindow;
                    p.ActualizarLista = RefreshListEvent;
                    p.Show();
                }
            }
        }

        private void Filtrar()
        {

            DataRowView dd = (DataRowView)dataGridProveedores.SelectedItem;
            int id = dd.Row.Field<int>("Id_Proveedor");
            DataTable dt = (DataTable)dataGridProveedores.ItemsSource;
            DataView dv = (DataView)dataGridProveedores.ItemsSource;
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn column in dv.Table.Columns)
            {
                sb.AppendFormat("[{0}] Like '%{1}%' OR ", column.ColumnName, "FilterString");
            }
            sb.Remove(sb.Length - 3, 3);
            dv.RowFilter = sb.ToString();
            dataGridProveedores.ItemsSource = dv;
            dataGridProveedores.Items.Refresh();
        }

        private void AccesoVentana(params string[] nombres)
        {

        }
    }
}