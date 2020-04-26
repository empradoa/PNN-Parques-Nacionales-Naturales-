using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNN.web.Data;
using PNN.Web.Data.Entities;

namespace PNN.Web.Controllers
{
    public class ZoneTypesController : Controller
    {
        private readonly DataContext _context;

        public ZoneTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: ZoneTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZoneTypes.ToListAsync());
        }

        // GET: ZoneTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoneType = await _context.ZoneTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zoneType == null)
            {
                return NotFound();
            }

            return View(zoneType);
        }

        // GET: ZoneTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ZoneTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ZoneType zoneType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zoneType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zoneType);
        }

        // GET: ZoneTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoneType = await _context.ZoneTypes.FindAsync(id);
            if (zoneType == null)
            {
                return NotFound();
            }
            return View(zoneType);
        }

        // POST: ZoneTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ZoneType zoneType)
        {
            if (id != zoneType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zoneType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZoneTypeExists(zoneType.Id))
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
            return View(zoneType);
        }

        // GET: ZoneTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoneType = await _context.ZoneTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zoneType == null)
            {
                return NotFound();
            }

            return View(zoneType);
        }

        // POST: ZoneTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zoneType = await _context.ZoneTypes.FindAsync(id);
            _context.ZoneTypes.Remove(zoneType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZoneTypeExists(int id)
        {
            return _context.ZoneTypes.Any(e => e.Id == id);
        }
    }
}
