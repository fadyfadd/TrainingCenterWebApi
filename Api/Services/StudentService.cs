using AutoMapper;
using Data;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Npgsql.Replication.PgOutput.Messages;

namespace Api.Services;

public class StudentService
{
    private readonly MainDataBaseContext dataContext;
    private readonly IMapper mapper;
    private readonly GeneralSettings generalSettings;
    public StudentService(MainDataBaseContext dataContext, IMapper mapper, IOptions<GeneralSettings> generalSettings)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
        this.generalSettings = generalSettings.Value;
    }


    public async Task<DownloadFileDto> GetDocumentFile(int documentId)
    {
        var entity = await dataContext.StudentDocument.FindAsync(documentId);
        var url = entity.DocumentUrl;
        var name = entity.DocumentName;
        var provider = new FileExtensionContentTypeProvider();

        String contentType = "application/octet-stream";
        Stream sourceStream = File.OpenRead(entity.DocumentUrl);

        DownloadFileDto fileDto = new DownloadFileDto
        {
            FileName = name
            ,
            ContentType = contentType
            ,
            ContentStream = sourceStream
        };

        return fileDto;

    }

    public async Task<StudentDocumentDto> AddDocument(StudentDocumentDto studentDocumentDto, IFormFile file)
    {
        var guid = Guid.NewGuid().ToString();
        string directoryPath = Path.Combine(AppContext.BaseDirectory, generalSettings.StudentDocumentsFolderName);

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
