using Microsoft.EntityFrameworkCore;
using Anonim.Models;

namespace Anonim.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Hikayeler> Hikayeler { get; set; }
        public DbSet<Itiraf> Itiraflar { get; set; }
        public DbSet<ItirafBegeni> ItirafBegeniler { get; set; }
        public DbSet<HikayeBegeni> HikayeBegeniler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Itiraf beğeni ayarları
            modelBuilder.Entity<ItirafBegeni>()
                .HasIndex(b => new { b.ItirafId, b.UserId })
                .IsUnique();

            modelBuilder.Entity<ItirafBegeni>()
                .HasOne(b => b.Itiraf)
                .WithMany()
                .HasForeignKey(b => b.ItirafId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItirafBegeni>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Hikaye beğeni ayarları
            modelBuilder.Entity<HikayeBegeni>()
                .HasIndex(b => new { b.HikayeId, b.UserId })
                .IsUnique();

            modelBuilder.Entity<HikayeBegeni>()
                .HasOne(b => b.Hikaye)
                .WithMany()
                .HasForeignKey(b => b.HikayeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HikayeBegeni>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
