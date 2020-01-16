using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

namespace SGE_erp.Controles
{
    /// <summary>
    /// Interaction logic for Numeric.xaml
    /// </summary>
    public partial class Numeric : UserControl
    {
        public Numeric()
        {
            InitializeComponent();
            NumericUD.Text = startvalue.ToString();
        }

        public int minvalue = 0,
        maxvalue = 100000,
        startvalue = 0;

        public int Value
        {
            get { return int.Parse(NumericUD.Text); }
            set { NumericUD.Text = value.ToString(); }
        }

        //public int Minimum
        //{

        //}

        //public int Maximum
        //{

        //}

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NumericUD.Text != "") number = Convert.ToInt32(NumericUD.Text);
            else number = 0;
            if (number < maxvalue)
                NumericUD.Text = Convert.ToString(number + 1);
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NumericUD.Text != "") number = Convert.ToInt32(NumericUD.Text);
            else number = 0;
            if (number > minvalue)
                NumericUD.Text = Convert.ToString(number - 1);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumericUD_TextChanged(object sender, TextChangedEventArgs e)
        {
            //int number = 0;
            //if (NumericUD.Text != "")
            //    if (!int.TryParse(NumericUD.Text, out number)) NumericUD.Text = startvalue.ToString();
            //if (number > maxvalue) NumericUD.Text = maxvalue.ToString();
            //if (number <= minvalue)
            //{
            //    NumericUD.Text = minvalue.ToString();
            //}
            //else { }
            //NumericUD.SelectionStart = NumericUD.Text.Length;

        }
    }
}