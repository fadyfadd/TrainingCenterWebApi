
namespace Data.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int CourseCategoryId { get; set; }

        public CourseCategory CourseCategory { get; set; }
        public ICollection<Student> Students { get; set; } 

    }
}
