namespace StudentHub.API.Models;
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
