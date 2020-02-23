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
    /// <summary>
    /// Lógica de interacción para DetalleInventario.xaml
    /// </summary>
    public partial class DetalleInventario : Window
    {
        public DetalleInventario(int id)
        {
            InitializeComponent();

            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM InventarioArticulos WHERE IdInventario= " +id, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                detalles.ItemsSource = dt.DefaultView;
            }
            con.Open();
            con.Close();

        }
    }
}
