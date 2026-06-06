

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

    [HttpGet("download/{id}")]
    public async Task<FileResult> DownloadDocument([FromRoute] int id)
    {
        var file = await studentService.GetDocumentFile(id);
       
        Response.Headers.Append("Content-Disposition", $"attachment;"); 

        return File(
            file.ContentStream,
            file.ContentType,
            file.FileName);

    }

    [HttpDelete("deleteDocument/{id}")]
    public async Task<IActionResult> DeleteDocument([FromRoute] int id)
    {
        await studentService.DeleteDocument(id);
        return NoContent();
    }   
}
