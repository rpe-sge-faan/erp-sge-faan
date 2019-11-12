using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SGE_erp.Gestion
{
    class MetodosGestion
    {
        public static bool IsOpen (Window window){
            return Application.Current.Windows.Cast<Window>().Any(x => x == window);
        }
    }
}
