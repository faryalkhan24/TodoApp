using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToDoWebApp.Models.DAL;

public partial class ToDoApplicationContext : DbContext
{
    public ToDoApplicationContext()
    {
    }

    public ToDoApplicationContext(DbContextOptions<ToDoApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-ERV6E6M\\SQLEXPRESSGROUP;Database=ToDoApplication;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__71B26F2D1EB8A0BF");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ForDate).HasColumnType("datetime");
            entity.Property(e => e.TaskDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TaskName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tasks__UserId__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C03169278");

            entity.ToTable("User");

            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
