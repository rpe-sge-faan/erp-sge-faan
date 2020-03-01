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
using CrystalDecisions.CrystalReports.Engine;

namespace SGE_erp.Gestion.Informes
{
    /// <summary>
    /// Lógica de interacción para EmpleadosViewer.xaml
    /// </summary>
    public partial class EmpleadosViewer : Window
    {
        public EmpleadosViewer()
        {
            InitializeComponent();
        }

        private ReportClass report;

        public EmpleadosViewer(ReportClass _report)
        {
            InitializeComponent();
            report = _report;
        }

        private void cargar()
        {
            reporteVer.ViewerCore.ReportSource = report;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Report.rpt");
            reporteVer.ViewerCore.ReportSource = report;
            //reporteVer.ViewerCore.RefreshReport();
        }
    }
}
