using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGE_erp.Database;

namespace SGE_erp.Gestion.Informes
{
    class Reportes
    {
        public static EmpleadosReport ObtenerReporteEmpleados()
        {
            EmpleadosReport InformeInventarioReport = new EmpleadosReport();
            dsInventario dataSet = Repositorio.ObtenerEmpleados();
            dataSet.EnforceConstraints = false;
            InformeInventarioReport.SetDataSource(dataSet);
            return InformeInventarioReport;
        }
    }
}
