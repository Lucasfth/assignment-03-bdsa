// See https://aka.ms/new-console-template for more information
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities
{
    public partial class KanbanContext : DbContext
    {
        public KanbanContext(DbContextOptions<KanbanContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.AssignedTo);
                entity.Property(e => e.Description);
                entity.Property(e => e.State);
                entity.Property(e => e.Tags);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Tasks);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Tasks);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}