using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityDay2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityDay2.Controllers
{
    [Authorize]
    public class CMSController : Controller
    {
        //adding userManager so I can access user properties for logic in the create controller
        private readonly UserManager<CrewMember> _userManager;
        private readonly IdentityDay2Context _context;

        public CMSController(UserManager<CrewMember> usermanager, IdentityDay2Context context)
        {
            _userManager = usermanager;
            _context = context;
        }

        // GET: CMS
        [Authorize(Policy = "Admin Only")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CMS.ToListAsync());
        }

        // GET: CMS/Details/5
        [Authorize(Policy = "Admin Only")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cMS = await _context.CMS
                .SingleOrDefaultAsync(m => m.ID == id);
            if (cMS == null)
            {
                return NotFound();
            }

            return View(cMS);
        }

        // GET: CMS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CMS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Channel,Content,IsAuthorized")] CMS cMS)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser.Department != cMS.Channel)
            {
                cMS.IsAuthorized = false;
            }
            else
            {
                cMS.IsAuthorized = true;
            }
            if (ModelState.IsValid)
            {
                _context.Add(cMS);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(cMS);
        }

        // GET: CMS/Edit/5
        [Authorize(Policy = "Admin Only")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cMS = await _context.CMS.SingleOrDefaultAsync(m => m.ID == id);
            if (cMS == null)
            {
                return NotFound();
            }
            return View(cMS);
        }

        // POST: CMS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin Only")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Channel,Content,IsAuthorized")] CMS cMS)
        {
            if (id != cMS.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cMS);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CMSExists(cMS.ID))
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
            return View(cMS);
        }

        // GET: CMS/Delete/5
        [Authorize(Policy = "Admin Only")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cMS = await _context.CMS
                .SingleOrDefaultAsync(m => m.ID == id);
            if (cMS == null)
            {
                return NotFound();
            }

            return View(cMS);
        }

        // POST: CMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin Only")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cMS = await _context.CMS.SingleOrDefaultAsync(m => m.ID == id);
            _context.CMS.Remove(cMS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CMSExists(int id)
        {
            return _context.CMS.Any(e => e.ID == id);
        }
    }
}
