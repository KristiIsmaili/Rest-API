using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Rest_API.Models
{
    public partial class kreatxTestContext : DbContext
    {
        public kreatxTestContext()
        {
        }

        public kreatxTestContext(DbContextOptions<kreatxTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=kreatxTestContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ContentType).HasMaxLength(100);

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Images__UserID__49C3F6B7");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.CreationDate).HasColumnType("date");

                entity.Property(e => e.ProjectIsDeleted).HasColumnName("projectIsDeleted");

                entity.Property(e => e.ProjectName).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("date");

                entity.HasOne(d => d.ProjectAdminNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProjectAdmin)
                    .HasConstraintName("FK__Projects__Projec__3E52440B");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.AssignedUserId).HasColumnName("AssignedUserID");

                entity.Property(e => e.AssignetEmployee).HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("date");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.IsCompleted).HasMaxLength(10);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.Property(e => e.TaskIsDeleted).HasColumnName("taskIsDeleted");

                entity.HasOne(d => d.AssignedUser)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.AssignedUserId)
                    .HasConstraintName("FK__Tasks__AssignedU__412EB0B6");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Tasks__ProjectID__403A8C7D");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Role).HasMaxLength(20);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
