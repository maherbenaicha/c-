using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniHub.BL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        [Display(Name = "Prénom")]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Nom")]
        public string? LastName { get; set; }

        public int? UsernameChangeLimit { get; set; } = 10;

        [Display(Name = "Photo de profil")]
        public byte[]? ProfilePicture { get; set; }

        [StringLength(100)]
        public string? ProfilePictureContentType { get; set; }

        [Display(Name = "Département")]
        public Department? Department { get; set; }

        [StringLength(20)]
        [Display(Name = "Numéro étudiant")]
        public string? StudentNumber { get; set; }

        // Navigation properties
        public virtual ICollection<Activity> CreatedActivities { get; set; } = new List<Activity>();
        public virtual ICollection<ActivityComment> Comments { get; set; } = new List<ActivityComment>();

        [NotMapped]
        [Display(Name = "Nom complet")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
