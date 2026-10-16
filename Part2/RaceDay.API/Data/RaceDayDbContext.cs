using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;
using RaceRoute = RaceDay.API.Models.Route;

namespace RaceDay.API.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext
            > options) : base(options)

        {
        }

   
    public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventCategory> EventCategories => Set<EventCategory>();
        public DbSet<Enrolment> Enrolments => Set<Enrolment>();
        public DbSet<Result> Results => Set<Result>();
        public DbSet<RaceRoute> Routes => Set<RaceRoute>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User table
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired();
            });

            // Category table
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryName).IsRequired();
            });

            // Event table
            modelBuilder.Entity<RaceDay.API.Models.Event>(entity =>
            {
                entity.ToTable("Event");
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.EventName).IsRequired();
                entity.Property(e => e.EventDate).HasColumnType("date");
                entity.Property(e => e.StartTime).HasColumnType("time");
                entity.Property(e => e.Location).IsRequired();
                entity.Property(e => e.Status).IsRequired();

                entity.HasOne(e => e.Organiser)
                    .WithMany(u => u.OrganisedEvents)
                    .HasForeignKey(e => e.OrganiserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // EventCategory table
            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.ToTable("EventCategory");
                entity.HasKey(e => e.EventCategoryId);

                entity.Property(e => e.EntryFee).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Event)
                    .WithMany(e => e.EventCategories)
                    .HasForeignKey(e => e.EventId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.EventCategories)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Enrolment table
            modelBuilder.Entity<Enrolment>(entity =>
            {
                entity.ToTable("Enrolment");
                entity.HasKey(e => e.EnrolmentId);

                entity.Property(e => e.EnrolmentDate).HasColumnType("date");
                entity.Property(e => e.EnrolmentStatus).IsRequired();

                entity.HasOne(e => e.Participant)
                    .WithMany(u => u.Enrolments)
                    .HasForeignKey(e => e.ParticipantId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.EventCategory)
                    .WithMany(ec => ec.Enrolments)
                    .HasForeignKey(e => e.EventCategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Result table
            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Result");
                entity.HasKey(e => e.ResultId);
                entity.Property(e => e.FinishTime).HasColumnType("time");
                entity.Property(e => e.RecordDate).HasColumnType("date");

                entity.HasOne(e => e.Enrolment)
                    .WithMany(en => en.Results)
                    .HasForeignKey(e => e.EnrolmentId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Route table
            modelBuilder.Entity<RaceDay.API.Models.Route>(entity =>
            {
                entity.ToTable("Route");
                entity.HasKey(e => e.RouteId);

                entity.Property(e => e.DistanceKM).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Event)
                    .WithMany(e => e.Routes)
                    .HasForeignKey(e => e.EventId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

    }
}