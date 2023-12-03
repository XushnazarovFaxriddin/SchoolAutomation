using SchoolAutomation.Enums;
using SchoolAutomation.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAutomation.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, Length(3, 15)]
        public string FirstName { get; set; }

        [Required, Length(3, 15)]
        public string LastName { get; set; }

        [Required, Length(6, 20)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }
        public RoleType Role { get; set; }

        public Teacher ToTeacher()
        {
            return new Teacher
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Username = this.Username,
                Password = this.Password,
                Role = this.Role
            };
        }

        public Student ToStudent()
        {
            return new Student
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Username = this.Username,
                Password = this.Password,
                Role = this.Role
            };
        }

        public Admin ToAdmin()
        {
            return new Admin
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Username = this.Username,
                Password = this.Password,
                Role = this.Role
            };
        }
    }
}
