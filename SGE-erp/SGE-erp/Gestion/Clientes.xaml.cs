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

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Interaction logic for ClientesMain.xaml
    /// </summary>
    public partial class ClientesMain : UserControl
    {
        public ClientesMain()
        {
            InitializeComponent();
            
        }

        ClientesEdicion c = null;

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

            // No cargue datos en tiempo de diseño.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Cargue los datos aquí y asigne el resultado a CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
                DataTable dt = new DataTable(); ;

                ds.Clear();
                da.Fill(dt);
                this.dataGridClientes.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();
            }
            catch
            {
                return;
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            if (!MetodosGestion.IsOpen(c))
            {
                c = new ClientesEdicion();
                c.Owner = System.Windows.Application.Current.MainWindow;
                c.Show();
            }                  
                    
        }


    }
}
