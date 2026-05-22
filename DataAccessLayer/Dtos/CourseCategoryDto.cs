using System;

namespace DataAccessLayer.Dtos;

public class CourseCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CourseDto> Courses { get; set; } = new();
}
