

using Api.Services;

namespace Api.Controllers;

public class StudentController
{
    private readonly StudentService studentService;

    public StudentController(StudentService studentService)
    {
        this.studentService = studentService;
    }
}
