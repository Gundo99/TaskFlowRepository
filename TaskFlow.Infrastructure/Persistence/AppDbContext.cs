using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Tasks;
using TaskFlow.Domain.Users;

namespace TaskFlow.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                builder.OwnsOne(x => x.Email, email =>
                {
                    email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(256); ;
                });
            });
            modelBuilder.Entity<TaskItem>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                builder.Property(x => x.Description)
                    .HasMaxLength(1000);
                builder.Property(x => x.IsCompleted)
                    .IsRequired();
                builder.Property(x => x.CreatedAt)
                .IsRequired();
                builder.Property(x => x.UserId)
                    .IsRequired();
                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
