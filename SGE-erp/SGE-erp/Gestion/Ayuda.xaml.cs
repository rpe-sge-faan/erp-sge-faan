﻿using System;
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

namespace SGE_erp.Gestion
{
    /// <summary>
    /// Interaction logic for Ayuda.xaml
    /// </summary>
    public partial class Ayuda : UserControl
    {
        public Ayuda()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Diagnostics.Process.Start(appPath + "manual.pdf");
        }
    }
}
