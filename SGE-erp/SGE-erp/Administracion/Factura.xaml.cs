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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGE_erp.Administracion
{
    /// <summary>
    /// Interaction logic for Factura.xaml
    /// </summary>
    public partial class Factura : Window
    {
        public Factura()
        {
            InitializeComponent();
        }

        private void addToTable()
        {
            Grid gr = new Grid();
            gr.HorizontalAlignment = HorizontalAlignment.Stretch;
            gr.Margin = new Thickness(2);
            gr.Width = 480;

            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(2, GridUnitType.Star);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol3 = new ColumnDefinition();
            gridCol1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol4 = new ColumnDefinition();
            gridCol1.Width = new GridLength(1, GridUnitType.Star);

            gr.ColumnDefinitions.Add(gridCol1);
            gr.ColumnDefinitions.Add(gridCol2);
            gr.ColumnDefinitions.Add(gridCol3);
            gr.ColumnDefinitions.Add(gridCol4);

            TextBlock tb1 = new TextBlock();
            tb1.Text = "HOLAAAAAA";     // <------------
            TextBlock tb2 = new TextBlock();
            tb2.Text = "AAAAAAA";       // <------------
            tb2.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb3 = new TextBlock();
            tb3.Text = "EEEEE";         // <------------
            tb3.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock tb4 = new TextBlock();
            tb4.Text = "UUUUUUUU";      // <------------
            tb4.HorizontalAlignment = HorizontalAlignment.Center;
            tb4.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF62013C"));

            Grid.SetColumn(tb1, 0);
            Grid.SetColumn(tb2, 1);
            Grid.SetColumn(tb3, 2);
            Grid.SetColumn(tb4, 3);

            gr.Children.Add(tb1);
            gr.Children.Add(tb2);
            gr.Children.Add(tb3);
            gr.Children.Add(tb4);


            this.listaArticulos.Items.Add(gr);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            //try
            //{
            //    this.IsEnabled = false;
            //    PrintDialog printDialog = new PrintDialog();
            //    if (printDialog.ShowDialog() == true)
            //    {
            //        printDialog.PrintVisual(print, "invoice");
            //    }
            //}
            //finally
            //{
            //    this.IsEnabled = true;
            //}
        }
    }
}
