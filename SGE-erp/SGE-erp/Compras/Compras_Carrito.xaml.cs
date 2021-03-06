﻿using System;
using System.Collections.Generic;
using System.Data;
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

namespace SGE_erp.Compras
{
    /// <summary>
    /// Lógica de interacción para Compras_Carrito.xaml
    /// </summary>
    public partial class Compras_Carrito : Window
    {
        public double precioFinal;
        public Delegate Comprar;

        public Compras_Carrito()
        {
            InitializeComponent();
            this.carrito.ItemsSource = ComprasAnadir.carritoCompra.DefaultView;
            calPrecioFinal();
            this.lb_PrecioTotal.Content = "PRECIO TOTAL " + precioFinal + "€";
            
            //this.carrito.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void BtnBorrarArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (carrito.SelectedItem != null)
            {
                if (Mensajes.Mostrar("¿Borrar articulo seleccionado?", Mensajes.Tipo.Confirmacion))
                {
                    ComprasAnadir.carritoCompra.Rows.RemoveAt(carrito.SelectedIndex);
                    calPrecioFinal();
                    this.lb_PrecioTotal.Content = "PRECIO TOTAL " + precioFinal + "€";
                }
            }                          
        }

        private void BtnVaciarCarro_Click(object sender, RoutedEventArgs e)
        {
            if(Mensajes.Mostrar("¿Eliminar todos los articulos?", Mensajes.Tipo.Confirmacion))
            {
                int j = ComprasAnadir.carritoCompra.Rows.Count;
                for (int i = 0; i < j; i++)
                {
                    ComprasAnadir.carritoCompra.Rows.RemoveAt(0);
                    calPrecioFinal();
                    this.lb_PrecioTotal.Content = "PRECIO TOTAL " + precioFinal + "€";
                }
                //this.carrito.ItemsSource = ComprasAnadir.carritoCompra.DefaultView;
            }            
        }

        private void BtnFinalizarCompra_Click(object sender, RoutedEventArgs e)
        {
            Comprar.DynamicInvoke();
            MainWindow mW = Application.Current.MainWindow as MainWindow;
            mW.actuar();
            this.Close();
        }

        public void calPrecioFinal()
        {
            precioFinal = 0;
            for (int i = 0; i < ComprasAnadir.carritoCompra.Rows.Count; i++)
            {
                DataRow row = ComprasAnadir.carritoCompra.Rows[i];
                precioFinal += Convert.ToDouble(row["Precio Total"]);
            }
        }
    }
}

