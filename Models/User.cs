using System.ComponentModel.DataAnnotations;

namespace Harjoitustyo.Models
{
    public class User
    {
        public long Id { get; set; }
        [MaxLength(100)]
        public string? Username { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Password { get; set; } = string.Empty;
        public byte[]? Salt { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? LastName { get; set; } = string.Empty;
        [MaxLength(100)]
        public DateTime? JoinedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public static implicit operator User?(Message? v)
        {
            throw new NotImplementedException();
        }
    }
    public class UserDTO // Verkon yli lähetettävä
    {
        public string? UserName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? LastName { get; set; } = string.Empty;
        [MaxLength(100)]
        public DateTime? JoinedAt { get; set; }
        public DateTime? LastLogin { get; set; }

    }
}
