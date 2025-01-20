using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Fitally.Models
{
    public class WorkoutDay
    {
        public int Id { get; set; }
        [DisplayName("User")]
        public string UserId { get; set; }
        [DisplayName("Workout date")]
        public DateTime WorkoutDate { get; set; }
        [DisplayName("Day name")]
        public string DayName { get; set; }
        [DisplayName("Info")]
        public string Info { get; set; }

        [DisplayName("Workout exercises")]
        public virtual ICollection<WorkoutExercise>? WorkoutExercises { get; set; }
        public virtual IdentityUser? User { get; set; }
    }
}
