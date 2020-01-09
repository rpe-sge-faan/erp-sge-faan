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


namespace SGE_erp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // http://www.pdfsharp.net/wiki/HelloWorld-sample.ashx
        // http://pdfsharp.net/wiki/Invoice-sample.ashx

        public MainWindow()
        {
            InitializeComponent();
        }
        public delegate void DoSomethingEvent();

        public void actuar()
        {
            ucCompras.tablita();
        }

    }
}
