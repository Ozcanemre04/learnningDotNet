using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet2.models
{   
    public class RefreshToken
    {
        [Key]
        public Guid id { get; set; }
        public required string refreshToken {get;set;}
        public DateTime expires {get;set;}

        [ForeignKey("ApplicationUser")]
        public required string UserId {get;set;}
        public required  ApplicationUser ApplicationUser;
    }
}