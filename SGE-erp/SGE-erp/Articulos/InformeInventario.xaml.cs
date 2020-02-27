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
using System.Windows.Shapes;
using SGE_erp.Gestion;
using SGE_erp.Administracion;

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Lógica de interacción para InformeInventario.xaml
    /// </summary>
    public partial class InformeInventario : UserControl
    {
        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;
        private AddInventario ai = null;
        public static Delegate FiltrarLista;

        public delegate void FilterList();
        public event FilterList FilterListEvent;

        public InformeInventario()
        {
            InitializeComponent();
            fechaAntes.SelectedDate = DateTime.Today;
            fechaDespues.SelectedDate = DateTime.Today;

        }

        public void RellenarTabla()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Inventario", con))
            {
                DataTable dt = new DataTable(); ;
                da.Fill(dt);

                dataGridInventario.ItemsSource = dt.DefaultView;
            }
            con.Open();
            con.Close();
        }


        private void AnadirInv_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(ai))
            {
                ai = new AddInventario();
                RefreshListEvent += new RefreshList(RellenarTabla);
                ai.Owner = System.Windows.Application.Current.MainWindow;
                ai.ActualizarLista = RefreshListEvent;
                ai.Show();
            }
        }

        private void InformeInv_Click(object sender, RoutedEventArgs e)
        {
            if(dataGridInventario.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)dataGridInventario.SelectedItem;
                int idInventario = dato.Row.Field<int>("Id");
                InformeInventarioReport informe = InventarioReporte.ObtenerReporteInventario(idInventario);
                VentanaInforme ventanaInforme = new VentanaInforme(informe);
                ventanaInforme.Show();
            }            
        }

        private void ActualizarStock_Click(object sender, RoutedEventArgs e)
        {
            if (Mensajes.Mostrar("¿Estás seguro de actualizar inventario?", Mensajes.Tipo.Confirmacion))
            {
                int[] ids = new int[dataGridInventario.Items.Count];
                int cont = 0;
                foreach (DataRowView row in dataGridInventario.Items)
                {
                    ids[cont++] = int.Parse(row["Id"].ToString());
                }

                int idArticulo;
                int stockContado = 0;

                SqlConnection con = new SqlConnection(MetodosGestion.db);
                using (SqlCommand articulos = con.CreateCommand())
                {
                    articulos.CommandText = "SELECT Id_Articulo FROM Articulos";
                    con.Open();

                    using (SqlDataReader reader = articulos.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idArticulo = Convert.ToInt32(reader[0]);

                            stockContado = 0;
                            for (int i = 0; i < ids.Length; i++)
                            {
                                using (SqlCommand inventario = con.CreateCommand())
                                {
                                    inventario.CommandText = "SELECT UnidadesContadas FROM InventarioArticulos WHERE IdArticulo = @id AND IdInventario = @idI";
                                    inventario.Parameters.AddWithValue("@id", idArticulo);
                                    inventario.Parameters.AddWithValue("@idI", ids[i]);

                                    using (SqlDataReader lector = inventario.ExecuteReader())
                                    {
                                        while (lector.Read())
                                        {
                                            stockContado += Convert.ToInt32(lector[0]);
                                        }
                                    }
                                }
                            }

                            // ACTUALIZAR STOCK
                            using (SqlCommand command = con.CreateCommand())
                            {
                                command.CommandText = "UPDATE Articulos SET Stock = @stock WHERE Id_Articulo = @idArt";

                                command.Parameters.AddWithValue("@idArt", idArticulo);
                                command.Parameters.AddWithValue("@stock", stockContado);

                                int a = command.ExecuteNonQuery();

                                if (a != 0)
                                {
                                    //con.Close();
                                }
                                else
                                {
                                    Mensajes.Mostrar("ERROR al actualizar", Mensajes.Tipo.Error);
                                }
                            }
                        }
                    }

                }
                Mensajes.Mostrar("Stock actualizado correctamente", Mensajes.Tipo.Info);
            }
        }

        private void ActualizarInvent_Click(object sender, RoutedEventArgs e)
        {
            RellenarTabla();
        }

        private void DataGridInventario_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridInventario.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)dataGridInventario.SelectedItem;
                int id = dd.Row.Field<int>("Id");
                DetalleInventario d = new DetalleInventario(id);
                d.ShowDialog();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RellenarTabla();
        }

        private void empleadoC_Loaded(object sender, RoutedEventArgs e)
        {
            this.empleadoC.ItemsSource = null;
            //comboBox1.Items.Clear();
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Empleado, Nombre FROM Empleados ORDER BY Nombre ASC", con);
            DataTable dt = new DataTable();

            da.Fill(dt);
            con.Open();
            con.Close();
            DataRow row = dt.NewRow();
            row["Nombre"] = "TODOS";
            row["Id_Empleado"] = 0;
            dt.Rows.InsertAt(row, 0);

            this.empleadoC.ItemsSource = dt.DefaultView;

            empleadoC.DisplayMemberPath = dt.Columns["Nombre"].ToString();
            empleadoC.SelectedValuePath = dt.Columns["Id_Empleado"].ToString();

            empleadoC.InvalidateArrange();

            empleadoC.SelectedIndex = 0;
        }

        DataView view = null;
        DataTable dt;

        private void btFiltrar_Click(object sender, RoutedEventArgs e)
        {
            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)dataGridInventario.ItemsSource).ToTable();
                dt.TableName = "Inventario";
                view.Table = dt;
            }
            else
            {
                if (empleadoC.SelectedIndex == 0)
                {
                    view.RowFilter = $"Fecha >= '{fechaAntes.SelectedDate}' AND Fecha <= '{fechaDespues.SelectedDate}'";
                }
                else
                {
                    view.RowFilter = $"Fecha >= '{fechaAntes.SelectedDate}' AND Fecha <= '{fechaDespues.SelectedDate}' AND IdEmpleado = {empleadoC.SelectedValue}";
                }

            }
            dt = view.ToTable();
            dataGridInventario.ItemsSource = null;
            dataGridInventario.ItemsSource = dt.DefaultView;
        }
    }
}
