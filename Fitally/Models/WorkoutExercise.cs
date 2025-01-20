using System.ComponentModel;

namespace Fitally.Models
{
    public class WorkoutExercise
    {
        public int Id { get; set; }
        [DisplayName("Workout day")]
        public int WorkoutDayId { get; set; }
        [DisplayName("Exercise")]
        public int ExerciseId { get; set; }

        [DisplayName("Workout day")]
        public WorkoutDay? WorkoutDay { get; set; }
        [DisplayName("Exercise")]
        public Exercise? Exercise { get; set; }
    }
}
