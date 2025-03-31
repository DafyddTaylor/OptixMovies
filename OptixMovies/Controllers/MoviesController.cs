using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptixMovies.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
//using System.Web.Http;

namespace OptixMovies.Controllers
{
   // [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {

        private enum OrderFields  { TitleAsc, TitleDesc, DateAsc, DateDesc};

        private OptixMovieDbContext dbContext = new OptixMovieDbContext();

        [HttpGet]
        [Route("api/[controller]/GenreList")]
        public IEnumerable<Models.Movie> GetMovieGenres([FromBody] string[] pGenre)
        {
            var genreList = dbContext.Genres.Where(o => pGenre.ToList().Contains(o.GenreDesc)).Select(o => o.GenreId);


            var ret = dbContext.Movies.Where(o => o.MovieGenres.Any(g => genreList.Contains(g.Genre))).ToList();

            return ret;
        }

        [HttpGet]
        [Route("api/[controller]/Search/")]
        public IEnumerable<Models.Movie> SearchMovies([FromQuery] int pageSize = 0, [FromQuery] int pageNum = 0, [FromQuery] string Title = "", [FromQuery] string pGenre = "", [FromQuery] string Actors = "", [FromQuery] string OrderBy = "")
        {

            string[] gList = pGenre.Split(',');
            IEnumerable<Models.Movie> ret = null;

            if (gList.Length > 0)
            {
                ret = dbContext.Movies.Where(o =>
                    (string.IsNullOrWhiteSpace(pGenre) || (null != o.Genre && o.Genre.Contains(pGenre)))
                    ).ToList();
            }
            else
            {
                ret = dbContext.Movies;
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                ret = FilterMovieTitle(ret, Title);
            }

            if (ret.Count() > 0 && !string.IsNullOrWhiteSpace(Actors))
            {
                var actorList = Actors.Split(',').ToList();
                List<Models.Movie> actorsret = new List<Movie>();
                foreach (var actor in actorList)
                {
                    actorsret.AddRange(ret.Where(o => o.Overview != null && o.Overview.Contains(actor)).ToList());
                }

                ret = actorsret;
            }

            return SortAndPage(ret, OrderBy, pageSize, pageNum);
        }

        private IEnumerable<Models.Movie> SortAndPage(IEnumerable<Models.Movie> InMovies, String OrderBy, int pageSize, int pageNum)
        {
            IEnumerable<Models.Movie> ret = InMovies;
            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                OrderFields ordering;
                if (Enum.TryParse<OrderFields>(OrderBy, out ordering))
                {
                    switch (ordering)
                    {
                        case OrderFields.TitleAsc:
                            ret = InMovies.OrderBy(o => o.Title).ToList();
                            break;
                        case OrderFields.DateAsc:
                            ret = InMovies.OrderBy(o => o.ReleaseDate).ToList();
                            break;
                        case OrderFields.TitleDesc:
                            ret = InMovies.OrderByDescending(o => o.Title).ToList();
                            break;
                        case OrderFields.DateDesc:
                            ret = InMovies.OrderByDescending(o => o.ReleaseDate).ToList();
                            break;
                    }
                }

            }

            if (pageSize > 0)
            {
                if (pageNum < 0) pageNum = 0;
                int start = pageSize * pageNum;
                int end = start + pageSize;

                if (start >= ret.Count())
                    return new List<Movie>();

                if (end >= ret.Count())
                    end = ret.Count() - 1;

                if (end <= start)
                    return new List<Movie>();

                Range retRange = new Range(start, end);

                return ret.Take(retRange);
            }

            return ret;
        }

        private IEnumerable<Models.Movie> FilterMovieTitle(IEnumerable<Models.Movie> InMovies, string Title)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            List<Movie> ret = InMovies.Where(o => o.Title.ToUpper() == Title.ToUpper()).ToList();

            foreach (var movie in InMovies.Where(o => o.Title.ToUpper() != Title.ToUpper() &&  o.Title != null))
            {
                string normTitle = rgx.Replace(movie.Title.ToUpper(), "*");

                List<string> tParts = normTitle.ToUpper().Split('*').ToList();

                if (tParts.Count == 1 && movie.Title.ToUpper().Contains(Title.ToUpper()) || Title.ToUpper().Contains(movie.Title.ToUpper()))
                {
                    ret.Add(movie);
                }
                else if (tParts.Any(o => !Title.Contains(o)))
                {
                    continue;
                }
                else
                {
                    ret.Add(movie);
                }
            }

            return ret;
        }


        [HttpGet]
        [Route("api/[controller]/Title/")]
        public IEnumerable<Models.Movie> GetMovieTitle([FromQuery][MaxLength(50)] string Title, [FromQuery] int pageSize = 0, [FromQuery] int pageNum = 0, [FromQuery][MaxLength(20)] string OrderBy = "")
        {
            var ret = FilterMovieTitle(dbContext.Movies, Title);

            return SortAndPage(ret, OrderBy, pageSize, pageNum);
        }
        [HttpGet]
        [Route("api/[controller]/Genre/")]
        public IEnumerable<Models.Movie> GetMovieGenre([FromQuery][MaxLength(50)] string pGenre, [FromQuery] int pageSize = 0, [FromQuery] int pageNum = 0, [FromQuery][MaxLength(20)] string OrderBy = "")
        {
            var ret = dbContext.Movies.Where(o => null != o.Genre && o.Genre.Contains(pGenre)).ToList();

            return SortAndPage(ret, OrderBy, pageSize, pageNum);
        }

        [HttpPost]
        [Route("api/[controller]/GenreListTEST")]
        public IEnumerable<Models.Movie> GetMoviesTEST([FromBody][MaxLength(50)] string pGenre)
        {
            string[] gList = pGenre.Split('|');

            var ret = GetMovieGenres(gList);

            return ret;
        }
    }
}
