using FilmHouse.Modelo;
using FilmHouse.Plantillas;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using MAUI = Microsoft.Maui.Controls;

namespace FilmHouse.Vista;

public partial class MainPage : Plantilla
{
    public MainPage()
	{
		InitializeComponent();
        GetDetallesPeliculas("https://api.themoviedb.org/3/movie/popular");
    }

    private async void GetDetallesPeliculas(string apiUrl)
    {
        List<Peliculas> listaPeliculas = new List<Peliculas>();

        //string apiUrl = "https://api.themoviedb.org/3/movie/popular";
        string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIzYzM2MDY4YjUxOTQ4MTJmODM2N2RhMWQxZmY3YjNmNyIsInN1YiI6IjY1YTBmOTkyNDQ3ZjljMDEyMjVhNjE1NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._Y48hLg92ILHsg8MsWJR6p01JUIRrRpF_m6bgI66pbo";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<Peliculas>(responseData);

                    listaPeliculas = apiResponse.Results.Take(5).ToList();

                    movieCollection.ItemsSource = listaPeliculas;

                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

}
