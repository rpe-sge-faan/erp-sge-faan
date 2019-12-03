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
using SGE_erp.Gestion;
using SGE_erp.SetaDataTableAdapters;

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Interaction logic for UCArticulos.xaml
    /// </summary>
    public partial class UCArticulos : UserControl
    {
        public UCArticulos()
        {
            InitializeComponent();
            Actualizar();
            //SetaData sd = new SetaData();
            //ArticulosTableAdapter adapter = new ArticulosTableAdapter();
            //adapter.Fill(sd.Articulos);
            //articulosListView.ItemsSource = sd.Articulos.DefaultView;

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
                //string bd = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Datos.mdf;Integrated Security=True";
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Articulos]", con);
                DataTable dt = new DataTable(); ;

                ds.Clear();
                da.Fill(dt);
                this.articulosDataGrid.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();
            }
            catch
            {
                return;
            }
        }

        EditaArticulos ea = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Actualizar();
        }

        private void anadirArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (articulosDataGrid.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)articulosDataGrid.SelectedItem;
                int id = dd.Row.Field<int>("Id_Articulo");

                ea = new EditaArticulos();
                RefreshListEvent += new RefreshList(RefreshListView); // event initialization
                ea.Title = "Añadir Articulo";
                ea.Owner = System.Windows.Application.Current.MainWindow;
                ea.ActualizarLista = RefreshListEvent; // assigning event to the Delegate
                ea.Show();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        /*private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // No cargue datos en tiempo de diseño.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Cargue los datos aquí y asigne el resultado a CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }


            //DataTable employeeData = CreateDataTable();


            //articulosListView.ItemsSource = query.ToList();


        }*/


        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditaArticulos ea = new EditaArticulos();
            ea.ShowDialog();
        }



        /* private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
           // MessageBox.Show("Hola");
        } */
    }
}


