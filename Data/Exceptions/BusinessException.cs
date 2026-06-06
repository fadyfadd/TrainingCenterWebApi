
namespace Data.Exceptions;

public class BusinessException : Exception
{
    public string ServerCode { set; get; }
  
    public Dictionary<string, string[]>? Errors;


    public BusinessException(string serverCode, string message, Exception innerException, Dictionary<string, string[]>? errors) : base(message, innerException)
    {
        this.ServerCode = serverCode;      
        this.Errors = errors;
    }
}
