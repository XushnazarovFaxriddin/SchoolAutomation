using SchoolAutomation.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolAutomation.Models
{
    public class RegisterModel
    {
        [Required, Length(3, 15)]
        public string FirstName { get; set; }

        [Required, Length(3, 15)]
        public string LastName { get; set; }

        [Required, Length(6, 20)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [DeniedValues(RoleType.Admin)]
        //[AllowedValues(RoleType.Teacher, RoleType.Student)]
        public RoleType Role { get; set; }
    }
}
