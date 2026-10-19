using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Anonim.Models
{
    [Index(nameof(NickName), IsUnique = true)]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string NickName { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; }
    }
}
