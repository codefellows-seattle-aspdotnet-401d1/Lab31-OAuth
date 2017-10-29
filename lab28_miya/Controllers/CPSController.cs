using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab28_miya.Models;
using Microsoft.AspNetCore.Authorization;

namespace lab28_miya.Controllers
{
    [Authorize(Policy = "Field Work")]
    public class CPSController : Controller
    {
        private readonly lab28_miyaContext _context;

        public CPSController(lab28_miyaContext context)
        {
            _context = context;
        }

        // GET: CPS
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CPS.ToListAsync());
        }

        // GET: CPS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cPS = await _context.CPS
                .SingleOrDefaultAsync(m => m.ID == id);
            if (cPS == null)
            {
                return NotFound();
            }

            return View(cPS);
        }

        // GET: CPS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CPS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,YearsInService,Region,HasCompassion,Zodiac")] CPS cPS)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cPS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cPS);
        }

        // GET: CPS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cPS = await _context.CPS.SingleOrDefaultAsync(m => m.ID == id);
            if (cPS == null)
            {
                return NotFound();
            }
            return View(cPS);
        }

        // POST: CPS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,YearsInService,Region,HasCompassion,Zodiac")] CPS cPS)
        {
            if (id != cPS.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cPS);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CPSExists(cPS.ID))
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
            return View(cPS);
        }

        // GET: CPS/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cPS = await _context.CPS
                .SingleOrDefaultAsync(m => m.ID == id);
            if (cPS == null)
            {
                return NotFound();
            }

            return View(cPS);
        }

        // POST: CPS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cPS = await _context.CPS.SingleOrDefaultAsync(m => m.ID == id);
            _context.CPS.Remove(cPS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CPSExists(int id)
        {
            return _context.CPS.Any(e => e.ID == id);
        }
    }
}
