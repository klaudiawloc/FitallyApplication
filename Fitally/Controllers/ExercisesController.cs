using Fitally.Common;
using Fitally.Data;
using Fitally.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitally.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            if(!User.IsAdmin())
                return NotFound();

            return View(await _context.Exercises.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.IsAdmin())
                return NotFound();

            if (id == null)
                return NotFound();

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
                return NotFound();

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            if (!User.IsAdmin())
                return NotFound();

            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Sets,Reps,Weight,Info")] Exercise exercise)
        {
            if (!User.IsAdmin())
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(exercise);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsAdmin())
                return NotFound();

            if (id == null)
                return NotFound();

            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
                return NotFound();

            return View(exercise);
        }

        // POST: Exercises/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Sets,Reps,Weight,Info")] Exercise exercise)
        {
            if (!User.IsAdmin())
                return NotFound();

            if (id != exercise.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsAdmin())
                return NotFound();

            if (id == null)
                return NotFound();

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
                return NotFound();

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsAdmin())
                return NotFound();

            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise != null)
                _context.Exercises.Remove(exercise);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
