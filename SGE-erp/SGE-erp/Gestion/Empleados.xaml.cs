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
using SGE_erp.Gestion;

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : UserControl
    {
        public Empleados()
        {
            InitializeComponent();
            //Actualizar();
        }

        //public static Delegate FiltrarLista;
        public delegate void FilterList();
        public event FilterList FilterListEvent;

        public delegate void RefreshList();
        public event RefreshList RefreshListEvent;

        EmpleadosEdicion p = null;

        private void Actualizar()
        {
            try
            {
                SqlConnection con = new SqlConnection(MetodosGestion.db);
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Empleados]", con);
                DataTable dt = new DataTable(); ;

                da.Fill(dt);
                this.dataGridEmpleados.ItemsSource = dt.DefaultView;

                con.Open();
                con.Close();

                dataGridEmpleados.Columns[0].Visibility = Visibility.Collapsed;
                dataGridEmpleados.Columns[10].Visibility = Visibility.Collapsed;
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
            if (!MetodosGestion.IsOpen(p))
            {
                p = new EmpleadosEdicion(0);
                RefreshListEvent += new RefreshList(Actualizar);
                p.ActualizarLista = RefreshListEvent;
                p.Title = "Añadir Empleado";
                p.Owner = System.Windows.Application.Current.MainWindow;
                p.Show();
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (!MetodosGestion.IsOpen(p))
            {
                if (dataGridEmpleados.SelectedItem != null)
                {
                    DataRowView dd = (DataRowView)dataGridEmpleados.SelectedItem;
                    int id = dd.Row.Field<int>("Id_Empleado");
                    p = new EmpleadosEdicion(id);
                    RefreshListEvent += new RefreshList(Actualizar);
                    p.ActualizarLista = RefreshListEvent;
                    p.Title = "Editar Empleado";
                    p.Owner = System.Windows.Application.Current.MainWindow;
                    p.Show();
                }
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEmpleados.SelectedItem != null)
            {
                DataRowView dd = (DataRowView)dataGridEmpleados.SelectedItem;
                int id = dd.Row.Field<int>("Id_Empleado");
                Boolean resul = Mensajes.Mostrar("¿Está seguro?", Mensajes.Tipo.Confirmacion);
                if (resul)
                {
                    try
                    {
                        string bd = MetodosGestion.db;
                        using (SqlConnection con = new SqlConnection(bd))
                        using (SqlCommand command = con.CreateCommand())
                        {
                            command.CommandText = "DELETE FROM Empleados WHERE Id_Empleado = @id";

                            command.Parameters.AddWithValue("@id", id);

                            con.Open();
                            int a = command.ExecuteNonQuery();

                            if (a != 0)
                            {
                                con.Close();
                            }
                            else
                            {
                                Mensajes.Mostrar("Empleado ERROR Borrar", Mensajes.Tipo.Error);
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
            if (!MetodosGestion.IsOpen(p))
            {
                p = new EmpleadosEdicion(-1);
                FilterListEvent += new FilterList(Filtrar);
                p.FiltrarLista = FilterListEvent;
                p.Title = "Buscar Empleado";
                p.Owner = Application.Current.MainWindow;
                p.Show();
            }
            else
            {
                Filtrar();
            }
        }

        DataView view = null;
        DataTable dt;
        public void Filtrar()
        {
            List<String> nombres = AccesoVentana();

            if (view == null)
            {
                view = new DataView();
                dt = ((DataView)dataGridEmpleados.ItemsSource).ToTable();
                dt.TableName = "Empleados";
                view.Table = dt;
            }

            DateTime date = DateTime.Parse(nombres[7]);
            // Console.WriteLine(dt.ToString("dd/MM/yyyy"));

            // [NumVentas] >= {nombres[5]} AND 
            view.RowFilter = $"[NumVentas] >= {nombres[5]} AND [Salario] >= {nombres[6]} AND [Nombre] LIKE '%{nombres[0]}%' AND [NIF] LIKE '%{nombres[1]}%' AND [Telefono] LIKE '%{nombres[2]}%' AND [Email] LIKE '%{nombres[3]}%' AND [Direccion] LIKE '%{nombres[4]}%' AND [FechaContratacion] >= #{date.ToShortDateString()}#";

            // view.Sort = "CompanyName DESC";
            dt = view.ToTable();
            dataGridEmpleados.ItemsSource = null;
            dataGridEmpleados.ItemsSource = dt.DefaultView;
            dataGridEmpleados.Columns[0].Visibility = Visibility.Collapsed;
        }
        // params string[] nombres

        public List<String> AccesoVentana()
        {
            List<String> nombres = new List<String>();
            foreach (Window item in Application.Current.Windows)
            {
                //  0    1         2        3          4         5              6         7              8
                // ID "Nombre", "NIF", "Telefono", "Email", "Direccion", "NumVentas", "Salario", "FechaContratacion"
                if (item.Name == "EdicionEmpleados")
                {
                    //((EmpleadosEdicion)item).fechaDatePicker.SelectedDate = DateTime.Today;
                    //((EmpleadosEdicion)item).fechaDatePicker.SelectedDate = new DateTime(1990, 1, 1);
                    DateTime time = (DateTime)((EmpleadosEdicion)item).fechaDatePicker.SelectedDate;
                    String fecha = time.ToShortDateString();
                    String ventas;
                    if (String.IsNullOrWhiteSpace(((EmpleadosEdicion)item).ventasTextBox.Text))
                    {
                        ventas = "0";
                    }
                    else
                    {
                        ventas = ((EmpleadosEdicion)item).ventasTextBox.Text;
                    }
                    String salario;
                    if (String.IsNullOrWhiteSpace(((EmpleadosEdicion)item).salarioTextBox.Text))
                    {
                        salario = "0";
                    }
                    else
                    {
                        salario = ((EmpleadosEdicion)item).salarioTextBox.Text;
                    }

                    String[] nombresArray = {
                        ((EmpleadosEdicion)item).nombreTextBox.Text,
                        ((EmpleadosEdicion)item).nifTextBox.Text,
                        ((EmpleadosEdicion)item).telefonoTextBox.Text,
                        ((EmpleadosEdicion)item).emailTextBox.Text,
                        ((EmpleadosEdicion)item).direccionTextBox.Text,
                        ventas,
                        salario,
                        fecha
                    };
                    nombres.AddRange(nombresArray);
                }
            }
            return nombres;
        }

        private void dataGridEmpleados_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        private void bActualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }
    }
}