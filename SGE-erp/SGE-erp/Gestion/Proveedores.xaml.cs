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
                p = new ProveedoresEdicion(0);
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

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                if (proveedoresListView.SelectedItem != null)
                {

                    string hola = proveedoresListView.SelectedItem.ToString();
                    MessageBox.Show(hola);

                    foreach (var item in this.proveedoresListView.SelectedItems)
                    {
                        // Have a look at it's type. It is our class!
                        Console.WriteLine("Type: " + item.GetType());
                        // We cast to the desired type
                        string ri = item as string;
                        // And we got our instance in our type and are able to work with it.
                        Console.WriteLine("RineItem: " + ri.Name + ", " + ri.Id);

                        // Let's modify it a little
                        ri.Name += ri.Name;
                        // Don't forget to Refresh the items, to see the new values on screen
                        this.ListBox1.Items.Refresh();
                    }

                    //p = new ProveedoresEdicion(1);
                    //RefreshListEvent += new RefreshList(RefreshListView); // event initialization
                    //p.Title = "Editar Proveedor";
                    //p.Owner = System.Windows.Application.Current.MainWindow;
                    //p.ActualizarLista = RefreshListEvent; // assigning event to the Delegate
                    //p.Show();
                }
            }
        }
    }
}
