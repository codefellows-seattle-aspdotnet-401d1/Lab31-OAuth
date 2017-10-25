using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityDay2.Data;
using IdentityDay2.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityDay2.Controllers
{
    [Authorize(Policy = "Admin Only")]
    public class CrewMembersController : Controller
    {
        private readonly AppDbContext _context;

        public CrewMembersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CrewMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.CrewMember.ToListAsync());
        }

        // GET: CrewMembers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crewMember = await _context.CrewMember
                .SingleOrDefaultAsync(m => m.Id == id);
            if (crewMember == null)
            {
                return NotFound();
            }

            return View(crewMember);
        }

        // GET: CrewMembers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crewMember = await _context.CrewMember.SingleOrDefaultAsync(m => m.Id == id);
            if (crewMember == null)
            {
                return NotFound();
            }
            return View(crewMember);
        }

        // POST: CrewMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Rank,Department,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] CrewMember crewMember)
        {
            if (id != crewMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crewMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrewMemberExists(crewMember.Id))
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
            return View(crewMember);
        }

        // GET: CrewMembers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crewMember = await _context.CrewMember
                .SingleOrDefaultAsync(m => m.Id == id);
            if (crewMember == null)
            {
                return NotFound();
            }

            return View(crewMember);
        }

        // POST: CrewMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var crewMember = await _context.CrewMember.SingleOrDefaultAsync(m => m.Id == id);
            _context.CrewMember.Remove(crewMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrewMemberExists(string id)
        {
            return _context.CrewMember.Any(e => e.Id == id);
        }
    }
}
