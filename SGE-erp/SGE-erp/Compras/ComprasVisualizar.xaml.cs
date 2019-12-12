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
            cargarDatos();
        }

        public void cargarDatos()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Compra", con);
            DataTable dt = new DataTable(); ;

            da.Fill(dt);

            this.comprasDatos.ItemsSource = dt.DefaultView;
        }

        private void actualizar_Click(object sender, RoutedEventArgs e)
        {
            cargarDatos();
        }
    }
}
