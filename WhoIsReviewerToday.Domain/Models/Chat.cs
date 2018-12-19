using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoIsReviewerToday.Domain.Models
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long TelegramChatId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FullName { get; set; }
    }
}