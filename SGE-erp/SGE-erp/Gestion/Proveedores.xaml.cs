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
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
                DataTable dt = new DataTable(); ;

                ds.Clear();
                da.Fill(dt);
                this.dataGridProveedores.ItemsSource = dt.DefaultView;

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
                p = new ProveedoresEdicion(0);
                RefreshListEvent += new RefreshList(RefreshListView);
                p.Title = "Añadir Proveedor";
                p.Owner = System.Windows.Application.Current.MainWindow;
                p.ActualizarLista = RefreshListEvent;
                p.Show();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                if (dataGridProveedores.SelectedItem != null)
                {
                    DataRowView dd = (DataRowView)dataGridProveedores.SelectedItem;
                    int id = dd.Row.Field<int>("Id_Proveedor");

                    p = new ProveedoresEdicion(id);
                    RefreshListEvent += new RefreshList(RefreshListView); // event initialization
                    p.Title = "Editar Proveedor";
                    p.Owner = System.Windows.Application.Current.MainWindow;
                    p.ActualizarLista = RefreshListEvent; // assigning event to the Delegate
                    p.Show();
                }
            }
        }
    }
}