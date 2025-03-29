using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OptixMovies.Models;

public partial class OptixMovieDbContext : DbContext
{
    public OptixMovieDbContext()
    {
    }

    public OptixMovieDbContext(DbContextOptions<OptixMovieDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<Mymoviedb> Mymoviedbs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=OptixMovieDB;User Id=MovieDBUser;Password=@Password123;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.GenreDesc)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.Genre)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Original_Language");
            entity.Property(e => e.Overview)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Popularity).HasColumnType("decimal(28, 3)");
            entity.Property(e => e.PosterUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Poster_Url");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("Release_Date");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VoteAverage)
                .HasColumnType("decimal(28, 3)")
                .HasColumnName("Vote_Average");
            entity.Property(e => e.VoteCount).HasColumnName("Vote_Count");
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.ToTable("MovieGenre");

            entity.Property(e => e.MovieGenreId).HasColumnName("MovieGenreID");

            entity.HasOne(d => d.GenreNavigation).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.Genre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieGenre_Genre");

            entity.HasOne(d => d.MovieNavigation).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.Movie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieGenre_Movies");
        });

        modelBuilder.Entity<Mymoviedb>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("mymoviedb");

            entity.Property(e => e.Genre)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Original_Language");
            entity.Property(e => e.Overview)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Popularity).HasColumnType("decimal(28, 3)");
            entity.Property(e => e.PosterUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Poster_Url");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("Release_Date");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VoteAverage)
                .HasColumnType("decimal(28, 3)")
                .HasColumnName("Vote_Average");
            entity.Property(e => e.VoteCount).HasColumnName("Vote_Count");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
