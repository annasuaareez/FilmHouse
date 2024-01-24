using FilmHouse.Modelo;
namespace FilmHouse.Template;

public partial class Pelis : ContentView
{
    public Pelis()
	{
		InitializeComponent();
    }

    private async void OnImageTapped(object sender, EventArgs e)
	{
		var pelicula = (Peliculas)BindingContext;

        await Navigation.PushAsync(new Vista.DetallesPeliculas(pelicula.Id));
	}

}
