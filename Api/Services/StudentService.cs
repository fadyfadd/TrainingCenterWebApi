using AutoMapper;
using Data;
using Data.Dtos;

namespace Api.Services;

public class StudentService
{
    private readonly MainDataBaseContext dataContext;
    private readonly IMapper mapper;
    public StudentService(MainDataBaseContext dataContext, IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
    }


public async Task<StudentDocumentDto> AddDocument(StudentDocumentDto studentDocumentDto, IFormFile file)
{
    var guid = Guid.NewGuid().ToString();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "Documents");

    Directory.CreateDirectory(directoryPath);         

    string fullFileName = Path.Combine(directoryPath, guid);

    studentDocumentDto.Id = 0;    
    studentDocumentDto.DocumentUrl = fullFileName;
    studentDocumentDto.DocumentName = file.FileName;
    
    studentDocumentDto.UploadedAt = DateTime.UtcNow;

    using (var stream = new FileStream(fullFileName, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var studentDocument = mapper.Map<StudentDocument>(studentDocumentDto);
    dataContext.StudentDocument.Add(studentDocument);
    await dataContext.SaveChangesAsync();

    return mapper.Map<StudentDocumentDto>(studentDocument);
}
}
