using SchoolAutomation.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolAutomation.Models;

public class LoginModel
{
    [Required,Length(6,20)]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    public RoleType Role { get; set; }
}
