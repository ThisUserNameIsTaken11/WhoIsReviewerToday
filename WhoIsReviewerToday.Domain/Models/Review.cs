using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoIsReviewerToday.Domain.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public long DeveloperId { get; set; }

        [ForeignKey("DeveloperId")]
        public virtual Developer Developer { get; set; }
    }
}