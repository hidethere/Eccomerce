using System.ComponentModel.DataAnnotations;

namespace User_service.Domain
{
    public enum UserRole
    {
        User,
        Admin
    }
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(20,MinimumLength = 4)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength (128)]
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }
}
