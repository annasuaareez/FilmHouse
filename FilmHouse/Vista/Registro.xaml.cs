namespace FilmHouse.Vista;

public partial class Registro : ContentPage
{
	public Registro()
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
                    case "Name":
                        await Clipboard.SetTextAsync(Nombre.Text);
                        Nombre.Text = "";
                        break;
                    case "Age":
                        await Clipboard.SetTextAsync(Edad.Text);
                        Edad.Text = "";
                        break;
                    case "Username":
                        await Clipboard.SetTextAsync(Usuario.Text);
                        Usuario.Text = "";
                        break;
                    case "Password":
                        await Clipboard.SetTextAsync(Contraseña.Text);
                        Contraseña.Text = "";
                        break;
                    case "Repeat Pasword":
                        await Clipboard.SetTextAsync(Contraseña2.Text);
                        Contraseña2.Text = "";
                        break;
                }
                break;
            case "Copy":
                switch (valorTexto)
                {
                    case "Name":
                        await Clipboard.SetTextAsync(Nombre.Text);
                        break;
                    case "Age":
                        await Clipboard.SetTextAsync(Edad.Text);
                        break;
                    case "Username":
                        await Clipboard.SetTextAsync(Usuario.Text);
                        break;
                    case "Password":
                        await Clipboard.SetTextAsync(Contraseña.Text);
                        break;
                    case "Repeat Pasword":
                        await Clipboard.SetTextAsync(Contraseña2.Text);
                        break;
                }
                break;
            case "Paste":
                switch (valorTexto)
                {
                    case "Name":
                        Nombre.Text += await Clipboard.GetTextAsync();
                        break;
                    case "Age":
                        Edad.Text += await Clipboard.GetTextAsync();
                        break;
                    case "Username":
                        Usuario.Text += await Clipboard.GetTextAsync();
                        break;
                    case "Password":
                        Contraseña.Text += await Clipboard.GetTextAsync();
                        break;
                    case "Repeat Pasword":
                        Contraseña2.Text += await Clipboard.GetTextAsync();
                        break;
                }
                break;
        }
    }
}