using System;
using System.Collections.Generic;

namespace OptixMovies.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreDesc { get; set; } = null!;

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
