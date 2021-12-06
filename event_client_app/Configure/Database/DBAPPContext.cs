using System.Data;
using event_client_app.Models;
using Microsoft.EntityFrameworkCore;

namespace event_client_app
{
    public class DBAPPContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<AttendanceStatus> AttendanceStatus { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<EventAttendees> EventAttendees { get; set; }
        public DbSet<EventCategory> EventCategory { get; set; }

        // should be public in the method 
        // does not know why should ask ??????????????
        public DBAPPContext(DbContextOptions<DBAPPContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // user's email is unique
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Event>().HasIndex(e => new {e.Title, e.Date, e.OrganizerId}).IsUnique();
            modelBuilder.Entity<Event>().HasOne(e => e.Organizer)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.OrganizerId)
                .HasPrincipalKey(u => u.UserId);


            modelBuilder.Entity<EventCategory>().HasIndex(ec => new {ec.CategoryId, ec.EventId}).IsUnique();
            // modelBuilder.Entity<EventCategory>().HasOne(ec => ec.Categories)
            //     .WithMany(c => c.EventCategories)
            //     .HasForeignKey(ec => ec.CategoryId)
            //     .HasPrincipalKey(c => c.Id);

            modelBuilder.Entity<EventAttendees>().HasOne(e => e.Status)
                .WithMany(status => status.EventAttendee)
                .HasForeignKey(e => e.AttendanceStatusId)
                .HasPrincipalKey(status => status.Id);

            // modelBuilder.Entity<Event>().HasOne<User>().WithMany().HasForeignKey(e => e.OrganizerId);


            modelBuilder.Entity<EventAttendees>().HasIndex(ea => new {ea.EventId, ea.UserId}).IsUnique();
            // modelBuilder.Entity<EventAttendees>().HasOne(ea => ea.Event)
            //     .WithMany(e => e.EventAttendees)
            //     .HasForeignKey(ea => ea.EventId)
            //     .HasPrincipalKey(e => e.EventId);
            //
            // modelBuilder.Entity<EventAttendees>().HasOne(ea => ea.Status)
            //     .WithOne(a => a.EventAttendee)
            //     .HasForeignKey<EventAttendees>(ea => ea.AttendanceStatusId)
            //     .HasPrincipalKey<AttendanceStatus>(a => a.Id);

            modelBuilder.Entity<AttendanceStatus>().HasIndex(status => status.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
        }
    }
}