using System;
using System.Collections.Generic;

namespace OptixMovies.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public string? Title { get; set; }

    public string? Overview { get; set; }

    public decimal? Popularity { get; set; }

    public int? VoteCount { get; set; }

    public decimal? VoteAverage { get; set; }

    public string? OriginalLanguage { get; set; }

    public string? Genre { get; set; }

    public string? PosterUrl { get; set; }

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}

public partial class MovieSearch
{
    public MovieSearch()
    { }

    public MovieSearch(Movie movie)
    {
        MovieId = movie.MovieId;
        
        if (movie.ReleaseDate.HasValue)
            ReleaseDate = movie.ReleaseDate.Value;
        else
            ReleaseDate = DateTime.MinValue;

        if (movie.Popularity.HasValue)
            Popularity = movie.Popularity.Value;
        else
            Popularity = 0m;

        if (movie.VoteAverage.HasValue)
            VoteAverage = movie.VoteAverage.Value;
        else
            VoteAverage = 0m;

        if (movie.VoteCount.HasValue)
            VoteCount = movie.VoteCount.Value;
        else
            VoteCount = 0;

        if (string.IsNullOrWhiteSpace(movie.Title))
            Title = ""; 
        else
            Title = movie.Title;

        if (string.IsNullOrWhiteSpace(movie.Overview))
            Overview = "";
        else
            Overview = movie.Overview;
        if (string.IsNullOrWhiteSpace(movie.OriginalLanguage))
            OriginalLanguage = "";
        else
            OriginalLanguage = movie.OriginalLanguage;
        if (string.IsNullOrWhiteSpace(movie.Genre))
            Genre = "";
        else
            Genre = movie.Genre;
        if (string.IsNullOrWhiteSpace(movie.PosterUrl))
            PosterUrl = "";
        else
            PosterUrl = movie.PosterUrl;

    }
    public int MovieId { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string Title { get; set; }

    public string Overview { get; set; }

    public decimal Popularity { get; set; }

    public int VoteCount { get; set; }

    public decimal VoteAverage { get; set; }

    public string OriginalLanguage { get; set; }

    public string Genre { get; set; }

    public string PosterUrl { get; set; }

}
