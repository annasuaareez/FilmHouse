using FilmHouse.Modelo;
using FilmHouse.Plantillas;
using FilmHouse.Repositorios;
using Newtonsoft.Json;

namespace FilmHouse.Vista;

public partial class Usuario : Plantilla
{
    private int userId;
    private FavoritosRepositorio favoritosRepositorio;

    public List<Peliculas> IDsFavoritosPelis { get; set; }

    public Usuario()
	{
		InitializeComponent();
        userId = Preferences.Get("UserID", defaultValue: 0);

        if (userId == 0)
        {
            System.Diagnostics.Debug.WriteLine("User ID not found");
            return;
        }

        string databasePath = @"C:\Users\annas\AppData\Local\Packages\a6f808c3-ea24-4ba6-8b77-77b71c77dc47_9zz4h110yvjzm\LocalState\usuarios.db";
        favoritosRepositorio = new FavoritosRepositorio(databasePath);

        LoadFavoriteMovies();

    }

    private async void LoadFavoriteMovies()
    {
        System.Diagnostics.Debug.WriteLine("ENTRA A LA FUNCION");

        try
        {
            var favoriteMovies = favoritosRepositorio.ListarPeliculasFavoritas().Where(item => item.UsuarioId == userId).ToList();

            if (favoriteMovies.Count > 0)
            {

                System.Diagnostics.Debug.WriteLine("mas de una peli fav");

                List<int> movieIds = favoriteMovies.Select(fav => fav.PeliculaId).ToList();

                List<Peliculas> movieDetails = await GetDetallesPeliculas(movieIds);

                IDsFavoritosPelis = movieDetails;

                movieCollection.ItemsSource = IDsFavoritosPelis;

                mensajeLabel.IsVisible = false;

                foreach (var movie in IDsFavoritosPelis)
                {
                    System.Diagnostics.Debug.WriteLine($"Movie ID: {movie.Id}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No peli fav");
                mensajeLabel.Text = "No tienes peliculas favoritas";
                mensajeLabel.IsVisible = true;

                IDsFavoritosPelis = new List<Peliculas>();
                movieCollection.ItemsSource = IDsFavoritosPelis;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task<List<Peliculas>> GetDetallesPeliculas(List<int> peliculaIds)
    {
        List<Peliculas> listaPeliculas = new List<Peliculas>();
        string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                foreach (int peliculaId in peliculaIds)
                {
                    string apiUrl = $"https://api.themoviedb.org/3/movie/{peliculaId}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var pelicula = JsonConvert.DeserializeObject<Peliculas>(responseData);
                        listaPeliculas.Add(pelicula);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }

                return listaPeliculas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null; // Manejo de errores: Puedes retornar null o una lista vacía según sea necesario
            }
        }
    }

}
