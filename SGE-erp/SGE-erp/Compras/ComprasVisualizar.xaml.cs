using SGE_erp.Administracion;
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

namespace SGE_erp.Compras
{
    /// <summary>
    /// Lógica de interacción para ComprasVisualizar.xaml
    /// </summary>
    public partial class ComprasVisualizar : UserControl
    {
        public Delegate FiltrarLista;
        public delegate void RefreshList();
        public ComprasVisualizar()
        {
            InitializeComponent();
            //cargarDatos();
        }
        
        public void cargarDatos()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Compra", con);
            dt.Clear();

            comprasDatos.ItemsSource = null;
            da.Fill(dt);

            DataColumn nombreProv = new DataColumn("Nombre Proveedor", typeof(string));
            dt.Columns.Add(nombreProv);
            dt.Columns["Nombre Proveedor"].SetOrdinal(3);

            DataColumn nombreEmp = new DataColumn("Nombre Empleado", typeof(string));
            dt.Columns.Add(nombreEmp);
            dt.Columns["Nombre Empleado"].SetOrdinal(4);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    String idProv= Convert.ToString(row["Id_Proveedor"]);
                    String idEmp = Convert.ToString(row["Id_Empleado"]);
                    DateTime fecha = Convert.ToDateTime(row["FechaCompra"]);
                    //MessageBox.Show(fecha);
                    dt.Rows[i]["FechaCompra"] = fecha.ToShortDateString();


                    using (SqlCommand command = con.CreateCommand())
                    {
                        command.CommandText = "SELECT Nombre FROM [Proveedores] where Id_Proveedor = @id";
                        command.Parameters.AddWithValue("@id", idProv);
                        DataTable dt2 = new DataTable();
                        SqlDataAdapter da2 = new SqlDataAdapter(command);
                        da2.Fill(dt2);
                        DataRow row2 = dt2.Rows[0];
                        String nombreDato = Convert.ToString(row2[0]);
                        dt.Rows[i]["Nombre Proveedor"] = nombreDato;
                    }

                    using (SqlCommand command2 = con.CreateCommand())
                    {
                        command2.CommandText = "SELECT Nombre FROM Empleados WHERE Id_Empleado=@id";
                        command2.Parameters.AddWithValue("@id", idEmp);
                        DataTable dt3 = new DataTable();
                        SqlDataAdapter da3 = new SqlDataAdapter(command2);
                        da3.Fill(dt3);
                        DataRow row3 = dt3.Rows[0];
                        String nombreDatoEmpl = Convert.ToString(row3[0]);
                        dt.Rows[i]["Nombre Empleado"] = nombreDatoEmpl;
                    }
                }
            }

            this.comprasDatos.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void actualizar_Click(object sender, RoutedEventArgs e)
        {
            cargarDatos();
        }

        private void ComprasDatos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dato = (DataRowView)comprasDatos.SelectedItem;
            String idCompra = dato.Row.Field<int>("Id_Compra").ToString();

            Compras_ComprasDetalles ccd = new Compras_ComprasDetalles(idCompra);
            ccd.Show();
        }

        private void facturaV_Click(object sender, RoutedEventArgs e)
        {
            InformeCompras f = new InformeCompras();
            f.Show();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            Compras_FiltroCompra cfc = new Compras_FiltroCompra();
            cfc.ShowDialog();
        }
    }
}
