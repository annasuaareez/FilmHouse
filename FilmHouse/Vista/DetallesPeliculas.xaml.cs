using FilmHouse.Modelo;
using FilmHouse.Plantillas;
using FilmHouse.Repositorios;
using Newtonsoft.Json;

namespace FilmHouse.Vista;

public partial class DetallesPeliculas : Plantilla
{
    private int movieId;

    public DetallesPeliculas(int movieId)
    {
        InitializeComponent();
        this.movieId = movieId;
        CheckAndUpdateFavoriteStatus();
        GetDetallesPelicula(movieId);
    }

    private async void GetDetallesPelicula(int movieId)
    {
        string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                string apiUrl = $"https://api.themoviedb.org/3/movie/{movieId}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var movieDetails = JsonConvert.DeserializeObject<Peliculas>(responseData);

                    string genres = movieDetails.Genres != null ? string.Join(",", movieDetails.Genres.Select
                        (gen => gen.Name)) : string.Empty;

                    moviePosterImage.Source = movieDetails.PosterUrl;
                    movieTitleLabel.Text = $"{movieDetails.OriginalTitle} ({movieDetails.VoteAverage})";
                    movieReleaseDateAndGenresLabel.Text = $"{movieDetails.ReleaseDate} • {genres}";
                    movieTaglineLabel.Text = movieDetails.Tagline;
                    movieOverviewLabel.Text = movieDetails.Overview;

                    await GetTrailerPelicula(movieId);

                    movieDetailsStackLayout.IsVisible = true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    private async Task GetTrailerPelicula(int movieID)
    {
        string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                string apiUrl = $"https://api.themoviedb.org/3/movie/{movieID}/videos";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var trailerPelicula = JsonConvert.DeserializeObject<TrailesPelicula>(responseData);

                    var trailerVideos = trailerPelicula.Videos.Where(video => video.Type == "Trailer").ToList();

                    if (trailerVideos.Count > 0)
                    {
                        string trailerKey = trailerVideos.First().Key;
                        // Usar YouTube Embedded Player API formato
                        string trailerUrl = $"https://www.youtube.com/embed/{trailerKey}?rel=0&modestbranding=1&autohide=1&showinfo=0";

                        //Console.WriteLine($"URL del tráiler: {firstTrailerUrl}");

                        trailerWebView.Source = new UrlWebViewSource { Url = trailerUrl };
                        trailerWebView.IsVisible = true;

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No se encontró ningún tráiler.");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    private void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("BOTON FAV");

        // Use the userId variable instead of user.Id
        try
        {

            int userId = Preferences.Get("UserID", defaultValue: 0);

            if (userId == 0)
            {
                System.Diagnostics.Debug.WriteLine("ID del usuario no encontrado");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"User ID: {userId}");

            string databasePath = @"C:\Users\annas\AppData\Local\Packages\a6f808c3-ea24-4ba6-8b77-77b71c77dc47_9zz4h110yvjzm\LocalState\usuarios.db";
            //string databaseFileName = "usuarios.db";
            //string databasePath = Path.Combine(ApplicationData.Current.LocalFolder, databaseFileName);

            FavoritosRepositorio favoritosRepositorios = new FavoritosRepositorio(databasePath);

            bool isFavorited = favoritosRepositorios.ListarPeliculasFavoritas().Any(item => item.UsuarioId == userId && item.PeliculaId == movieId);

            if (isFavorited)
            {
                System.Diagnostics.Debug.WriteLine("PELI ELIMINADA");
                favoritosRepositorios.RemovePeliculaFav(new UsuarioPeliculaFavorita { UsuarioId = userId, PeliculaId = movieId });
                favoriteButton.Source = "star.png";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("PELI AGREGADA");
                favoritosRepositorios.AddPeliculaFav(new UsuarioPeliculaFavorita { UsuarioId = userId, PeliculaId = movieId });
                favoriteButton.Source = "star_fill.png";

                SecureStorage.SetAsync($"FavoriteStatus_{userId}_{movieId}", "true");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    private async void CheckAndUpdateFavoriteStatus()
    {
        int userId = Preferences.Get("UserID", defaultValue: 0);

        if (userId == 0)
        {
            System.Diagnostics.Debug.WriteLine("User ID not found");
            return;
        }

        bool isFavorited = await SecureStorage.GetAsync($"FavoriteStatus_{userId}_{movieId}") == "true";

        if (isFavorited)
        {
            favoriteButton.Source = "star_fill.png";
        }
        else
        {
            favoriteButton.Source = "star.png";
        }
    }
}
