using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EvimCebim.Models;

namespace EvimCebim.Data
{
    public class ApplicationDbContext : IdentityDbContext // IdentityDbContext'ten miras alıyor
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; }
    }
}