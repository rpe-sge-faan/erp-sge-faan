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
using System.Windows.Shapes;

namespace SGE_erp.Venta
{
    /// <summary>
    /// Lógica de interacción para InformeVenta.xaml
    /// </summary>
    public partial class InformeVenta : Window
    {
        public InformeVenta(string idVentas)
        {
            InitializeComponent();
            Actualizar(idVentas);
        }

        private void Actualizar(string idVentas)
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("SELECT Ventas.Id_Ventas, Ventas.FechaVentas, Clientes.Nombre, Clientes.NIF, Iva.Porcentaje_Iva, Ventas.PrecioTotal" +
                                                    "FROM Ventas, Clientes, Iva " +
                                                    "WHERE Id_Ventas ='" + idVentas + "'", con);
            DataTable dt = new DataTable();
            ds.Clear();
            da.Fill(dt);

            this.ventasDataGrid.ItemsSource = dt.DefaultView;
            con.Open();
            con.Close();

        }
    }
}
