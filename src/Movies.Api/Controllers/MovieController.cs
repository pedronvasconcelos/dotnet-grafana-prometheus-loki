using Microsoft.AspNetCore.Mvc;
using Movies.Api.Models;
using Movies.Api.Observability;
using Movies.Api.Services;

namespace Movies.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;   
        private readonly ILogger<MovieController> _logger;
        private readonly ObservabilityService observabilityService;

        public MovieController(IMovieService movieService,
            ILogger<MovieController> logger,
            ObservabilityService observabilityService)
        {
            _movieService = movieService;
            _logger = logger;
            this.observabilityService = observabilityService;       
        }


    
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            try
            {
                observabilityService.IncomingRequestCounter.Add(1,
                   new KeyValuePair<string, object?>("operation", "GetAllMovies" ),
                   new KeyValuePair<string, object?>("controller", nameof(MovieController))); 
                var movies = _movieService.GetAll();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get movies: {ex}");
                return BadRequest("Failed to get movies");
            }
        }

    
        [HttpGet("{id}")]
        public IActionResult GetMovie(Guid id)
        {
            try
            {
                observabilityService.IncomingRequestCounter.Add(1,
               new KeyValuePair<string, object?>("operation", "GetMovie"),
               new KeyValuePair<string, object?>("controller", nameof(MovieController)));
                var movie = _movieService.GetById(id);

                if (movie == null)
                {
                    _logger.LogInformation($"Movie with id {id} not found.");
                    return NotFound();
                }

                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get movie with id {id}: {ex}");
                return BadRequest($"Failed to get movie with id {id}");
            }
        }

      
        [HttpPost]
        public IActionResult CreateMovie([FromBody] Movie movie)
        {
            try
            {
                observabilityService.IncomingRequestCounter.Add(1,
               new KeyValuePair<string, object?>("operation", "CreateMovie"),
               new KeyValuePair<string, object?>("controller", nameof(MovieController)));
                if (movie == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid movie data");
                }

                var createdMovie = _movieService.Add(movie);
                return CreatedAtAction(nameof(GetMovie), new { id = createdMovie.Id }, createdMovie);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create movie: {ex}");
                return BadRequest("Failed to create movie");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(Guid id)
        {
            try
            {
                observabilityService.IncomingRequestCounter.Add(1,
               new KeyValuePair<string, object?>("operation", "DeleteMovie"),
               new KeyValuePair<string, object?>("controller", nameof(MovieController)));
                var result = _movieService.Remove(id);

                if (!result)
                {
                    _logger.LogInformation($"Movie with id {id} not found.");
                    return NotFound();
                }

                return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete movie with id {id}: {ex}");
                return BadRequest($"Failed to delete movie with id {id}");
            }
        }

    }
}
