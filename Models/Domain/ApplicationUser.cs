using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public int? ImageId { get; set; }
        
        // Navigation property
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}
