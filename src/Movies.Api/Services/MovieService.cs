using Movies.Api.Models;

namespace Movies.Api.Services;

public class MovieService : IMovieService   
{

    private readonly List<Movie> _movies = new List<Movie>();

    public MovieService()
    {
       

        _movies.Add(new Movie ("The Shawshank Redemption", "Drama"));
        _movies.Add(new Movie("The Godfather","Crime" ));
        _movies.Add(new Movie("The Dark Knight","Action"));
        _movies.Add(new Movie("The Lord of the Rings: The Return of the King","Adventure"));
        _movies.Add(new Movie("Pulp Fiction","Crime"));
        _movies.Add(new Movie("Schindler's List","Biography"));
        _movies.Add(new Movie("Inception","Action"));
        _movies.Add(new Movie("Fight Club","Drama"));
        _movies.Add(new Movie("Forrest Gump","Drama"));
        _movies.Add(new Movie("The Matrix","Action"));
        _movies.Add(new Movie("The Lord of the Rings: The Fellowship of the Ring","Adventure"));
        _movies.Add(new Movie("The Lord of the Rings: The Two Towers","Adventure"));
        _movies.Add(new Movie("Goodfellas","Biography"));
        _movies.Add(new Movie("The Usual Suspects","Crime"));
        _movies.Add(new Movie("Se7en","Crime"));
        _movies.Add(new Movie("The Silence of the Lambs","Crime"));
        _movies.Add(new Movie("The Green Mile","Crime"));
        _movies.Add(new Movie("The Prestige","Drama"));
        _movies.Add(new Movie("Leon: The Professional","Crime"));
        _movies.Add(new Movie("American History X","Drama"));
        _movies.Add(new Movie("Dune","Adventure"));
        _movies.Add(new Movie("Dune 2","Adventure"));
        _movies.Add(new Movie("Star Wars: Episode IV - A New Hope","Action"));
        _movies.Add(new Movie("Star Wars: Episode V - The Empire Strikes Back","Action"));
        _movies.Add(new Movie("Star Wars: Episode VI - Return of the Jedi","Action"));
        _movies.Add(new Movie("Star Wars: Episode I - The Phantom Menace","Action"));
        _movies.Add(new Movie("Star Wars: Episode II - Attack of the Clones","Action"));
        _movies.Add(new Movie("Star Wars: Episode III - Revenge of the Sith","Action"));
        _movies.Add(new Movie("Star Wars: Episode VII - The Force Awakens","Action"));
        _movies.Add(new Movie("Star Wars: Episode VIII - The Last Jedi","Action"));
        _movies.Add(new Movie("Star Wars: Episode IX - The Rise of Skywalker","Action"));
        _movies.Add(new Movie("The Hangover","Comedy"));
        _movies.Add(new Movie("Superbad","Comedy"));
        _movies.Add(new Movie("Sound of Freedom","Drama"));    
        _movies.Add(new Movie("Cidade de Deus","Crime"));
        _movies.Add(new Movie("Tropa de Elite","Action"));
        _movies.Add(new Movie("Tropa de Elite 2","Action"));
        _movies.Add(new Movie("O Auto da Compadecida","Comedy"));
        _movies.Add(new Movie("La Bamba","Biography"));
        _movies.Add(new Movie("Senna","Biography"));





    }

    public Movie Add(Movie movie)
    {
        _movies.Add(movie);
        return movie;
    }

    public IEnumerable<Movie> GetAll()
    {
        return _movies;
    }

    public Movie? GetById(Guid id)
    {
        return _movies.FirstOrDefault(x => x.Id == id);
    }

    public bool Remove(Guid id)
    {
        var movie = GetById(id);
        if (movie != null)
        {
            _movies.Remove(movie);
            return true;
        }
        return false;       
    }   
    

}
