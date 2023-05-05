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
        public DbSet<Support> Supports { get; set; }
        public DbSet<Nofication> Nofications { get; set; }
        public DbSet<MyDirect> MyDirects { get; set; }
        public DbSet<Message> Messages { get; set; }

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

            modelBuilder.Entity<Nofication>()
                .HasOne(n => n.User)
                .WithMany(u => u.Nofications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Nofication>()
                .HasOne(n => n.FromUser)
                .WithMany()
                .HasForeignKey(n => n.FromUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
            modelBuilder.Entity<MyDirect>()
                .HasOne(d => d.AppUser)
                .WithMany(u => u.Directs)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MyDirect>()
                .HasOne(d => d.WriteingWithUser)
                .WithMany()
                .HasForeignKey(d => d.WriteingWithUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MyDirect>()
                .HasMany(d => d.Messages)
                .WithOne(m => m.MyDirect)
                .HasForeignKey(m => m.MyDirectId)
                .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
