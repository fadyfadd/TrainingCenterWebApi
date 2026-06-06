using System;
using Data.Entities;

namespace Data.Dtos;

public class StudentDocument
{
    public int Id { get; set; }
    public string DocumentName { get; set; }
    public string DocumentUrl { get; set; }
    public DateTime UploadedAt { get; set; }
    public int StudentId { set; get; }
    public Student Student { set; get; }
}
