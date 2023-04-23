using Final_Project_Tenslog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Tenslog.DataAccessLayer
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Saved> Saveds { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Following> Followings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });

            modelBuilder.Entity<Following>()
                .HasKey(f => new { f.UserId, f.UserFollowingId });

            modelBuilder.Entity<Following>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followings)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follower>()
                .HasKey(f => new { f.UserId, f.UserFollowerId });

            modelBuilder.Entity<Follower>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
