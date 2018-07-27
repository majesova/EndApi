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
            //Granted follower permission
            modelBuilder.Entity<GrantedFollowerPermission>().HasKey(x=>new {x.FollowingId,x.Key});
            //Following
            modelBuilder.Entity<Following>().HasOne(x=>x.FollowedUser).WithMany().HasForeignKey(x=>x.FollowedUserId).IsRequired();
            modelBuilder.Entity<Following>().HasMany(x=>x.GrantedPermissions).WithOne(x=>x.Following).HasForeignKey(x=>x.FollowingId);
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
            //Following Request
            modelBuilder.Entity<FollowingRequest>().HasOne(x=>x.Requester).WithMany().HasForeignKey(x=>x.RequesterId).IsRequired();
            modelBuilder.Entity<FollowingRequest>().HasOne(x=>x.Followed).WithMany().HasForeignKey(x=>x.FollowedId).IsRequired();

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
        public DbSet<FollowerPermission> FollowerPermissions { get; set; }
        public DbSet<FollowingRequest> FollowingRequests { get; set; }
        public DbSet<GrantedFollowerPermission> GrantedFollowerPermissions { get; set; }
    }
}