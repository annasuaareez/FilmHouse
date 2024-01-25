
namespace FilmHouse.Vista;

public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
    }

    private async void TapCopyCutPaste(object sender, EventArgs e)
    {
        var resultado = await DisplayActionSheet("Actions", "Cancel", null, "Cut", "Copy", "Paste");
        Label valor = (Label)sender;
        string valorTexto = valor.Text;

        switch (resultado)
        {
            case "Cut":
                switch (valorTexto)
                {
                    case "Username":
                        await Clipboard.SetTextAsync(Username.Text);
                        Username.Text = "";
                        break;
                    case "Password":
                        await Clipboard.SetTextAsync(Pass.Text);
                        Pass.Text = "";
                        break;
                }
                break;
            case "Copy":
                switch (valorTexto)
                {
                    case "Username":
                        await Clipboard.SetTextAsync(Username.Text);
                        break;
                    case "Password":
                        await Clipboard.SetTextAsync(Pass.Text);
                        break;
                }
                break;
            case "Paste":
                switch (valorTexto)
                {
                    case "Username":
                        Username.Text += await Clipboard.GetTextAsync();
                        break;
                    case "Password":
                        Pass.Text += await Clipboard.GetTextAsync();
                        break;
                }
                break;
        }
    }
}

