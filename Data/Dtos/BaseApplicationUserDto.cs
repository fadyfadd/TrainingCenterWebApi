
using System.ComponentModel.DataAnnotations;

namespace Data.Dtos;

public class BaseApplicationUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    [EmailAddress]
    public String Email { set; get; } = "";
    public String PlainPassword { get; set; } = "";
 
}
