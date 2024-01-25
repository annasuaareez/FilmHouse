using FilmHouse.Modelo;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmHouse.Repositorio
{
    public class PeliculaRepositorio
    {
        private String _ruta;
        private SQLiteConnection conexion;

        public PeliculaRepositorio(String ruta)
        {
            _ruta = ruta;
            conexion = new SQLiteConnection(ruta);
            System.Diagnostics.Debug.WriteLine($"La ruta es {_ruta}");

            if (!conexion.TableMappings.Any(e => e.MappedType.Name == "PeliculaFavorita"))
            {
                conexion.CreateTable<Pelicula>();
            }
        }

        public void AddPelicula(Pelicula pelicula)
        {
            conexion.Insert(pelicula);
        }

        public void RemovePelicula(Pelicula pelicula)
        {
            conexion.Insert(pelicula);
        }

        public ObservableCollection<Pelicula> ListarPeliculas()
        {
            List<Pelicula> lista = conexion.Table<Pelicula>().ToList();
            ObservableCollection<Pelicula> listaPeliculas = new ObservableCollection<Pelicula>(lista);
            return listaPeliculas;
        }

        public async Task<List<Peliculas>> GetPeliculasByQuery(string query, string apiKey)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                try
                {
                    string apiUrl = $"https://api.themoviedb.org/3/search/movie?query={query}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var movies = JsonConvert.DeserializeObject<List<Peliculas>>(responseData);
                        return movies;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
