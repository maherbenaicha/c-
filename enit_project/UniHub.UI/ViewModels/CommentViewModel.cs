using System.ComponentModel.DataAnnotations;

namespace UniHub.UI.ViewModels
{
    public class CommentViewModel
    {
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Le commentaire ne peut pas être vide.")]
        [StringLength(1000, ErrorMessage = "Maximum 1000 caractères.")]
        [Display(Name = "Votre commentaire")]
        public string Content { get; set; } = string.Empty;
    }
}
