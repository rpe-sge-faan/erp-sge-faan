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

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Interaction logic for ProveedoresMain.xaml
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
        private void RefreshListView()
        {
            Actualizar();
        }


        private void Actualizar()
        {
            try
            {
                string bd = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\repos\erp-sge-faan\SGE-erp\SGE-erp\DeBaseDatos.mdf;Integrated Security=True";
                SqlConnection con = new SqlConnection(bd);
                DataSet ds;
                SqlDataAdapter da;
                SqlCommandBuilder scb;
                DataTable dt;

                da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
                ds = new DataSet();
                dt = new DataTable();
                ds.Clear();
                da.Fill(dt);
                this.proveedoresListView.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();
            }
            catch
            {
                return;
            }
        }

        ProveedoresEdicion p = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Actualizar();
        }

        private void anadirPro_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                p = new ProveedoresEdicion();
                RefreshListEvent += new RefreshList(RefreshListView); // event initialization
                p.Title = "Añadir Proveedor";
                p.Owner = System.Windows.Application.Current.MainWindow;             
                p.ActualizarLista = RefreshListEvent; // assigning event to the Delegate
                p.Show();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }
    }
}
