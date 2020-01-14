﻿using SGE_erp.Gestion;
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

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Lógica de interacción para InfoArticulos.xaml
    /// </summary>
    public partial class InfoArticulos : Window
    {
        public InfoArticulos(String idArticulo)
        {
            InitializeComponent();
            Actualizar(idArticulo);

        }

        public static double[] valoresStock;
        private void Actualizar(string idArticulo)
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Movimientos WHERE Id_Articulo=" + idArticulo + " order by Fecha;", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                this.articuloDataGrid.ItemsSource = dt.DefaultView;
                con.Open();
                con.Close();

                valoresStock = new double[dt.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    valoresStock[i++] = double.Parse(dr[6].ToString());
                }

            }
            catch
            {
                return;
            }
        }

    }
}
