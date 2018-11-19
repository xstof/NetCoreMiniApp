using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvc.Database;
using MyMvc.Database.Model;

namespace MyMvc.Controllers
{
    public class Countries : Controller
    {
        private readonly WideWorldDbContext _context;

        public Countries(WideWorldDbContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            var wideWorldDbContext = _context.Countries.Include(c => c.LastEditedByNavigation);
            return View(await wideWorldDbContext.ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var countries = await _context.Countries
                .Include(c => c.LastEditedByNavigation)
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (countries == null)
            {
                return NotFound();
            }

            return View(countries);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            ViewData["LastEditedBy"] = new SelectList(_context.People, "PersonId", "FullName");
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryId,CountryName,FormalName,IsoAlpha3Code,IsoNumericCode,CountryType,LatestRecordedPopulation,Continent,Region,Subregion,LastEditedBy,ValidFrom,ValidTo")] MyMvc.Database.Model.Countries countries)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LastEditedBy"] = new SelectList(_context.People, "PersonId", "FullName", countries.LastEditedBy);
            return View(countries);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var countries = await _context.Countries.FindAsync(id);
            if (countries == null)
            {
                return NotFound();
            }
            ViewData["LastEditedBy"] = new SelectList(_context.People, "PersonId", "FullName", countries.LastEditedBy);
            return View(countries);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CountryId,CountryName,FormalName,IsoAlpha3Code,IsoNumericCode,CountryType,LatestRecordedPopulation,Continent,Region,Subregion,LastEditedBy,ValidFrom,ValidTo")] MyMvc.Database.Model.Countries countries)
        {
            if (id != countries.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(countries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountriesExists(countries.CountryId))
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
            ViewData["LastEditedBy"] = new SelectList(_context.People, "PersonId", "FullName", countries.LastEditedBy);
            return View(countries);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var countries = await _context.Countries
                .Include(c => c.LastEditedByNavigation)
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (countries == null)
            {
                return NotFound();
            }

            return View(countries);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var countries = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(countries);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountriesExists(int id)
        {
            return _context.Countries.Any(e => e.CountryId == id);
        }
    }
}
