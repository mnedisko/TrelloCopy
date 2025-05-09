using System.ComponentModel.DataAnnotations;

namespace TrelloCopy.Models
{
    public class UserDataViewModel
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string UserName { get; set; }
        public  string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string  Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifreler Eşleşmelidir")]

        public string PasswordAgain { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = "User";
    }
}
