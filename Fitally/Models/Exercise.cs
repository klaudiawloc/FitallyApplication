namespace Fitally.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public string Info { get; set; }

        public List<WorkoutExercise>? WorkoutExercises { get; set; }
    }
}
