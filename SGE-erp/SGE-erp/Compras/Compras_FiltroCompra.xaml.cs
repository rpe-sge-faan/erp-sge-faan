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

namespace SGE_erp.Compras
{
    /// <summary>
    /// Lógica de interacción para Compras_FiltroCompra.xaml
    /// </summary>
    public partial class Compras_FiltroCompra : Window
    {
        public static String fechaDesde = "01/01/2000";
        public static String fechaHasta = DateTime.Today.ToShortDateString();
        public static String idProv;
        public static String swhere;
        public static String sorder;
        public Compras_FiltroCompra()
        {
            InitializeComponent();
            string dateInput = "01/01/2000";
            DateTime parsedDate = DateTime.Parse(dateInput);
            dp_FDesde.SelectedDate = parsedDate;
            dp_FHasta.SelectedDate = DateTime.Today;           
        }

        private void Btn_Aplicar_Click(object sender, RoutedEventArgs e)
        {
            String sproveedor;
            fechaDesde = Convert.ToDateTime(Convert.ToString(dp_FDesde )).ToShortDateString();
            fechaHasta = Convert.ToDateTime(Convert.ToString(dp_FHasta)).ToShortDateString();
            if(cb_ProvDesde.SelectedValue == null)
            {
                sproveedor = " ";
            }
            else
            {
                idProv = cb_ProvDesde.SelectedValue.ToString();
                sproveedor = " AND compra.Id_Proveedor = " + cb_ProvDesde.SelectedValue.ToString();
            }


            swhere = "where convert(date,Compra.FechaCompra,103) >= convert(date,'" + fechaDesde + "',103)and convert(date,Compra.FechaCompra,103) <= convert(date,'" +
                fechaHasta + "',103) " + sproveedor;
            sorder = " order by convert(date,Compra.FechaCompra,103)DESC";
            this.Close();
        }

        private DataTable Cargar()
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
            DataTable dt = new DataTable();

            //ds.Clear();
            da.Fill(dt);

            return dt;
        }

        private void Cb_ProvDesde_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(MetodosGestion.db);
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Proveedores", con))
            {
                DataTable dt = new DataTable();

                da.Fill(dt);

                this.cb_ProvDesde.ItemsSource = dt.DefaultView;

                cb_ProvDesde.DisplayMemberPath = dt.Columns["Nombre"].ToString();
                cb_ProvDesde.SelectedValuePath = dt.Columns["Id_Proveedor"].ToString();
            }
            con.Open();
            con.Close();
        }
    }
}
