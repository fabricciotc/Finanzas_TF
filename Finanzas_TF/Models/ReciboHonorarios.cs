using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finanzas_TF.Models
{
    public class ReciboHonorarios
    { 
        public Guid Id { set; get; }
        public DateTime FechaSubida { set; get; }
        public decimal Monto { set; get; }
    }
}
