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

namespace SGE_erp.Articulos
{
    public partial class AddInventario : Window
    {
        public Delegate ActualizarLista;

        public AddInventario()
        {
            InitializeComponent();
            RellenarTabla();
        }

        private void RellenarTabla()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Articulo, Nombre, Descripcion FROM Articulos", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataColumn workCol = dt.Columns.Add("StockNuevo", typeof(Int32));
                dataGridInventario.ItemsSource = dt.DefaultView;
            }
            con.Open();
            con.Close();
        }

        private Boolean CheckRelleno()
        {
            DataTable dtaAnadir = ((DataView)dataGridInventario.ItemsSource).ToTable();
            int cont = 0;
            foreach (DataRow row in dtaAnadir.Rows)
            {
                if (row["StockNuevo"].GetType().ToString().Equals("System.DBNull"))
                {
                    //int indexAnadir = dataGridInventario.Items.IndexOf(row);
                    //dtaAnadir.Rows[cont]["Stock"] = Convert.ToInt32(dtaAnadir.Rows[cont]["Stock"]) + stock;
                    //break;
                    return false;
                }
                cont++;
            }
            return true;
        }

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Nombre")
            {
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Id_Articulo")
            {
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Descripcion")
            {
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        private void dataGridInventario_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (CheckRelleno())
            {
                guardarTabla.IsEnabled = true;
            }
            else
            {
                guardarTabla.IsEnabled = false;
            }
        }

        private void dataGridInventario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckRelleno())
            {
                guardarTabla.IsEnabled = true;
            }
            else
            {
                guardarTabla.IsEnabled = false;
            }
        }

        private void dataGridInventario_KeyDown(object sender, KeyEventArgs e)
        {
            if (CheckRelleno())
            {
                guardarTabla.IsEnabled = true;
            }
            else
            {
                guardarTabla.IsEnabled = false;
            }
        }

        private void guardarTabla_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtaAnadir = ((DataView)dataGridInventario.ItemsSource).ToTable();
            string nombre = "";
            int stock = 0;
            decimal pvp = 0;
            decimal precio = 0;
            int idInvent;
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            
            //AÑADIR REGISTRO A INVENTARIO
            using (SqlCommand command = con.CreateCommand())
            {
                command.CommandText = "INSERT INTO Inventario (Fecha, IdEmpleado) OUTPUT INSERTED.Id VALUES (@fecha, @emple)";
                command.Parameters.AddWithValue("@fecha", DateTime.Now);
                command.Parameters.AddWithValue("@emple", MainWindow.idEmpleado);
                con.Open();

                idInvent = (int)command.ExecuteScalar();
                //idInvent = command.ExecuteNonQuery();

                foreach (DataRow row in dtaAnadir.Rows)
                {
                    //AÑADIR REGISTRO A INVENTARIO-ARTICULO
                    using (SqlCommand anadir = con.CreateCommand())
                    {
                        anadir.CommandText = "INSERT INTO InventarioArticulos (IdInventario,IdArticulo,NombreArticulo,UnidadesContadas,UnidadesStock,PrecioVenta,PrecioCompra)" +
                            " VALUES (@idi,@ida,@nombre,@contado,@stock,@venta,@compra)";

                        int idArt = Convert.ToInt32(row["Id_Articulo"]);
                        int contado = Convert.ToInt32(row["StockNuevo"]);
                        
                        // SACAR DATOS DEL ARTICULO
                        using (SqlCommand articulo = con.CreateCommand())
                        {
                            articulo.CommandText = "SELECT Nombre, Descripcion, PVP, Stock FROM Articulos WHERE Articulos.Id_Articulo = @id ";
                            articulo.Parameters.AddWithValue("@id", idArt);
                            using (SqlDataReader reader = articulo.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    nombre = $"{reader[0].ToString()} {reader[1].ToString()}";
                                    stock = Convert.ToInt32(reader[3]);
                                    pvp = Convert.ToDecimal(reader[2]);
                                    //precio = Convert.ToDecimal(reader[4]);
                                }
                            }
                        }

                        anadir.Parameters.AddWithValue("@idi", idInvent);
                        anadir.Parameters.AddWithValue("@ida", idArt);
                        anadir.Parameters.AddWithValue("@nombre", nombre);
                        anadir.Parameters.AddWithValue("@contado", contado);
                        anadir.Parameters.AddWithValue("@stock", stock);
                        anadir.Parameters.AddWithValue("@venta", pvp);
                        if ((pvp - 10) < 0)
                        {
                            anadir.Parameters.AddWithValue("@compra", 0);
                        }
                        else
                        {
                            anadir.Parameters.AddWithValue("@compra", pvp - 10);
                        }

                        anadir.ExecuteNonQuery();
                        Console.WriteLine("GUARDADO");
                    }
                }
                ActualizarLista.DynamicInvoke();
            }
            this.Close();
        }
    }
}
