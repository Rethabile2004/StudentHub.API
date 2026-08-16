namespace StudentHub.API.Models;
 public class LecturerCourse
    {
        public Guid Id { get; set; }

        public Guid LecturerId { get; set; }
        public Guid CourseId { get; set; }

        public Lecturer Lecturer { get; set; }
        public Course Course { get; set; }
    }
