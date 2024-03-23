using Movies.Api.Models;

namespace Movies.Api.Services;

public interface IMovieService
{
    Movie Add(Movie movie);
    IEnumerable<Movie> GetAll();
    Movie? GetById(Guid id);
    bool Remove(Guid id);
}
