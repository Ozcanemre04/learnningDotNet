using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet2.models
{
  public class UserAdress
  {
    
    [Key]
    public Guid id { get; set; }
    
    [StringLength(100)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(50)]
    public string? Country { get; set; }

    [ForeignKey("ApplicationUser")]
    public required string UserId {get;set;}
    public required  ApplicationUser ApplicationUser;
    }
}