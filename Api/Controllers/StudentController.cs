

using Api.Services;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class StudentController : ApiBaseController
{
    private readonly StudentService studentService;

    public StudentController(StudentService studentService)
    {
        this.studentService = studentService;
    }

    [HttpPost("addDocument")]
    public async Task<IActionResult> AddDocument([FromForm] StudentDocumentDto studentDocumentDto, [FromForm] IFormFile file)
    {
        var addedDocument = await studentService.AddDocument(studentDocumentDto, file);
        return Ok(addedDocument);
    }
}
