using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGE_erp.Administracion;
using SGE_erp.Database;

namespace SGE_erp.Articulos
{
    class InventarioReporte
    {
        public static InformeInventarioReport ObtenerReporteInventario(int idInventario)
        {
            InformeInventarioReport InformeInventarioReport = new InformeInventarioReport();
            
            dsInventario inventario = InventarioRepo.ObtenerDatos(idInventario);
            inventario.EnforceConstraints = false;
            InformeInventarioReport.SetDataSource(inventario);
            return InformeInventarioReport;
        }
        
    }
}
