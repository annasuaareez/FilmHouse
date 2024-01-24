using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilmHouse.Modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmHouse.VistaModelo
{
    public partial class ValidadorLogin : ObservableValidator
    {
        public ObservableCollection<string> ErroresLogin { get; set; } = new ObservableCollection<string>();
        public RelayCommand IrAWelcomePageCommand { get; private set; }
        public RelayCommand IrARegistroCommand { get; private set; }

        private String username;
        //no se que tipo de error poner, es que si user y contraseña se encuentran en bd se valida
        [Required(ErrorMessage = "Campo vacío: Username")]
        public String Username
        {
            get => username;
            set => SetProperty(ref (username), value);
        }

        private String contrasena;
        [Required(ErrorMessage = "Campo vacío: Contraseña")]
        public String Contrasena
        {
            get => contrasena;
            set => SetProperty(ref (contrasena), value);
        }

        public ValidadorLogin()
        {
            IrAWelcomePageCommand = new RelayCommand(IrAWelcomePage);
            IrARegistroCommand = new RelayCommand(IrARegistro);
        }

        [RelayCommand]
        //el método sería sincrónico si quisiese pasar entre vistas
        public void validar()
        {
            ValidateAllProperties();
            ErroresLogin.Clear();
            GetErrors(nameof(Username)).ToList().ForEach(f => ErroresLogin.Add(f.ErrorMessage));
            GetErrors(nameof(Contrasena)).ToList().ForEach(f => ErroresLogin.Add(f.ErrorMessage));

            if (ErroresLogin.Count == 0)
            {
                if (AutenticarUsuario(Username, Contrasena))
                {
                    IrAWelcomePage();
                }
                else
                {
                    ErroresLogin.Add("Credenciales inválidas");
                    MostrarMensajeEmergente("Error", "Credenciales inválidas");
                }
            }
            else
            {
                MostrarMensajeEmergente("Error", "Complete todos los campos");
            }

        }

        private bool AutenticarUsuario(string username, string password)
        {
            var usuarioExistente = App.userRepositorio.listarAlumnos().FirstOrDefault(u => u.Username == username && u.Contrasena == Encriptador.ObtenerHash(password));

            if (usuarioExistente != null)
            {
                //Application.Current.Properties["UserID"] = usuarioExistente.Id;
                //Application.Current.Properties["Username"] = usuarioExistente.Username;

                Preferences.Set("UserID", usuarioExistente.Id);
                Preferences.Set("Username", usuarioExistente.Username);

                App.Username = usuarioExistente.Username;

                return true;
            }

            return false;
        }

        private async void MostrarMensajeEmergente(string titulo, string mensaje)
        {
            await Application.Current.MainPage.DisplayAlert(titulo, mensaje, "OK");
        }

        private void IrAWelcomePage()
        {
            var navigation = App.Current.MainPage.Navigation;
            navigation.PushAsync(new Vista.MainPage());
        }

        private void IrARegistro()
        {
            var navigation = App.Current.MainPage.Navigation;
            navigation.PushAsync(new Vista.Registro());
        }
    }
}
