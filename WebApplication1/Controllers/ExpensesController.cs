using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvimCebim.Data;
using EvimCebim.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EvimCebim.Controllers
{
    [Authorize] // Giriş zorunlu
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Sadece giriş yapan kullanıcının verileri
            return View(await _context.Expenses.Where(x => x.AppUserId == userId).ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            // GÜVENLİK KONTROLÜ: Başkasının detayını görmesin
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (expense.AppUserId != userId)
            {
                return Unauthorized(); 
            }

            return View(expense);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Amount,Date,Category")] Expense expense)
        {
             // Validasyon iptal, direkt ID basıp ekliyoruz
             expense.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             _context.Add(expense);
             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (expense.AppUserId != userId)
            {
                return Unauthorized();
            }

            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Amount,Date,Category")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Güvenlik: Başkasının verisini editleyemesin
            var existingExpense = await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (existingExpense == null || existingExpense.AppUserId != userId)
            {
                 return Unauthorized();
            }
            
            // AppUserId kaybolmasın diye tekrar atıyoruz
            expense.AppUserId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
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
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (expense.AppUserId != userId)
            {
                return Unauthorized();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expenses.FindAsync(id);
            
            if (expense != null)
            {
                // Güvenlik: Sadece sahibi silebilir
                if (expense.AppUserId == userId) 
                {
                    _context.Expenses.Remove(expense);
                    await _context.SaveChangesAsync();
                }
                else 
                {
                    return Unauthorized();
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
