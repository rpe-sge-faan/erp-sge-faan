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

namespace SGE_erp.Venta
{
    /// <summary>
    /// Lógica de interacción para VentaEdicion.xaml
    /// </summary>
    public partial class VentaEdicion : Page
    {
        int id = 0;
        public VentaEdicion(int num)
        {
            InitializeComponent();
            id = num;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}