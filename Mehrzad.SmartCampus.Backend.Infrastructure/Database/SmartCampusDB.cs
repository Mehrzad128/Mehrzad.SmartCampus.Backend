using Mehrzad.SmartCampus.Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Infrastructure.Database
{
    public class SmartCampusDB : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public SmartCampusDB(DbContextOptions<SmartCampusDB> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>().HasKey(m => m.ID);
            modelBuilder.Entity<Student>().HasKey(m => m.ID);
            modelBuilder.Entity<Faculty>().HasKey(m => m.ID);
            modelBuilder.Entity<Event>().HasKey(m => m.ID);
            modelBuilder.Entity<Enrollment>().HasKey(m => m.ID);
            modelBuilder.Entity<Course>().HasKey(m => m.ID);
            modelBuilder.Entity<Booking>().HasKey(m => m.ID);
            modelBuilder.Entity<Attendance>().HasKey(m => m.ID);
            modelBuilder.Entity<Admin>().HasKey(m => m.ID);
        }
    }
}
