using mfc_tomsk.Models;
using Microsoft.EntityFrameworkCore;

namespace mfc_tomsk
{
    public partial class mfcContext : DbContext
    {
        public mfcContext()
        {
        }

        public mfcContext(DbContextOptions<mfcContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<PassworD> PassworDs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=mfc.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<PassworD>(entity =>
            {
                entity.ToTable("PassworD");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.IdAdminNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdAdmin);

                entity.HasOne(d => d.IdDepartmentNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdDepartment);

                entity.HasOne(d => d.IdPositionNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdPosition);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
