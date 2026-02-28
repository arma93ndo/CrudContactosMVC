using CrudContactosMVC.Data;
using CrudContactosMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudContactosMVC.Controllers
{
    public class ContactosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contactos
        [HttpGet] // No hace falta poner este HttpGet ya que no se trata de una API. Es sólo para volverlo explícito.
        public async Task<IActionResult> Index()
        {   // Este método consulta la tabla Contactos. Usa EF Core de forma asíncrona. Y regresa una lista de entidades.
            var contactos = await _context.Contactos.ToListAsync();
            return View(contactos);
        }

        [HttpGet] // No hace falta poner este HttpGet ya que no se trata de una API. Es sólo para volverlo explícito.
        public IActionResult Crear()
        { 
            return View();
        }

        [HttpPost] // No hace falta poner este HttpPost ya que no se trata de una API. Es sólo para volverlo explícito.
        [ValidateAntiForgeryToken] // Me protege de ataques XSS.
        public async Task<IActionResult> Crear(Contacto contacto)
        {
            if (ModelState.IsValid)
            {   // Guardamos en la BBDD.
                _context.Add(contacto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // En caso de éxito, redirijo al usuario a la página Index de este controlador (Contactos).
            }

            return View(contacto); // Si falla las validaciones, lo devuelvo al mismo formulario.
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }

            var contacto = await _context.Contactos.FindAsync(id);

            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto); // Si todo fue correcto, entonces regresamos el contacto a la vista.
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Me protege de ataques XSS.
        public async Task<IActionResult> Editar(int id, Contacto contacto)
        {
            if (id != contacto.Id)
            {   // Los datos no concuerdan.
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contacto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (!ContactoExists(contacto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(contacto);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contactos.FirstOrDefaultAsync(c => c.Id == id);

            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contactos.FirstOrDefaultAsync(c => c.Id == id);

            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        [HttpPost] // No hace falta poner este HttpPost ya que no se trata de una API. Es sólo para volverlo explícito.
        [ValidateAntiForgeryToken] // Me protege de ataques XSS.
        public async Task<IActionResult> Borrar(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);

            if (contacto != null) 
            {
                _context.Contactos.Remove(contacto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index)); // Si falla las validaciones, lo devuelvo a la página de inicio de Contactos.
        }

        private bool ContactoExists(int id)
        {
            return _context.Contactos.Any(e => e.Id == id);
        }
    }
}
