namespace StudentHub.API.Models;
 public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string Email { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
