

namespace Data.Dtos;

public class CourseWithStudentsDto : CourseDto
{    
    public List<BaseStudentDto> students { get; set; } = new List<BaseStudentDto>();
}
