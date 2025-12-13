using EvimCebim.Models;
using Microsoft.EntityFrameworkCore;
namespace EvimCebim.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {
        }
        //veritabanındaki 'Expenses' tablosu bizim 'Expense' modelimize karşılık gelir.
        public DbSet<Expense> Expenses { get; set; }
    }
}