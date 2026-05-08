using System.ComponentModel.DataAnnotations;

namespace UniHub.BL.Entities
{
    public class ActivityComment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Commentaire")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Date de création")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Keys
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
