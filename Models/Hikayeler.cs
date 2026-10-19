using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Anonim.Models
{
    public class Hikayeler
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "Hikaye title must be less than 50 characters.")]
        public string Title { get; set; }
        [MaxLength(1000, ErrorMessage = "Hikaye text must be less than 1000 characters.")]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int LikeCount { get; set; } = 0;

        public int UserId { get; set; }
        public Users User { get; set; } 
    }
}
