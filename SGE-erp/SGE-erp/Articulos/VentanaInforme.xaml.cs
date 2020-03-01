using CrystalDecisions.CrystalReports.Engine;
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

namespace SGE_erp.Articulos
{
    /// <summary>
    /// Lógica de interacción para VentanaInforme.xaml
    /// </summary>
    public partial class VentanaInforme : Window
    {
        private ReportClass report;
        public VentanaInforme()
        {
            InitializeComponent();
        }

        public VentanaInforme(ReportClass reporte)
        {
            InitializeComponent();
            report = reporte;
            cargar();
        }   
        
        private void cargar()
        {
            reporteVer.ViewerCore.ReportSource = report;
        }

    }
}
