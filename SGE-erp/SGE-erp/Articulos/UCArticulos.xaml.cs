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
using SGE_erp;
using SGE_erp.Gestion;
using SGE_erp.SetaDataTableAdapters;

namespace SGE_erp.Articulos
{

    public partial class UCArticulos : UserControl
    {
        public UCArticulos()
        {
            InitializeComponent();
            Actualizar();
        }

        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;
        EditaArticulos a = null;

        private void OcultarColumnas()
        {
            //articulosDataGrid.Columns[0].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[1].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[4].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[5].Visibility = Visibility.Collapsed;
            articulosDataGrid.Columns[7].Visibility = Visibility.Collapsed;
        }

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Articulos, TipoArticulo, Iva WHERE Articulos.Id_Iva = Iva.Id_Iva AND Articulos.TipoArticulo = TipoArticulo.Id_Tipo", con);
                DataTable dt = new DataTable(); ;
                //ds.Clear();
                da.Fill(dt);

                this.articulosDataGrid.ItemsSource = dt.DefaultView;
                con.Open();
                con.Close();
                articulosDataGrid.Columns[6].Header = "Categoría";

                OcultarColumnas();
            }
            catch
            {
                return;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(a))
            {
                a = new EditaArticulos(0);
                RefreshListEvent += new RefreshList(Actualizar);
                a.Title = "Añadir Artículo";
                a.Owner = System.Windows.Application.Current.MainWindow;
                a.ActualizarLista = RefreshListEvent;
                a.Show();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(a))
            {
                if (articulosDataGrid.SelectedItem != null)
                {
                    DataRowView dd = (DataRowView)articulosDataGrid.SelectedItem;
                    int id = dd.Row.Field<int>("Id_Articulo");

                    a = new EditaArticulos(id);
                    RefreshListEvent += new RefreshList(Actualizar);
                    a.Title = "Editar Artículo";
                    a.Owner = System.Windows.Application.Current.MainWindow;
                    a.ActualizarLista = RefreshListEvent;
                    a.Show();
                }
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            if (articulosDataGrid.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)articulosDataGrid.SelectedItem;
                int id = dd.Row.Field<int>("Id_Articulo");
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Estás seguro?", "Confirmacion Borrado", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        string bd = MetodosGestion.db;
                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand command = con.CreateCommand())
                        {
                            command.CommandText = "DELETE FROM Articulos WHERE Id_Articulo = @id";

                            command.Parameters.AddWithValue("@id", id);

                            con.Open();
                            int a = command.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                MessageBox.Show("Error al borrar articulo");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Actualizar();
                }
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(a))
            {
                a = new EditaArticulos(-1);
                RefreshListEvent += new RefreshList(Filtrar);
                a.Title = "Buscar Artículo";
                a.Owner = Application.Current.MainWindow;
                a.ActualizarLista = RefreshListEvent;
                a.Show();
            }
        }

        DataView view = null;
        DataTable dt;
        private void Filtrar()
        {
            List<String> nombres = AccesoVentana();
            String[] campos = { "Nombre", "Descripcion", "Categoria", "PorcentajeIva"};

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)articulosDataGrid.ItemsSource).ToTable();
                dt.TableName = "Articulos";
                view.Table = dt;
            }

            view.RowFilter = $"Nombre LIKE '%{nombres[0]}%' AND NIF LIKE '%{nombres[5]}%' AND Telefono LIKE '%{nombres[1]}%' " +
                $"AND Email LIKE '%{nombres[2]}%' AND Direccion LIKE '%{nombres[4]}%' AND Persona_Contacto LIKE '%{nombres[3]}%' " +
                $"AND Tipo = '{nombres[6]}'";

            //view.Sort = "CompanyName DESC";
            dt = view.ToTable();
            articulosDataGrid.ItemsSource = null;
            articulosDataGrid.ItemsSource = dt.DefaultView;

            OcultarColumnas();
        }

        public List<String> AccesoVentana()
        {
            List<String> nombres = new List<String>();
            foreach (Window item in Application.Current.Windows)
            {
                if (item.Name == "EditarArticulos")
                {
                    String[] nombresArray = {
                        //((EditaArticulos)item).nombreTextBox.Text,
                        //((EditaArticulos)item).telefonoTextBox.Text,
                        //((EditaArticulos)item).emailTextBox.Text,
                        //((EditaArticulos)item).personaContactoTextBox.Text
                    };
                    nombres.AddRange(nombresArray);
                }
            }
            return nombres;
        }
    }
}


