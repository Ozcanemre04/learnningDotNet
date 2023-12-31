using Microsoft.EntityFrameworkCore;
using dotnet2.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace dotnet2.data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>  //||DbContext if you dont use identity
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
                   AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }
        public required DbSet<Books> books { get; set; }
            
        public required DbSet<UserAdress> Adress { get; set; }
        public required DbSet<RefreshToken> RefreshToken { get; set; }




       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>()
            .HasKey(i=>i.Id);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne<UserAdress>(s => s.UserAdress)
            .WithOne(ad => ad.ApplicationUser)
            .HasForeignKey<UserAdress>(ad => ad.UserId);

      

        modelBuilder.Entity<ApplicationUser>()
            .HasOne<RefreshToken>(s => s.RefreshToken)
            .WithOne(ad => ad.ApplicationUser)
            .HasForeignKey<RefreshToken>(ad => ad.UserId);
        
        }

    }
}