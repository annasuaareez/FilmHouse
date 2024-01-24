
namespace FilmHouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Vista.MainPage), typeof(Vista.MainPage));
            Routing.RegisterRoute(nameof(Vista.DetallesPeliculas), typeof(Vista.DetallesPeliculas));
            Routing.RegisterRoute(nameof(Vista.PeliculasPage), typeof(Vista.PeliculasPage));
            Routing.RegisterRoute(nameof(Vista.Usuario), typeof(Vista.Usuario));
            Routing.RegisterRoute(nameof(Vista.Login), typeof(Vista.Login));
            Routing.RegisterRoute(nameof(Vista.Registro), typeof(Vista.Registro));
        }
    }
}