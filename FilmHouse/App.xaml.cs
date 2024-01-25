using FilmHouse.Modelo;
using FilmHouse.Repositorio;
using FilmHouse.Repositorios;
using System.Collections.ObjectModel;

namespace FilmHouse
{
    public partial class App : Application
    {
        public static UserRepositorio userRepositorio { get; set; }
        public static FavoritosRepositorio favoritosRepositorio { get; set; }
        public static PeliculaRepositorio peliculaRepositorio { get; set; }

        public ObservableCollection<Peliculas> Pelicula { get; set; }
        private string _searchQuery;
        private Picker filterPicker;

        public static string Username { get; set; }

        public App(UserRepositorio _userRepositorio, FavoritosRepositorio _favoritosRepositorios, PeliculaRepositorio _peliculaRepositorio)
        {
            Pelicula = new ObservableCollection<Peliculas>();

            userRepositorio = _userRepositorio;
            favoritosRepositorio = _favoritosRepositorios;
            peliculaRepositorio = _peliculaRepositorio;

            InitializeComponent();

            MainPage = new AppShell();
            MainPage.BindingContext = this;
        }

        private async void OnUsuarioClicked(object sender, EventArgs e)
        {
            await DisplayOptionsPopup();
        }

        private async Task DisplayOptionsPopup()
        {
            var result = await MainPage.DisplayActionSheet(null, "Cancelar", null, "Favoritos", "Cerrar Sesión");

            if (result == "Favoritos")
            {
                OnFavoritesClicked();
            }
            else if (result == "Cerrar Sesión")
            {
                OnLogoutClicked();
            }
        }

        private void OnFavoritesClicked()
        {
            Page usuarioPage = new Vista.Usuario();
            AppShell.Current.Navigation.PushAsync(usuarioPage);
        }

        private void OnLogoutClicked()
        {
            Page loginPage = new Vista.Login();
            AppShell.Current.Navigation.PushAsync(loginPage);
        }

        private async void OnLogoClicked(object sender, EventArgs e)
        {
            //AppShell.Current.Navigation.PushAsync(Vista.MainPage));
            Page mainPage = new Vista.MainPage();
            await AppShell.Current.Navigation.PushAsync(mainPage);

        }

        private async void OnFilmClicked(object sender, EventArgs e)
        {
            var selectText = (Label)sender;

            if (selectText.Text == "Películas")
            {
                Page peliculas = new Vista.PeliculasPage();
                await AppShell.Current.Navigation.PushAsync(peliculas);
            }

        }

        private void OnLinkedinClicked(object sender, EventArgs e)
        {
            try
            {
                var uri = new Uri("https://www.linkedin.com/in/anasuarezvillaescusa/");
                Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al abrir el enlace: {ex.Message}");
            }
        }

        private void OnGitHubClicked(object sender, EventArgs e)
        {
            try
            {
                var uri = new Uri("https://github.com/annasuaareez");
                Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al abrir el enlace: {ex.Message}");
            }
        }

    }
}
