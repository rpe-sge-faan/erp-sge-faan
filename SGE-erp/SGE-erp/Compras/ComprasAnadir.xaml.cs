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


    public partial class ComprasAnadir : UserControl
    {
        //CREACION Y ESTRUCTURACION DEL CARRITO
        public static DataTable carritoCompra = new DataTable();
        DataColumn idElementoC = new DataColumn("Elemento", typeof(string));
        DataColumn nombreArticuloc = new DataColumn("Articulo", typeof(string));
        DataColumn cantidadc = new DataColumn("Cantidad", typeof(string));
        DataColumn precioElementoc = new DataColumn("Precio Articulo", typeof(string));
        DataColumn precioTotalElementoc = new DataColumn("Precio Total", typeof(string));
        Compras_Carrito cc = null;
        int cont;

        public static String idProveedorCompra;

        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;

        public ComprasAnadir()
        {
            InitializeComponent();
            cont = 0;
        }

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Proveedores]", con);
                DataTable dt = new DataTable(); ;

                //ds.Clear();
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

                con.Open();
                con.Close();

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

            if (proveedores.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)proveedores.SelectedItem;
                String idProv = dato.Row.Field<int>("Id_Proveedor").ToString();

                try
                {
                    SqlConnection con = new SqlConnection(MetodosGestion.db);
                    //DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProveedorArticulo WHERE Id_Proveedor='" + idProv + "'", con);
                    DataTable dt = new DataTable();

                    //ds.Clear();
                    da.Fill(dt);
                    DataTable dt2 = null;

                    DataColumn nombre = new DataColumn("Nombre Articulo", typeof(string));
                    dt.Columns.Add(nombre);
                    dt.Columns["Nombre Articulo"].SetOrdinal(0);

                    DataColumn descripcion = new DataColumn("Descripción", typeof(string));
                    dt.Columns.Add(descripcion);
                    dt.Columns["Descripción"].SetOrdinal(1);

                    DataColumn pvp = new DataColumn("PVP", typeof(string));
                    dt.Columns.Add(pvp);
                    dt.Columns["PVP"].SetOrdinal(6);

                    DataColumn stock = new DataColumn("Stock", typeof(string));
                    dt.Columns.Add(stock);
                    dt.Columns["Stock"].SetOrdinal(7);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                            idArticulo = Convert.ToString(row["Id_Articulo"]);
                            //MessageBox.Show(idArticulo);
                            SqlDataAdapter da2 = new SqlDataAdapter("SELECT Nombre,Descripcion,PVP,Stock FROM [Articulos] WHERE Id_Articulo='"
                                + idArticulo + "'", con);
                            dt2 = new DataTable();
                            da2.Fill(dt2);

                            DataRow row2 = dt2.Rows[0];
                            String nombreDato = Convert.ToString(row2[0]);
                            String descripcionDato = Convert.ToString(row2[1]);
                            String pvpDato = Convert.ToString(row2[2]);
                            String stockDato = Convert.ToString(row2[3]);

                            dt.Rows[i]["Nombre Articulo"] = nombreDato;
                            dt.Rows[i]["Descripción"] = descripcionDato;
                            dt.Rows[i]["PVP"] = pvpDato;
                            dt.Rows[i]["Stock"] = stockDato;
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
            if (articulos.SelectedItem != null)
            {
                DataRowView dato = (DataRowView)articulos.SelectedItem;
                String idArt = dato.Row.Field<int>("Id_Articulo").ToString();

                Compras_ArticulosDetalles cad = new Compras_ArticulosDetalles(idArt);
                cad.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
            if (cont == 0)
            {
                carritoCompra.Columns.Add(idElementoC);
                carritoCompra.Columns.Add(nombreArticuloc);
                carritoCompra.Columns.Add(cantidadc);
                carritoCompra.Columns.Add(precioElementoc);
                carritoCompra.Columns.Add(precioTotalElementoc);
            }
            cont++;
        }

        private void BtnComparar_Click(object sender, RoutedEventArgs e)
        {
            if (articulos.SelectedItem != null)
            {

                DataRowView dato = (DataRowView)articulos.SelectedItem;
                String idArt = dato.Row.Field<int>("Id_Articulo").ToString();

                Compras_CompararProveedor ccp = new Compras_CompararProveedor(idArt);
                ccp.ShowDialog();
            }
        }

        private void BtnCompararAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (articulos.SelectedItem != null)
            {
                String idProveedorActual = "";
                DataRowView tProveedor = (DataRowView)proveedores.SelectedItem;
                if (carritoCompra.Rows.Count == 0)
                {
                    idProveedorCompra = tProveedor.Row.Field<int>("Id_Proveedor").ToString();
                }
                idProveedorActual = tProveedor.Row.Field<int>("Id_Proveedor").ToString();
                if (idProveedorCompra.Equals(idProveedorActual))
                {
                    int cantidad = pedirCantidad();
                    if (cantidad != 0)
                    {
                        DataRowView tArticulo = (DataRowView)articulos.SelectedItem;

                        String idElemento = tArticulo.Row.Field<int>("Id_Elemento").ToString();
                        String precioElemento = tArticulo.Row.Field<decimal>("PrecioCompra").ToString();
                        double precioTotalElemento = double.Parse(precioElemento) * cantidad;
                        String nombreArticulo = tArticulo.Row.Field<String>("Nombre Articulo");

                        DataRow fila = carritoCompra.NewRow();
                        fila[0] = idElemento;
                        fila[1] = nombreArticulo;
                        fila[2] = cantidad;
                        fila[3] = precioElemento;
                        fila[4] = precioTotalElemento;
                        carritoCompra.Rows.Add(fila);
                    }
                }
                else
                {
                    MessageBox.Show("No puede comprar articulos de distintos proveedores. Realice otra compra o vacie el carrito");
                }
            }
        }

        private int pedirCantidad()
        {
            int cantidad = 0;
            Compras_PedirDatos cpd = new Compras_PedirDatos();
            cpd.ShowDialog();
            cantidad = Compras_PedirDatos.cantidadRecogida;
            return cantidad;
        }

        private void BtnCompararVerCarrito_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(cc))
            {
                cc = new Compras_Carrito();
                RefreshListEvent += new RefreshList(guardarCompra);
                cc.Comprar = RefreshListEvent;
                cc.Show();
            }
        }

        private double precioFinal()
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.Name == "Carrito")
                {
                    ((Compras_Carrito)item).calPrecioFinal();
                    double precio = ((Compras_Carrito)item).precioFinal;

                    return precio;
                }
            }
            return 0.0;
        }

        public void guardarCompra()
        {
            //Compras_Carrito cc = new Compras_Carrito();
            //cc.calPrecioFinal();
            String idEmpleado = "1";
            //double precioTotal = cc.precioFinal;
            double precioTotal = precioFinal();


            if (carritoCompra.Rows.Count > 0)
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                con.Open();
                SqlCommand da = new SqlCommand(@"INSERT INTO Compra OUTPUT INSERTED.Id_Compra VALUES("
                    + int.Parse(idProveedorCompra) + "," + idEmpleado + ",'" + DateTime.Today + "'," + precioTotal + ");", con);
                da.ExecuteNonQuery();

                SqlConnection con3 = new SqlConnection(MetodosGestion.db);
                SqlDataAdapter da3 = new SqlDataAdapter("SELECT Id_Compra FROM Compra ORDER BY Id_Compra DESC", con3);
                DataTable dt2 = new DataTable();
                da3.Fill(dt2);
                DataRow row2 = dt2.Rows[0];
                String idcompra = Convert.ToString(row2[0]);


                for (int i = 0; i < carritoCompra.Rows.Count; i++)
                {
                    DataRow datos = carritoCompra.Rows[i];
                    SqlConnection con2 = new SqlConnection(MetodosGestion.db);
                    con2.Open();
                    SqlCommand da2 = new SqlCommand(@"INSERT INTO CompraArticulos VALUES("
                        + idcompra + "," + Convert.ToInt32(datos[0]) + "," + Convert.ToInt32(datos[2]) + ");", con2);
                    da2.ExecuteNonQuery();

                    //SqlCommand upgrade = new SqlCommand(@"UPDATE Articulos SET Stock=Stock+" + Convert.ToInt32(datos[2])
                    //    + " FROM Articulos INNER JOIN ProveedorArticulo ON Articulos.Id_Articulo=ProveedorArticulo.Id_Articulo" +
                    //    " WHERE ProveedorArticulo.Id_Elemento=" + Convert.ToInt32(datos[0]) + ";", con2);
                    //upgrade.ExecuteNonQuery();

                    con2.Close();
                }
                MessageBox.Show("Guardado.");
                int j = carritoCompra.Rows.Count;
                for (int i = 0; i < j; i++)
                {
                    carritoCompra.Rows.RemoveAt(0);
                }
                con.Close();
            }
        }

        private void BtnCompararFinalizarCompra_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(cc))
            {
                cc = new Compras_Carrito();
                RefreshListEvent += new RefreshList(guardarCompra);
                cc.Comprar = RefreshListEvent;
                cc.Show();
            }

        }
    }
}
