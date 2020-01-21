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
    /// Lógica de interacción para FacturasVentas.xaml
    /// </summary>
    public partial class FacturasVentas : Window
    {
        public FacturasVentas()
        {
            InitializeComponent();
        }
       

        private void Actualizar(string idVentas)
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Ventas, FechaVentas, Nombre, NIF, SUM(PrecioTotal - PorcenajeIva), PorcenajeIva, PrecioTotal" +
                                                    "FROM Ventas, Clientes, Iva " +
                                                    "WHERE Id_Articulo=" + idVentas + " order by FechaVentas, Id_Ventas", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            this.ventasDataGrid.ItemsSource = dt.DefaultView;
            con.Open();
            con.Close();
            
        }
    }
}
