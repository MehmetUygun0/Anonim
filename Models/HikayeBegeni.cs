using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anonim.Models
{
    public class HikayeBegeni
    {
        [Key]
        public int Id { get; set; }

        public int HikayeId { get; set; }
        [ForeignKey(nameof(HikayeId))]
        public Hikayeler? Hikaye { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public Users? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
