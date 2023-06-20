﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebMvc.Data;

#nullable disable

namespace WebMvc.Migrations
{
    [DbContext(typeof(SqlDataBaseContext))]
    [Migration("20230619213049_add new feather to course")]
    partial class addnewfeathertocourse
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AccessesTeachers", b =>
                {
                    b.Property<Guid>("FK_accessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_teacherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FK_accessId", "FK_teacherId");

                    b.HasIndex("FK_teacherId");

                    b.ToTable("AccessesTeachers");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Accesses", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accesses");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Answers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_questionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_studentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FK_questionId");

                    b.HasIndex("FK_studentId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Courses", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_departmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_teacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("databseFieldId")
                        .HasColumnType("int");

                    b.Property<int>("databseId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FK_departmentId");

                    b.HasIndex("FK_teacherId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.CourseStudent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FK_CourseId");

                    b.HasIndex("FK_StudentId");

                    b.ToTable("CoursesStudents");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Departments", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_facultyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FK_facultyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Faculties", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Questions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_videoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("quest")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("questionTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("FK_videoId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Students", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_departmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FK_departmentId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Teachers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_departmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FK_departmentId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Videos", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FK_coursesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("startTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FK_coursesId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("AccessesTeachers", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Accesses", null)
                        .WithMany()
                        .HasForeignKey("FK_accessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebMvc.Models.Domain.Teachers", null)
                        .WithMany()
                        .HasForeignKey("FK_teacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Answers", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Questions", "FK_question")
                        .WithMany()
                        .HasForeignKey("FK_questionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebMvc.Models.Domain.Students", "FK_student")
                        .WithMany("FK_Answer")
                        .HasForeignKey("FK_studentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_question");

                    b.Navigation("FK_student");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Courses", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Departments", "FK_department")
                        .WithMany("FK_cours")
                        .HasForeignKey("FK_departmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebMvc.Models.Domain.Teachers", "FK_teacher")
                        .WithMany("FK_course")
                        .HasForeignKey("FK_teacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_department");

                    b.Navigation("FK_teacher");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.CourseStudent", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Courses", "FK_Course")
                        .WithMany("CourseStudent")
                        .HasForeignKey("FK_CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebMvc.Models.Domain.Students", "FK_Student")
                        .WithMany("CourseStudent")
                        .HasForeignKey("FK_StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_Course");

                    b.Navigation("FK_Student");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Departments", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Faculties", "FK_faculty")
                        .WithMany("FK_department")
                        .HasForeignKey("FK_facultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_faculty");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Questions", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Videos", "FK_video")
                        .WithMany("FK_questions")
                        .HasForeignKey("FK_videoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_video");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Students", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Departments", "FK_department")
                        .WithMany("FK_student")
                        .HasForeignKey("FK_departmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_department");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Teachers", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Departments", "FK_department")
                        .WithMany("FK_teacher")
                        .HasForeignKey("FK_departmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_department");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Videos", b =>
                {
                    b.HasOne("WebMvc.Models.Domain.Courses", "FK_courses")
                        .WithMany("FK_videos")
                        .HasForeignKey("FK_coursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_courses");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Courses", b =>
                {
                    b.Navigation("CourseStudent");

                    b.Navigation("FK_videos");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Departments", b =>
                {
                    b.Navigation("FK_cours");

                    b.Navigation("FK_student");

                    b.Navigation("FK_teacher");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Faculties", b =>
                {
                    b.Navigation("FK_department");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Students", b =>
                {
                    b.Navigation("CourseStudent");

                    b.Navigation("FK_Answer");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Teachers", b =>
                {
                    b.Navigation("FK_course");
                });

            modelBuilder.Entity("WebMvc.Models.Domain.Videos", b =>
                {
                    b.Navigation("FK_questions");
                });
#pragma warning restore 612, 618
        }
    }
}
