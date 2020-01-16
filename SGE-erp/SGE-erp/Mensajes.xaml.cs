using System;
using System.Collections.Generic;
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

namespace SGE_erp
{
    /// <summary>
    /// Interaction logic for Mensajes.xaml
    /// </summary>
    public partial class Mensajes : Window
    {
        Tipo tipo;
        string mensaje;

        public enum Tipo
        {
            Info,
            Confirmacion,
            Error
        }


        public Mensajes(string m, Tipo t)
        {
            InitializeComponent();
            this.tipo = t;
            this.mensaje = m;
            SetMensaje();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static Boolean Mostrar(string mensaje, Tipo t)
        {
            Mensajes m = new Mensajes(mensaje, t);
            m.ShowDialog();

            if (t == Tipo.Confirmacion && m.DialogResult == true)
                return true;
            return false;
        }


        public void SetMensaje()
        {
            switch (this.tipo)
            {
                case Tipo.Info:
                    btnCancel.Visibility = Visibility.Hidden;
                    iconito.Kind = MaterialDesignThemes.Wpf.PackIconKind.InfoCircle;
                    break;
                case Tipo.Confirmacion:
                    btnCancel.Visibility = Visibility.Visible;
                    iconito.Kind = MaterialDesignThemes.Wpf.PackIconKind.QuestionMarkCircle;
                    break;
                case Tipo.Error:
                    btnCancel.Visibility = Visibility.Hidden;
                    iconito.Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning;
                    break;
            }
            info.Text = $"{this.mensaje}";


        }
    }
}
