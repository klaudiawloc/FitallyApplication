using Fitally.Common;
using Fitally.Data;
using Fitally.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fitally.Controllers
{
    [Authorize]
    public class WorkoutDaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutDaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkoutDays
        public async Task<IActionResult> Index()
        {
            if(User.IsAdmin())
                return View(await _context.WorkoutDays.Include(x => x.WorkoutExercises).ThenInclude(x => x.Exercise).ToListAsync());
            else
                return View(await _context.WorkoutDays.Include(x => x.WorkoutExercises).ThenInclude(x =>x.Exercise).Where(x => x.UserId == User.GetId()).ToListAsync());
        }

        // GET: WorkoutDays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var workoutDay = await _context.WorkoutDays
                   .FirstOrDefaultAsync(m => m.Id == id);

            if (workoutDay == null)
                return NotFound();

            if (User.IsAdmin())
            {
                return View(workoutDay);
            }
            else
            {
                if (workoutDay.UserId == User.GetId())
                    return View(workoutDay);
                else
                    return NotFound();
            }
            
        }

        // GET: WorkoutDays/Create
        public IActionResult Create()
        {
            if(User.IsAdmin())
                ViewBag.AvailableUsers = new SelectList(_context.Users, "Id", "UserName");

            return View();
        }

        // POST: WorkoutDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,WorkoutDate,DayName,Info")] WorkoutDay workoutDay)
        {
            if (!User.IsAdmin())
                workoutDay.UserId = User.GetId();

            if (ModelState.IsValid)
            {
                _context.Add(workoutDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(workoutDay);
        }

        // GET: WorkoutDays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsAdmin())
                ViewBag.AvailableUsers = new SelectList(_context.Users, "Id", "UserName");

            if (id == null)
                return NotFound();

            var workoutDay = await _context.WorkoutDays.FindAsync(id);

            if (workoutDay == null)
                return NotFound();

            if(!User.IsAdmin() && workoutDay.UserId != User.GetId())
                return NotFound();

            return View(workoutDay);
        }

        // POST: WorkoutDays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,WorkoutDate,DayName,Info")] WorkoutDay workoutDay)
        {
            if (id != workoutDay.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var originalWorkoutDay = await _context.WorkoutDays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == workoutDay.Id);

                    if(originalWorkoutDay is null)
                        return NotFound();

                    if (!User.IsAdmin() && originalWorkoutDay.UserId != User.GetId())
                        return NotFound();

                    if(!User.IsAdmin() && originalWorkoutDay.UserId == User.GetId())
                    {
                        workoutDay.UserId = User.GetId();
                    }

                    _context.Update(workoutDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutDayExists(workoutDay.Id))
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
            return View(workoutDay);
        }

        // GET: WorkoutDays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var workoutDay = await _context.WorkoutDays
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workoutDay is null)
                return NotFound();

            if (User.IsAdmin())
                return View(workoutDay);

            if (workoutDay.UserId == User.GetId())
                return View(workoutDay);

            return NotFound();
        }

        // POST: WorkoutDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutDay = await _context.WorkoutDays.FindAsync(id);

            if (workoutDay is null)
                return NotFound();

            if (User.IsAdmin())
            {
                _context.WorkoutDays.Remove(workoutDay);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            if (workoutDay.UserId == User.GetId())
            {
                _context.WorkoutDays.Remove(workoutDay);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        private bool WorkoutDayExists(int id)
        {
            return _context.WorkoutDays.Any(e => e.Id == id);
        }
    }
}
