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
    public class OrganizationUnitsController : Controller
    {
        private readonly AppDbContext _context;

        public OrganizationUnitsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OrganizationUnits
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.OrganizationUnits.Include(o => o.OrganizationType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: OrganizationUnits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationUnit = await _context.OrganizationUnits
                .Include(o => o.OrganizationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizationUnit == null)
            {
                return NotFound();
            }

            return View(organizationUnit);
        }

        // GET: OrganizationUnits/Create
        public IActionResult Create()
        {
            ViewData["OrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "Id", "Id");
            return View();
        }

        // POST: OrganizationUnits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OrganizationTypeId")] OrganizationUnit organizationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organizationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "Id", "Id", organizationUnit.OrganizationTypeId);
            return View(organizationUnit);
        }

        // GET: OrganizationUnits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationUnit = await _context.OrganizationUnits.FindAsync(id);
            if (organizationUnit == null)
            {
                return NotFound();
            }
            ViewData["OrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "Id", "Id", organizationUnit.OrganizationTypeId);
            return View(organizationUnit);
        }

        // POST: OrganizationUnits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OrganizationTypeId")] OrganizationUnit organizationUnit)
        {
            if (id != organizationUnit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organizationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationUnitExists(organizationUnit.Id))
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
            ViewData["OrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "Id", "Id", organizationUnit.OrganizationTypeId);
            return View(organizationUnit);
        }

        // GET: OrganizationUnits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationUnit = await _context.OrganizationUnits
                .Include(o => o.OrganizationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizationUnit == null)
            {
                return NotFound();
            }

            return View(organizationUnit);
        }

        // POST: OrganizationUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organizationUnit = await _context.OrganizationUnits.FindAsync(id);
            if (organizationUnit != null)
            {
                _context.OrganizationUnits.Remove(organizationUnit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationUnitExists(int id)
        {
            return _context.OrganizationUnits.Any(e => e.Id == id);
        }
    }
}
