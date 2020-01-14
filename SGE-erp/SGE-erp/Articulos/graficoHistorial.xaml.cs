using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Interaction logic for graficoHistorial.xaml
    /// </summary>
    public partial class graficoHistorial : UserControl
    {
        public graficoHistorial()
        {
            InitializeComponent();

            Values = new ChartValues<double> { 0,150, 12, 160, 140 };
            Values.Add(100);
            double[] stock = new double[5] { 99, 98, 92, 97, 95 };
            IEnumerable<double> m = stock;
            Values.AddRange(m);


            DataContext = this;
        }

        public ChartValues<double> Values { get; set; }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            Chart.Update(true);
        }

        private string nameValue = "Nombre articulo";
        public string Prueba
        {
            get { return nameValue; }
            set { nameValue = value; }
        }
    }
}
