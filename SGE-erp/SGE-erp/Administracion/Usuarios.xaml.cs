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

namespace SGE_erp.Administracion
{
    /// <summary>
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios : UserControl
    {
        public Usuarios()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Empleados]", con);
                DataTable dt = new DataTable(); ;

                // ds.Clear();
                da.Fill(dt);
                this.dataGridUsuarios.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                dataGridUsuarios.Columns[0].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[1].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[2].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[3].Visibility = Visibility.Collapsed;


                dataGridUsuarios.Columns[5].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[6].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[7].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[8].Visibility = Visibility.Collapsed;
                dataGridUsuarios.Columns[9].Visibility = Visibility.Collapsed;

            }
            catch
            {
                return;
            }
        }

    }
}
