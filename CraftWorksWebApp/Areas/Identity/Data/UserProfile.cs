using CraftWorksWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserProfile
{
    [Key]
    public string UserId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
}
