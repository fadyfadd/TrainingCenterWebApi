using System;
using Data.Dtos;

namespace Data;

public class StudentDocumentDto : BaseStudentDocumentDto
{
    public StudentDto? Student {set; get;}

}
