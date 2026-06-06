namespace Data.Dtos;

public class DownloadFileDto  
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }  
    public Stream ContentStream {set; get;}
     
}
