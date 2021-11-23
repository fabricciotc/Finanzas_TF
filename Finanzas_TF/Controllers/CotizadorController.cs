using Finanzas_TF.Data;
using Finanzas_TF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Excel.FinancialFunctions;
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
        [HttpGet]
        public async Task<IActionResult> Index2(Guid re)
        {
            var applicationDbContext = await _context.ReciboHonorarios.Include(r => r.Cliente).Where(c => c.Id == re).ToListAsync();
            ViewBag.code = re;
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
           ViewBag.FechaDescuento = c.FechaDescuento;

           var applicationDbContext = await _context.ReciboHonorarios.Include(r => r.Cliente).Where(c => c.FechaPago.Date > DateTime.Now.Date).ToListAsync();
           var ListFinal = new List<ReciboHonorariosCalculo>();
           foreach (var item in applicationDbContext)
           {
               var tepTEM = Math.Pow((double)(1 + c.TEA), (double)((double)(item.FechaPago.Date - c.FechaDescuento).TotalDays / (double)c.Anio)) / 100;
               var dTem = (decimal)(tepTEM / (1 + tepTEM));
               var monto = (item.Moneda == 0 ? item.Monto : item.Monto * c.Dolar);
               ListFinal.Add(new ReciboHonorariosCalculo {
                   NombreCliente = item.Cliente.RUC + " - " + item.Cliente.RazonSocial,
                   Monto = monto,
                   dias = Convert.ToInt32((item.FechaPago.Date - c.FechaDescuento).TotalDays),
                   gFinal = c.gFinal,
                   gInicial = c.gInicio,
                   TEP = Math.Round((decimal)tepTEM * 100, 2),
                   d = Math.Round(dTem * 100, 2),
                   Descuento = Math.Round(monto * dTem, 2),
                   ValorNeto = Math.Round(monto - (monto * dTem), 2),
                   ValorRecibir = Math.Round((monto - (monto * dTem)) - c.gInicio, 2),
                   Flujo =  c.gFinal- monto ,
                   FechaPago = item.FechaPago
               });
           }
           var total = ListFinal.Sum(c => c.ValorRecibir);
           var fechaHoy = DateTime.Now.Date;
           var fechas = ListFinal.Select(c => c.FechaPago).ToList();
           fechas.Insert(0,fechaHoy);
           var flujos = ListFinal.Select(c => (double)c.Flujo).ToList();
           flujos.Add((double)total);
           double result = Financial.XIrr(flujos, fechas);
           ViewBag.TCEA = Math.Round(result*-100,2);
            return View(ListFinal);
        }

        [HttpPost]
        public async Task<IActionResult> Calculo2(Calculador c,Guid re)
        {
            ViewBag.TipoDeTasa = c.TipoDeTasa;
            ViewBag.Tasa = c.Tasa;
            ViewBag.Anio = c.Anio;
            ViewBag.Moneda = c.Moneda;
            ViewBag.Dolar = c.Dolar;
            ViewBag.gInicial = c.gInicio;
            ViewBag.gFinal = c.gFinal;
            ViewBag.FechaDescuento = c.FechaDescuento;

            if (c.TipoDeTasa == 0)//efectiva
            {
                ViewBag.TEA = Math.Round((Math.Pow( (double) (1+(c.Tasa/100)), (360/ (double) c.Anio)) - 1)*100, 6);
            }
            if (c.TipoDeTasa == 1) //nominal
            {
                ViewBag.TEA = Math.Round(((Math.Pow( ( (double)(1+((c.Tasa/100)/c.Anio)) ) ,360))-1)*100, 6);
            }

            c.TEA = (decimal)ViewBag.TEA;

            ViewBag.TCEA = c.TCEA;
            c.ValorARecibir = 0;

            var applicationDbContext = await _context.ReciboHonorarios.Include(r => r.Cliente).Where(c => c.Id ==re).ToListAsync();
            var ListFinal = new List<ReciboHonorariosCalculo>();

            foreach (var item in applicationDbContext)
            {
                var difd = (double) (item.FechaPago.Date - c.FechaDescuento).TotalDays; 
                var tep = (decimal) (Math.Pow((double)(1 + c.TEA / 100),  difd / 360 ) - 1) * 100;
                var dTep = (decimal) ( ( (tep/100) / (1 + (tep/100)) )*100 );
                var monto = (item.Moneda == 0 ? item.Monto : item.Monto * c.Dolar);

                ListFinal.Add(new ReciboHonorariosCalculo
                {
                    NombreCliente = item.Cliente.RUC + " - " + item.Cliente.RazonSocial,
                    Monto = monto,
                    dias = (int) difd,
                    gFinal = c.gFinal,
                    gInicial = c.gInicio,
                    TEP = Math.Round(tep, 3),
                    d = Math.Round(dTep, 3),
                    Descuento = Math.Round((monto*-1) * (dTep/100), 3),
                    ValorNeto = Math.Round(monto + Math.Round((monto*-1) * (dTep/100), 3), 2),
                    ValorRecibir = Math.Round(monto + Math.Round((monto*-1) * (dTep/100), 3) - c.gInicio, 2),
                    Flujo = (c.gFinal*-1) - monto,
                    FechaPago = item.FechaPago,
                    TCEAUnit = (decimal) Math.Round( ((Math.Pow((double)( (( (c.gFinal*-1) - monto) *-1) / (monto + Math.Round((monto*-1) * (dTep/100), 3) - c.gInicio) ), (360/difd) ) - 1)*100) , 3),
                });

                c.ValorARecibir += (monto + Math.Round((monto*-1) * (dTep/100), 3) - c.gInicio);
            }           

            var fechas = ListFinal.Select(c => c.FechaPago).ToList();
            fechas.Add(c.FechaDescuento);

            var flujos = ListFinal.Select(c => (double)c.Flujo).ToList();
            var total = ListFinal.Sum(c => c.ValorRecibir);
            flujos.Add((double)total);

            fechas.Reverse();
            flujos.Reverse();

            double result = Financial.XIrr(flujos, fechas);
            ViewBag.TCEA = Math.Round(result * 100, 4);
            ViewBag.ValorARecibir = Math.Round(total, 4);

            return View(ListFinal);
        }
    }
}
