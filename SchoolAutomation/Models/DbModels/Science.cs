using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAutomation.Models.DbModels;

public class Science
{
    public  int Id { get; set; }
    public string  Name  { get; set; }
    public string  Description { get; set; }
}
