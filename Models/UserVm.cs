using System.ComponentModel.DataAnnotations;

namespace TaskAssessment.Models
{
    public class UserVm
    {
        [Required]
        public required String Name { get; set; }
        [Required]
        [EmailAddress]
        public required String Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Password should contain at least 1 upper case, 1 lower case, 1 special character and 1 digit and minimum 8 charecters")]
        public required String Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public required String ConfirmPassword { get; set; }
    }
}
