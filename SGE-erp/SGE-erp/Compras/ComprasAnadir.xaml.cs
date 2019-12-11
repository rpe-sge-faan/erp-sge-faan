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
    public partial class ComprasAnadir : UserControl
    {
        public static String direccionbbdd = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Datos.mdf;Integrated Security=True";

        public ComprasAnadir()
        {
            InitializeComponent();
            Actualizar();
        }

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(direccionbbdd);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
                DataTable dt = new DataTable(); ;

                da.Fill(dt);

                DataColumn tipoProv = new DataColumn("Tipo Proveedor", typeof(string));
                dt.Columns.Add(tipoProv);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Tipo"].Equals(1))
                    {
                        dt.Rows[i]["Tipo Proveedor"] = "Particular";
                    }
                    else
                    {
                        dt.Rows[i]["Tipo Proveedor"] = "Empresa";
                    }
                }

                dt.Columns["Tipo Proveedor"].SetOrdinal(3);

                this.proveedores.ItemsSource = dt.DefaultView;                

                this.proveedores.Columns[0].Visibility = Visibility.Collapsed;
                this.proveedores.Columns[1].Visibility = Visibility.Collapsed;
            }
            catch
            {
                return;
            }
        }

        private void buscarArtProv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            String idArticulo;
            String nombreDato;
            String descripcionDato;
            if(proveedores.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)proveedores.SelectedItem;
                String idProv = dato.Row.Field<int>("Id_Proveedor").ToString();

                try
                {
                    SqlConnection con = new SqlConnection(direccionbbdd);
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProveedorArticulo WHERE Id_Proveedor='" + idProv + "'", con);
                    DataTable dt = new DataTable();

                    da.Fill(dt);
                    DataTable dt2 = null;

                    DataColumn nombre = new DataColumn("Nombre Articulo", typeof(string));
                    dt.Columns.Add(nombre);
                    dt.Columns["Nombre Articulo"].SetOrdinal(0);

                    DataColumn descripcion = new DataColumn("Descripción", typeof(string));
                    dt.Columns.Add(descripcion);
                    dt.Columns["Descripción"].SetOrdinal(1);

                    if (dt.Rows.Count > 0)
                    {                        
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                            idArticulo = Convert.ToString(row["Id_Articulo"]);
                            //MessageBox.Show(idArticulo);
                            SqlDataAdapter da2 = new SqlDataAdapter("SELECT Nombre,Descripcion FROM [Articulos] WHERE Id_Articulo='" + idArticulo + "'", con);
                            dt2 = new DataTable();
                            da2.Fill(dt2);                 

                            DataRow row2 = dt2.Rows[0];
                            nombreDato = Convert.ToString(row2[0]);
                            dt.Rows[i]["Nombre Articulo"] = nombreDato;
                            descripcionDato = Convert.ToString(row2[1]);
                            dt.Rows[i]["Descripción"] = descripcionDato;
                        }                        
                    }                    

                    this.articulos.ItemsSource = dt.DefaultView;
                    this.articulos.Columns[2].Visibility = Visibility.Collapsed;
                    this.articulos.Columns[3].Visibility = Visibility.Collapsed;
                    this.articulos.Columns[4].Visibility = Visibility.Collapsed;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Articulos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(articulos.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)articulos.SelectedItem;
                String idArt = dato.Row.Field<int>("Id_Articulo").ToString();
                Compras_ArticulosDetalles cad = new Compras_ArticulosDetalles();
                cad.cargarDatos(idArt);
                cad.ShowDialog();
            }            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }
    }
}
