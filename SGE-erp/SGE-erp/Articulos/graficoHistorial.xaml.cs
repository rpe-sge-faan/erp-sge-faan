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
            Values = new ChartValues<double> { 0 };
            lab = new ChartValues<string> { "0" };
            DataContext = this;
        }

        public ChartValues<double> Values { get; set; }
        public ChartValues<string> lab { get; set; }
        //private string[] lab;

        private void Update()
        {
            double[] stock = InfoArticulos.valoresStock;
            IEnumerable<double> m = stock;
            Values.AddRange(m);
            lab.AddRange(InfoArticulos.valoresFecha);
            Chart.Update(true);
            //DataContext = this;
            //lab = InfoArticulos.valoresFecha;
        }

        private string nameValue = "Nombre articulo";
        public string Prueba
        {
            get { return nameValue; }
            set { nameValue = value; }
        }

        
        //public string[] Labels
        //{
        //    get { return lab.; }
        //    set { lab = value; }
        //}


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
