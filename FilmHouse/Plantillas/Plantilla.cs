
namespace FilmHouse.Plantillas;

public class Plantilla : ContentPage
{
    public Plantilla()
    {
        var plantillas = Application.Current.Resources["plantilla"] as ControlTemplate;

        ControlTemplate = plantillas;
    }
}
