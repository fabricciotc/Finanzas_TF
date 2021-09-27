using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Finanzas_TF.Models
{
    public class Cliente
    {
        [Key]
        public Guid Id { set; get; }
        [Required]
        public string Fullname { set; get; }
        [StringLength(450)]
        [Required]
        public string RUC { set; get; }
        [Required]
        public string Email { set; get; }
        [Required]
        public string Telefono { set; get; }
        public List<ReciboHonorarios> Ventas { set; get; }
        public string Direccion { set; get; }
    }
}
