using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using lapp3.Models;

namespace lapp3.Data;

public partial class NykopingsgymnasiumContext : DbContext
{
    public NykopingsgymnasiumContext()
    {
    }

    public NykopingsgymnasiumContext(DbContextOptions<NykopingsgymnasiumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseGrade> CourseGrades { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SIEMTEAGHESOGUB;Database=Nykopingsgymnasium;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__class__CB1927A09223E2D4");

            entity.ToTable("class");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ResponsibleTeacherId).HasColumnName("ResponsibleTeacherID");

            entity.HasOne(d => d.ResponsibleTeacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.ResponsibleTeacherId)
                .HasConstraintName("FK__class__Responsib__398D8EEE");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D718746827ADC");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CourseGrade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__CourseGr__54F87A37051C6D25");

            entity.ToTable("CourseGrade");

            entity.Property(e => e.GradeId).HasColumnName("GradeID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Grade)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.StudntId).HasColumnName("StudntID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseGrades)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CourseGra__Cours__412EB0B6");

            entity.HasOne(d => d.Studnt).WithMany(p => p.CourseGrades)
                .HasForeignKey(d => d.StudntId)
                .HasConstraintName("FK__CourseGra__Studn__4222D4EF");

            entity.HasOne(d => d.Teacher).WithMany(p => p.CourseGrades)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__CourseGra__Teach__4316F928");
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.PersonalId).HasName("PK__Personal__2834371337F69C4C");

            entity.ToTable("Personal");

            entity.Property(e => e.PersonalId).HasColumnName("PersonalID");
            entity.Property(e => e.FristName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SocialSecurityNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A797468AE4B");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.FristName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SocialSecurityNumber)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Student__ClassID__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
