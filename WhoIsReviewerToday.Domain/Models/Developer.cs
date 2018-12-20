using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoIsReviewerToday.Domain.Models
{
    public class Developer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FullName { get; set; }

        public Team Team { get; set; }

        public Chat Chat { get; set; }

        public long? TelegramUserId { get; set; }
    }
}