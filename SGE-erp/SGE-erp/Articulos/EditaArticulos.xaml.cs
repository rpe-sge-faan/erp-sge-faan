using SGE_erp.Gestion;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para EditaArticulos.xaml
    /// </summary>
    public partial class EditaArticulos : Window
    {
        public EditaArticulos()
        {
            InitializeComponent();
        }

        public Delegate ActualizarLista;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            SGE_erp.SetaData setaData = ((SGE_erp.SetaData)(this.FindResource("setaData")));
            // Cargar datos en la tabla Articulos. Puede modificar este código según sea necesario.
            SGE_erp.SetaDataTableAdapters.ArticulosTableAdapter setaDataArticulosTableAdapter = new SGE_erp.SetaDataTableAdapters.ArticulosTableAdapter();
            setaDataArticulosTableAdapter.Fill(setaData.Articulos);
            System.Windows.Data.CollectionViewSource articulosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("articulosViewSource")));
            articulosViewSource.View.MoveCurrentToFirst();
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            //int tipo;
            //if (tipoComboBox.SelectedValue.ToString() == "Particular") { tipo = 1; }
           // else { tipo = 2; }


            try
            {
                string bd = MetodosGestion.db;
                using (SqlConnection con = new SqlConnection(bd))
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Proveedores (Tipo, Nombre, Telefono, Email, Persona_Contacto, Direccion, NIF) " +
                        "VALUES (@tipo, @nombre, @telefono, @email, @persona, @direccion, @nif)";

                    //command.Parameters.AddWithValue("@tipo", tipo);
                    command.Parameters.AddWithValue("@nombre", id_ArticuloTextBox1.Text);
                   //command.Parameters.AddWithValue("@telefono", telefonoTextBox.Text);
                    //command.Parameters.AddWithValue("@email", emailTextBox.Text);
                    //command.Parameters.AddWithValue("@persona", personaContactoTextBox.Text);
                   // command.Parameters.AddWithValue("@direccion", direccionTextBox.Text);
                   // command.Parameters.AddWithValue("@nif", nIFTextBox.Text);

                    con.Open();
                    int a = command.ExecuteNonQuery();

                    if (a != 0)
                    {
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Proveedor ERROR");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            ActualizarLista.DynamicInvoke();

            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
