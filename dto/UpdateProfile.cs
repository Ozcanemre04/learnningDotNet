using System.ComponentModel.DataAnnotations;

namespace dotnet2.dto;

    public class UpdateProfile
    {
        [Required(ErrorMessage="firstname Is required")]
        [StringLength(50)]
        [MinLength(3)]
        public  string? firstName {get;set;}
        
        [Required(ErrorMessage="author is required")]
        [StringLength(10)]
        [MinLength(3)]
        public  string? lastName {get;set;}
        
        public UpdateAdressDto? Adress{ get; set; }


    }
  


      public class UpdateAdressDto
    {
        [Required(ErrorMessage="country Is required")]
        [MaxLength(50)]
        [MinLength(3)]
        public required string Country { get; set; }

        [Required(ErrorMessage="adress Is required")]
        [StringLength(100)]
        [MinLength(3)]
        public required string Address { get; set; }

        [Required(ErrorMessage="city Is required")]
        [StringLength(50)]
        [MinLength(3)]
        public required string City { get; set; }

        [Required(ErrorMessage="state Is required")]
        [StringLength(50)]
        [MinLength(3)]
        public required string State { get; set; }
       
    }