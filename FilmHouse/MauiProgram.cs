using FilmHouse.Repositorio;
using FilmHouse.Repositorios;
using Microsoft.Extensions.Logging;

namespace FilmHouse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif
            String ruta = ObtenerRuta.devolverRuta("usuarios.db");
            builder.Services.AddSingleton<UserRepositorio>(
                s => ActivatorUtilities.CreateInstance<UserRepositorio>(s, ruta)
            );
            builder.Services.AddSingleton<FavoritosRepositorio>(
                s => ActivatorUtilities.CreateInstance<FavoritosRepositorio>(s, ruta)
            );
            builder.Services.AddSingleton<PeliculaRepositorio>(
                s => ActivatorUtilities.CreateInstance<PeliculaRepositorio>(s, ruta)
            );

            return builder.Build();
        }
    }
}