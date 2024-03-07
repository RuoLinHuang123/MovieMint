using Microsoft.EntityFrameworkCore;
using System.IO;


namespace MovieMint.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movies_Genres>()
                .HasKey(i => new { i.MovieId, i.GenreId });

            modelBuilder.Entity<Movies_Genres>()
                .HasOne(x => x.Movie)
                .WithMany(y => y.Movies_Genres)
                .HasForeignKey(f => f.MovieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movies_Genres>()
                .HasOne(o => o.Genre)
                .WithMany(m => m.Movies_Genres)
                .HasForeignKey(f => f.GenreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movies_Stars>()
                .HasKey(i => new { i.MovieId, i.StarId });

            modelBuilder.Entity<Movies_Stars>()
                .HasOne(x => x.Movie)
                .WithMany(y => y.Movies_Stars)
                .HasForeignKey(f => f.MovieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movies_Stars>()
                .HasOne(x => x.Star)
                .WithMany(y => y.Movies_Stars)
                .HasForeignKey(f => f.StarId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movie>()
                .HasOne(x => x.Certificate)
                .WithMany(y => y.Movies)
                .HasForeignKey(f => f.CertificateId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Movie>()
                .HasOne(x => x.Director)
                .WithMany(y => y.Movies)
                .HasForeignKey(f => f.DirectorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Certificate> Certificates => Set<Certificate>();
        public DbSet<Director> Directors => Set<Director>();
        public DbSet<Star> Stars => Set<Star>();
        public DbSet<Movies_Genres> Movies_Genres => Set<Movies_Genres>();
        public DbSet<Movies_Stars> Movies_Stars => Set<Movies_Stars>();
    }
}
