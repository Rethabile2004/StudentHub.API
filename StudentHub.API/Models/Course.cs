namespace StudentHub.API.Models;

public class Course
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }

    public Guid DepartmentId { get; set; }

    public Department Department { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<LecturerCourse> LecturerCourses { get; set; } = new List<LecturerCourse>();
}