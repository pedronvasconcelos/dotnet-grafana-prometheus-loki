namespace Movies.Api.Models;

public class Movie
{
    public Guid Id { get;   }
    public string Title { get;  }
    public string Genre { get;   }   

    public Movie(string title, string genre)
    {
        Id = Guid.NewGuid();
        Title = title;
        Genre = genre;
    }   
}
