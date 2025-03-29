using System;
using System.Collections.Generic;

namespace OptixMovies.Models;

public partial class MovieGenre
{
    public int MovieGenreId { get; set; }

    public int Genre { get; set; }

    public int Movie { get; set; }

    public virtual Genre GenreNavigation { get; set; } = null!;

    public virtual Movie MovieNavigation { get; set; } = null!;
}
