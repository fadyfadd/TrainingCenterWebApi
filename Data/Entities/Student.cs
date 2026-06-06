

namespace Data.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrolledAt { get; set; }
        public Int32 UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Course> Courses { get; set; }

    }

}
