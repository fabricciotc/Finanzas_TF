using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Finanzas_TF.Data;
using Finanzas_TF.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Finanzas_TF.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }
        static bool validateEmail(string email)
        {
            if (email == null)
            {
                return false;
            }
            if (new EmailAddressAttribute().IsValid(email))
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RazonSocial,RUC,Email,Telefono,Direccion")] Cliente cliente)
        {
            if(cliente.RazonSocial==null || cliente.RazonSocial == String.Empty)
            {
                ViewBag.Error = "Razon social no puede ser nula o vacia";
                return View(cliente);
            }
            bool successfullyParsed = int.TryParse(cliente.RUC, out var ignoreMe);
            bool successfullyParsedTE = int.TryParse(cliente.Telefono, out var ignoreMeTE);
            if (!successfullyParsedTE)
            {
                ViewBag.Error = "El telefono debe contener solo numeros";
                return View(cliente);
            }
            if (!successfullyParsed)
            {
                ViewBag.Error = "RUC debe ser solo numeros";
                return View(cliente);
            }
            if (cliente.RUC.Length!=11)
            {
                ViewBag.Error = "RUC debe ser de 11 digitos";

                return View(cliente);
            }
            if (!validateEmail(cliente.Email))
            {
                ViewBag.Error = "No es un email valido";

                return View(cliente);
            }
            if (cliente.RUC.Length !=9)
            {
                ViewBag.Error = "El Telefono debe ser de 9 digitos";

                return View(cliente);
            }
            if (cliente.RUC == null || cliente.RUC == String.Empty)
            {
                ViewBag.Error = "RUC no peude ser nulo o vacio";

                return View(cliente);
            }
            if (cliente.RUC == null || cliente.RUC == String.Empty)
            {
                ViewBag.Error = "RUC no peude ser nulo o vacio";

                return View(cliente);
            }
            if (ModelState.IsValid)
            {
                cliente.RazonSocial = cliente.RazonSocial.ToUpper();
                cliente.Id = Guid.NewGuid();
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,RazonSocial,RUC,Email,Telefono,Direccion")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }
            if (cliente.RazonSocial == null || cliente.RazonSocial == String.Empty)
            {
                ViewBag.Error = "Razon social no puede ser nula o vacia";
                return View(cliente);
            }
            bool successfullyParsed = int.TryParse(cliente.RUC, out var ignoreMe);
            bool successfullyParsedTE = int.TryParse(cliente.Telefono, out var ignoreMeTE);
            if (!successfullyParsedTE)
            {
                ViewBag.Error = "El telefono debe contener solo numeros";
                return View(cliente);
            }
            if (!successfullyParsed)
            {
                ViewBag.Error = "RUC debe ser solo numeros";
                return View(cliente);
            }
            if (cliente.RUC.Length != 11)
            {
                ViewBag.Error = "RUC debe ser de 11 digitos";

                return View(cliente);
            }
            if (!validateEmail(cliente.Email))
            {
                ViewBag.Error = "No es un email valido";

                return View(cliente);
            }
            if (cliente.RUC.Length != 9)
            {
                ViewBag.Error = "El Telefono debe ser de 9 digitos";

                return View(cliente);
            }
            if (cliente.RUC == null || cliente.RUC == String.Empty)
            {
                ViewBag.Error = "RUC no peude ser nulo o vacio";

                return View(cliente);
            }
            if (cliente.RUC == null || cliente.RUC == String.Empty)
            {
                ViewBag.Error = "RUC no peude ser nulo o vacio";

                return View(cliente);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
