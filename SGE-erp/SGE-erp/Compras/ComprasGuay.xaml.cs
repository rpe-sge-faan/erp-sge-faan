﻿using System;
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
    /// Lógica de interacción para ComprasGuay.xaml
    /// </summary>
    public partial class ComprasGuay : UserControl
    {
        public ComprasGuay()
        {
            InitializeComponent();
            Actualizar();
        }

        private void Actualizar()
        {
            try
            {
                string bd = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|DeBaseDatos.mdf;Integrated Security=True";
                SqlConnection con = new SqlConnection(bd);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Compra]", con);
                DataTable dt = new DataTable(); ;

                ds.Clear();
                da.Fill(dt);
                this.comprasDatos.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();
            }
            catch
            {
                return;
            }
        }

        private void ComprasDatos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(comprasDatos.SelectedItem != null)
            {
                //String dato = comprasDatos.SelectedItems[0].GetTyp;
                //int dato = (int)comprasDatos.CurrentRow.Cells["NombreColumna"].Value;
                DataRowView dato = (DataRowView)comprasDatos.SelectedItem;
                MessageBox.Show((dato.Row.Field<int>("Id_Compra")).ToString());
                MessageBox.Show(dato.Row.Field<DateTime>("FechaCompra").ToString());
            }
        }
    }
}