namespace StudentHub.API.Models;
    public class Lecturer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid DepartmentId { get; set; }

        public Department Department { get; set; }
        public ICollection<LecturerCourse> LecturerCourses { get; set; } = new List<LecturerCourse>();
    }   
