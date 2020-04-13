using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PNN.Web.Data.Entities;
using PNN.web.Data;

namespace PNN.Web.Controllers
{
    public class ParksController : Controller
    {
        private readonly DataContext _context;

        public ParksController(DataContext context)
        {
            _context = context;
        }

        // GET: Parks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parks.ToListAsync());
        }

        // GET: Parks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var park = await _context.Parks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (park == null)
            {
                return NotFound();
            }

            return View(park);
        }

        // GET: Parks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Creation,ImageUrl,Been,Extension,Height,Temperature,Flora,Wildlife,Communities,Like,DisLike")] Park park)
        {
            if (ModelState.IsValid)
            {
                _context.Add(park);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(park);
        }

        // GET: Parks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var park = await _context.Parks.FindAsync(id);
            if (park == null)
            {
                return NotFound();
            }
            return View(park);
        }

        // POST: Parks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Creation,ImageUrl,Been,Extension,Height,Temperature,Flora,Wildlife,Communities,Like,DisLike")] Park park)
        {
            if (id != park.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(park);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkExists(park.Id))
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
            return View(park);
        }

        // GET: Parks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var park = await _context.Parks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (park == null)
            {
                return NotFound();
            }

            return View(park);
        }

        // POST: Parks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var park = await _context.Parks.FindAsync(id);
            _context.Parks.Remove(park);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkExists(int id)
        {
            return _context.Parks.Any(e => e.Id == id);
        }
    }
}
