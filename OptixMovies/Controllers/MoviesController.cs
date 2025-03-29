using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptixMovies.Models;
using System.Linq;
using System.Net.Http.Headers;
//using System.Web.Http;

namespace OptixMovies.Controllers
{
   // [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {

        private string[] OrderFields = { "Title", "Date" };

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
        public IEnumerable<Models.Movie> SearchMoves([FromQuery] int pageSize=0, [FromQuery] int pageNum=0, [FromQuery] string Title="", [FromQuery] string pGenre = "", [FromQuery] string Actors = "", [FromQuery] string OrderBy="")
        {
            var ret = dbContext.Movies.Where(o => null!=o.Genre && o.Genre.Contains(pGenre)).ToList();

            return ret;
        }

        [HttpGet]
        [Route("api/[controller]/Genre/")]
        public IEnumerable<Models.Movie> GetMovieGenre([FromQuery] string pGenre)
        {
            var ret = dbContext.Movies.Where(o => null != o.Genre && o.Genre.Contains(pGenre)).ToList();

            return ret;
        }

        [HttpPost]
        [Route("api/[controller]/GenreListTEST")]
        public IEnumerable<Models.Movie> GetMoviesTEST([FromBody] string pGenre)
        {
            string[] gList = pGenre.Split('|');

            var ret = GetMovies(gList);

            return ret;
        }
    }
}
