using CrudContactosMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudContactosMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) // Envío los parámetros a la clase base "DbContext"
        { }

        public DbSet<Contacto> Contactos { get; set; } 
    }
}
 