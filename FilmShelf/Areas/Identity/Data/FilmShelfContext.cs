using FilmShelf.Areas.Identity.Data;
using FilmShelf.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmShelf.Areas.Identity.Data
{
    public class FilmShelfContext : IdentityDbContext<FilmShelfUser>
    {
        public FilmShelfContext(DbContextOptions<FilmShelfContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Rental> Rental { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new FilmShelfUserEntityConfiguration());
            builder.ApplyConfiguration(new MovieEntityConfiguration());

            // Apply Rental entity configuration
            builder.Entity<Rental>(entity =>
            {
                entity.HasKey(r => r.RentalID);
                entity.Property(r => r.RentDate).HasColumnType("date");
                entity.Property(r => r.ReturnDate).HasColumnType("date");

                // Define foreign key relationship with Movie
                entity.HasOne(r => r.Movie)
                      .WithMany()  // No navigation property in Movie for Rental
                      .HasForeignKey(r => r.MovieID)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

    public class FilmShelfUserEntityConfiguration : IEntityTypeConfiguration<FilmShelfUser>
    {
        public void Configure(EntityTypeBuilder<FilmShelfUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(50);
            builder.Property(u => u.LastName).HasMaxLength(50);
        }
    }

    public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.MovieID);
            builder.Property(m => m.Title).IsRequired().HasMaxLength(50);
            builder.Property(m => m.Description).HasMaxLength(500);
            builder.Property(m => m.ReleaseDate).HasColumnType("date");
            builder.Property(m => m.Price).HasColumnType("decimal(18,2)");
            builder.Property(m => m.ImageUrl).HasMaxLength(255);

            // Define foreign key relationship with Genre
            builder.HasOne(m => m.Genre)
                   .WithMany(g => g.Movies)
                   .HasForeignKey(m => m.GenreID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


