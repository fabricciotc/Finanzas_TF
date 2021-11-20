using Finanzas_TF.Data;
using Finanzas_TF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finanzas_TF.Controllers
{
    [Authorize]
    public class CotizadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CotizadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.ReciboHonorarios.Include(r => r.Cliente).Where(c=>c.FechaPago.Date> DateTime.Now.Date).ToListAsync();
            return View(applicationDbContext);
        }
        [HttpPost]
        public async Task<IActionResult> Calculo(Calculador c)
        {
            if (c.Tasa == 1)
            {
                var div = (double)(c.TEA / 1200);
                var tg = Math.Pow((double)(1 + (div)), 12.00) - 1;
                c.TEA = (decimal)tg * 100;
            }
            ViewBag.gInicial = c.gInicio;
            ViewBag.gFinal = c.gFinal;
            ViewBag.TEA = c.TEA;
            ViewBag.anio = c.Anio;
            ViewBag.dolar = c.Dolar;
            var applicationDbContext =await _context.ReciboHonorarios.Include(r => r.Cliente).Where(c => c.FechaPago.Date > DateTime.Now.Date).ToListAsync();
            var ListFinal = new List<ReciboHonorariosCalculo>();
            foreach (var item in applicationDbContext)
            {
                var tepTEM = Math.Pow((double)(1 + c.TEA), (double)((double)(item.FechaPago.Date - DateTime.Now.Date).TotalDays / (double)c.Anio)) / 100;
                var dTem = (decimal)(tepTEM / (1 + tepTEM));
                var monto = (item.Moneda==0?item.Monto:item.Monto*c.Dolar);
                ListFinal.Add(new ReciboHonorariosCalculo { 
                    NombreCliente=item.Cliente.RUC+" - "+item.Cliente.RazonSocial,
                    Monto=monto,
                    dias= Convert.ToInt32((item.FechaPago.Date - DateTime.Now.Date).TotalDays),
                    gFinal=c.gFinal,
                    gInicial=c.gInicio,
                    TEP= Math.Round((decimal)tepTEM*100,2),
                    d=Math.Round(dTem*100,2),
                    Descuento= Math.Round(monto*dTem,2),
                    ValorNeto= Math.Round(monto-(monto * dTem),2),
                    ValorRecibir = Math.Round((monto - (monto * dTem))-c.gInicio,2),
                    Flujo=monto-c.gFinal,
                    FechaPago=item.FechaPago
                });
            }
            return View(ListFinal);
        }
    }
}
