using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anonim.Models
{
    public class ItirafBegeni
    {
        [Key]
        public int Id { get; set; }

        public int ItirafId { get; set; }
        [ForeignKey(nameof(ItirafId))]
        public Itiraf? Itiraf { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public Users? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
