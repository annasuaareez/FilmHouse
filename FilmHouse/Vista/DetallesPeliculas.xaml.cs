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
        GetDetallesPelicula(movieId);
        VerificarFavorito();
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

                    string releaseYear = DateTime.Parse(movieDetails.ReleaseDate).Year.ToString();

                    moviePosterImage.Source = movieDetails.PosterUrl;
                    movieTitleLabel.Text = $"{movieDetails.OriginalTitle} ({releaseYear})";
                    movieReleaseDateAndGenresLabel.Text = $"{movieDetails.ReleaseDate} � {genres}";
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

            VerificarFavorito();
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

                        //Console.WriteLine($"URL del tr�iler: {firstTrailerUrl}");

                        trailerWebView.Source = new UrlWebViewSource { Url = trailerUrl };
                        trailerWebView.IsVisible = true;

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No se encontr� ning�n tr�iler.");
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

    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("BOTON FAV");

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
                bool answer = await DisplayAlert("Alerta", "�Seguro que quieres quitar esta pel�cula de favoritos?", "S�", "No");

                if (answer)
                {
                    System.Diagnostics.Debug.WriteLine("PELI ELIMINADA");
                    favoritosRepositorios.RemovePeliculaFav(new UsuarioPeliculaFavorita { UsuarioId = userId, PeliculaId = movieId });

                    // Update the image of the button after removal
                    favoriteButton.Source = "star.png";
                }
                else
                {
                    favoriteButton.Source = "star_fill.png";
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("PELI AGREGADA");
                favoritosRepositorios.AddPeliculaFav(new UsuarioPeliculaFavorita { UsuarioId = userId, PeliculaId = movieId });

                favoriteButton.Source = "star_fill.png";
            }

            //favoriteButton.Source = isFavorited ? "star.png" : "star_fill.png";

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    private void VerificarFavorito()
    {
        try
        {
            int userId = Preferences.Get("UserID", defaultValue: 0);

            if (userId == 0)
            {
                System.Diagnostics.Debug.WriteLine("ID del usuario no encontrado");
                return;
            }

            string databasePath = @"C:\Users\annas\AppData\Local\Packages\a6f808c3-ea24-4ba6-8b77-77b71c77dc47_9zz4h110yvjzm\LocalState\usuarios.db";
            FavoritosRepositorio favoritosRepositorios = new FavoritosRepositorio(databasePath);

            bool isFavorited = favoritosRepositorios.ListarPeliculasFavoritas().Any(item => item.UsuarioId == userId && item.PeliculaId == movieId);

            // Ajustar la imagen del bot�n seg�n el estado de favorito
            favoriteButton.Source = isFavorited ? "star_fill.png" : "star.png";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }

}
