using FilmHouse.Modelo;
using FilmHouse.Plantillas;
using Newtonsoft.Json;

namespace FilmHouse.Vista;

public partial class MainPage : Plantilla
{
    private const string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";
    private List<Peliculas> listaPeliculasPopulares;
    private List<Peliculas> listaPeliculas;
    private List<Peliculas> listaPeliculasGenre;
    private bool esBusqueda = false;

    public MainPage()
	{
		InitializeComponent();
        searchBar.TextChanged += OnSearchBar;
        Genres.SelectedIndexChanged += async (sender, args) =>
        {
            System.Diagnostics.Debug.WriteLine("SelectedIndexChanged event triggered.");

            if (Genres.SelectedIndex >= 0)
            {
                string selectedGenre = Genres.SelectedItem as string;
                int genreId = GenreNameById(selectedGenre);

                // Provide the apiUrl and genreId when calling LoadMoviesByGenre
                await LoadMoviesByGenre("https://api.themoviedb.org/3/discover/movie?sort_by=popularity.desc&", genreId);
            }
        };
        GetDetallesPeliculasPopulares("https://api.themoviedb.org/3/movie/popular");
    }

    private async void OnSearchBar(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue == null || string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            RestaurarPeliculasPopulares();
        }
        else
        {
            await GetDetallesPeliculasBusqueda("https://api.themoviedb.org/3/search/movie?query=", e.NewTextValue);
            titleLabel.IsVisible = false;
        }
    }

    private async Task GetDetallesPeliculasPopulares(string apiUrl)
    {
        listaPeliculasPopulares = new List<Peliculas>();
        string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                //HttpResponseMessage response = await client.GetAsync(apiUrl);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<Peliculas>(responseData);

                    listaPeliculasPopulares = apiResponse.Results.ToList();

                    // Mostrar solo las primeras 5 películas
                    movieCollection.ItemsSource = listaPeliculasPopulares.Take(5).ToList();
                    esBusqueda = false;
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

    private async Task GetDetallesPeliculasBusqueda(string apiUrl, string searchTerm)
    {
        listaPeliculas = new List<Peliculas>();
        string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                HttpResponseMessage response = await client.GetAsync($"{apiUrl}{searchTerm}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<Peliculas>(responseData);

                    listaPeliculas = apiResponse.Results.ToList();

                    movieCollection.ItemsSource = listaPeliculas.Take(10).ToList();
                    esBusqueda = true;
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

    private void MostrarPeliculasPorGenero(string genre)
    {
        if (esBusqueda)
        {
            titleLabel.IsVisible = false;
            titleLabel.Text = $"No se han encontrado películas de {genre}.";
        }
        else
        {
            titleLabel.IsVisible = true;
            titleLabel.Text = $"Películas de {genre}:";
        }
    }

    private async Task LoadMoviesByGenre(string apiUrl, int genreId)
    {
        listaPeliculasGenre = new List<Peliculas>();
        string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                HttpResponseMessage response = await client.GetAsync($"{apiUrl}with_genres={genreId}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<Peliculas>(responseData);

                    listaPeliculasGenre = apiResponse.Results.ToList();

                    movieCollection.ItemsSource = listaPeliculasGenre.Take(10).ToList();

                    if (listaPeliculas.Any())
                    {
                        MostrarPeliculasPorGenero(GenreIdByName(genreId).ToString());
                    }
                    else
                    {
                        titleLabel.Text = "No hay peliculas de ese genero";
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in LoadMoviesByGenre: {ex.Message}");
            }
        }
    }

    private int GenreNameById(string genreName)
    {
        Dictionary<string, int> genreMappings = new Dictionary<string, int>
    {
        {"Action", 28},
        {"Adventure", 12},
        {"Animation", 16},
        {"Comedy", 35},
        {"Crime", 80},
        {"Documentary", 99},
        {"Drama", 18},
        {"Family", 10751},
        {"Fantasy", 14},
        {"History", 36},
        {"Horror", 27},
        {"Music", 10402},
        {"Mystery", 9648},
        {"Romance", 10749},
        {"Science Fiction", 878},
        {"TV Movie", 10770},
        {"Thriller", 53},
        {"War", 10752},
        {"Western", 37}
    };

        return genreMappings.ContainsKey(genreName) ? genreMappings[genreName] : -1;
    }

    private string GenreIdByName(int genreId)
    {
        Dictionary<string, int> genreMappings = new Dictionary<string, int>
            {
                {"Action", 28},
                {"Adventure", 12},
                {"Animation", 16},
                {"Comedy", 35},
                {"Crime", 80},
                {"Documentary", 99},
                {"Drama", 18},
                {"Family", 10751},
                {"Fantasy", 14},
                {"History", 36},
                {"Horror", 27},
                {"Music", 10402},
                {"Mystery", 9648},
                {"Romance", 10749},
                {"Science Fiction", 878},
                {"TV Movie", 10770},
                {"Thriller", 53},
                {"War", 10752},
                {"Western", 37}
            };

        return genreMappings.FirstOrDefault(x => x.Value == genreId).Key;
    }

    private void MostrarPeliculasPopulares()
    {
        movieCollection.ItemsSource = listaPeliculasPopulares.Take(5).ToList();
        titleLabel.IsVisible = true;
    }

    private void RestaurarPeliculasPopulares()
    {
        MostrarPeliculasPopulares();
        esBusqueda = false;
    }

}
