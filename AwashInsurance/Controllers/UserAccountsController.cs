using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AwashInsurance.Models;

namespace AwashInsurance.Controllers
{
    public class UserAccountsController : Controller
    {
        private readonly AppDbContext _context;

        public UserAccountsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserAccounts
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserAccounts.Include(u => u.Employee).Include(u => u.Role);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.Employee)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // GET: UserAccounts/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoginId,Password,EmployeeId,RoleId,CreatedAt,IsActive")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", userAccount.EmployeeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", userAccount.RoleId);
            return View(userAccount);
        }

        // GET: UserAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", userAccount.EmployeeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", userAccount.RoleId);
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LoginId,Password,EmployeeId,RoleId,CreatedAt,IsActive")] UserAccount userAccount)
        {
            if (id != userAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountExists(userAccount.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", userAccount.EmployeeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", userAccount.RoleId);
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.Employee)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount != null)
            {
                _context.UserAccounts.Remove(userAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountExists(int id)
        {
            return _context.UserAccounts.Any(e => e.Id == id);
        }
    }
}
