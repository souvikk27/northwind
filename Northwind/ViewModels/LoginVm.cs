using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Northwind.ViewModels
{
    public class LoginVm
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}