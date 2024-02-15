using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskAssessment.Areas.Identity.Data
{
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required int EmployeeID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Department { get; set; }
    }
}
