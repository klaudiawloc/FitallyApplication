using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fitally.Data;
using Fitally.Models;
using Fitally.Common;
using Microsoft.AspNetCore.Authorization;

namespace Fitally.Controllers
{
    [Authorize]
    public class WorkoutExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkoutExercises
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return View(await _context.WorkoutExercises
                    .Include(w => w.Exercise)
                    .Include(w => w.WorkoutDay)
                    .ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.WorkoutExercises
                    .Include(w => w.Exercise)
                    .Include(w => w.WorkoutDay)
                    .Where(w => w.WorkoutDay.UserId == User.GetId());

                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: WorkoutExercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var workoutExercise = await _context.WorkoutExercises
                .Include(w => w.Exercise)
                .Include(w => w.WorkoutDay)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workoutExercise is null)
                return NotFound();

            if (User.IsInRole("Admin"))
            {
                return View(workoutExercise);
            }
            else
            {
                if (workoutExercise.WorkoutDay.UserId == User.GetId())
                    return View(workoutExercise);
                else
                    return NotFound();
            }
        }

        // GET: WorkoutExercises/Create
        public IActionResult Create()
        {
            var workoutDays = User.IsAdmin() ? _context.WorkoutDays : _context.WorkoutDays.Where(x => x.UserId == User.GetId());

            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Name");
            ViewData["WorkoutDayId"] = new SelectList(workoutDays, "Id", "DayName");
            return View();
        }

        // POST: WorkoutExercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkoutDayId,ExerciseId")] WorkoutExercise workoutExercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workoutExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var workoutDays = User.IsAdmin() ? _context.WorkoutDays : _context.WorkoutDays.Where(x => x.UserId == User.GetId());

            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Name", workoutExercise.ExerciseId);
            ViewData["WorkoutDayId"] = new SelectList(workoutDays, "Id", "DayName", workoutExercise.WorkoutDayId);

            return View(workoutExercise);
        }

        // GET: WorkoutExercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var workoutExercise = await _context.WorkoutExercises.FindAsync(id);

            if (workoutExercise is null)
                return NotFound();

            var workoutDays = User.IsAdmin() ? _context.WorkoutDays : _context.WorkoutDays.Where(x => x.UserId == User.GetId());

            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Name", workoutExercise.ExerciseId);
            ViewData["WorkoutDayId"] = new SelectList(workoutDays, "Id", "DayName", workoutExercise.WorkoutDayId);

            return View(workoutExercise);
        }

        // POST: WorkoutExercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkoutDayId,ExerciseId")] WorkoutExercise workoutExercise)
        {
            if (id != workoutExercise.Id)
                return NotFound();


            if (ModelState.IsValid)
            {
                try
                {
                    var originalWorkoutExercise = await _context.WorkoutExercises
                        .Include(x => x.WorkoutDay)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (originalWorkoutExercise is null)
                        return NotFound();

                    if (!User.IsAdmin() && originalWorkoutExercise.WorkoutDay.UserId != User.GetId())
                        return NotFound();

                    _context.Update(workoutExercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExerciseExists(workoutExercise.Id))
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
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Name", workoutExercise.ExerciseId);
            ViewData["WorkoutDayId"] = new SelectList(_context.WorkoutDays, "Id", "DayName", workoutExercise.WorkoutDayId);
            return View(workoutExercise);
        }

        // GET: WorkoutExercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var workoutExercise = await _context.WorkoutExercises
                .Include(w => w.Exercise)
                .Include(w => w.WorkoutDay)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workoutExercise == null)
                return NotFound();

            if (User.IsAdmin())
                return View(workoutExercise);

            if (workoutExercise.WorkoutDay.UserId == User.GetId())
                return View(workoutExercise);

            return NotFound();
        }

        // POST: WorkoutExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutExercise = await _context.WorkoutExercises.Include(x => x.WorkoutDay).FirstOrDefaultAsync(x => x.Id == id);

            if (workoutExercise is null)
                return NotFound();

            if (User.IsAdmin())
            {
                _context.WorkoutExercises.Remove(workoutExercise);
            }
            else
            {
                if (workoutExercise.WorkoutDay.UserId == User.GetId())
                    _context.WorkoutExercises.Remove(workoutExercise);
                else
                    return NotFound();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExerciseExists(int id)
        {
            return _context.WorkoutExercises.Any(e => e.Id == id);
        }
    }
}
