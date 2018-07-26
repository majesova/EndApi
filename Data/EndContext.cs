using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EndApi.Data
{
    public class EndContext:IdentityDbContext<AppUser, AppRole, string>
    {
        public EndContext(DbContextOptions<EndContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
             base.OnModelCreating(modelBuilder);
             modelBuilder.Entity<AppUser>().ToTable("AppUsers");
             modelBuilder.Entity<AppRole>().ToTable("AppRoles");
             
            //EndUser
            modelBuilder.Entity<EndUser>().HasMany(x=>x.Followings).WithOne(x=>x.FollowedUser).HasForeignKey(x=>x.FollowedUserId).IsRequired();
            //Following
            modelBuilder.Entity<Following>().HasOne(x=>x.FollowedUser).WithMany().HasForeignKey(x=>x.FollowedUserId).IsRequired();
            //Measurement
            modelBuilder.Entity<MeasurementRevision>().HasKey(x=>new {x.Key,x.RevisionId});
            modelBuilder.Entity<MeasurementRevision>().HasOne(x=>x.Revision)
            .WithMany(x=>x.Measurements).HasForeignKey(x=>x.RevisionId).IsRequired();
            //Revision
            modelBuilder.Entity<Revision>().HasOne(x=>x.AssignedUser).WithMany().HasForeignKey(x=>x.AssignedUserId).IsRequired(false);
            modelBuilder.Entity<Revision>().HasOne(x=>x.CreatedBy).WithMany().HasForeignKey(x=>x.CreatedById).IsRequired();
            //Plan
            modelBuilder.Entity<PlanNutritional>().HasMany(x=>x.Foods).WithOne(x=>x.Plan).HasForeignKey(x=>x.PlanId);
            modelBuilder.Entity<PlanNutritional>().HasMany(x=>x.PlanProperties).WithOne(x=>x.Plan).HasForeignKey(x=>x.PlanId);


        }

        public DbSet<Measure> Measurements{ get;set; }
        public DbSet<NutritionProperty> NutritionProperties { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }
        public DbSet<MeasurementRevision> RevisonMeasurements { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<PlanNutritional> Plans { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodPicture> FoodPicture { get; set; }
        public DbSet<PlanProperty> PlanProperties { get; set; }
    }
}