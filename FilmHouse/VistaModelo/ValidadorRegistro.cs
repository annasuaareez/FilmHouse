using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FilmHouse.VistaModelo
{
    public partial class ValidadorRegistro : ObservableValidator

    {
        public ObservableCollection<string> ErroresRegistro { get; set; } = new ObservableCollection<string>();
        public RelayCommand IrAInicioSesionCommand { get; private set; }
        private bool camposVaciosMostrados = false;

        private String username;
        [Required(ErrorMessage = "Campo vacío")]
        public String Username
        {
            get => username;
            set
            {
                SetProperty(ref username, value);
                ValidateProperty(value, nameof(Username));
            }
        }

        private String nombre;
        [RegularExpression(@"^[a-zA-Z]+(?: [a-zA-Z]+)?$", ErrorMessage = "El nombre no debe contener números ni espacios a no se que introduzcas otro nombre")]
        [Required(ErrorMessage = "Campo vacío")]
        public String Nombre
        {
            get => nombre;
            set
            {
                SetProperty(ref (nombre), value);
                ValidateProperty(value, nameof(Nombre));
            }
        }


        private String contrasena;
        //falta coincidir con repetir contraseña
        [RegularExpression(@"^.{5,}$", ErrorMessage = "La contraseña debe tener al menos 5 caracteres")]
        [Required(ErrorMessage = "Campo vacío")]
        public String Contrasena
        {
            get => contrasena;
            set
            {
                SetProperty(ref (contrasena), value);
                ValidateProperty(value, nameof(Contrasena));
            }
        }

        private String repetirContrasena;
        //falta coincidir con repetir contraseña que no funciona
        [Compare(nameof(Contrasena), ErrorMessage = "Las contraseñas no coinciden")]
        //[Required(ErrorMessage = "Campo vacío: Repetir Contraseña")]
        public String RepetirContrasena
        {
            get => repetirContrasena;
            set
            {
                SetProperty(ref (repetirContrasena), value);
                ValidateProperty(value, nameof(RepetirContrasena));
            }
        }

        private String edad;
        [Range(1, 99, ErrorMessage = "La edad debe ser un número válido, mayor que 0 y menor que 100")]
        [Required(ErrorMessage = "Campo vacío")]
        public String Edad
        {
            get => edad;
            set
            {
                SetProperty(ref (edad), value);
                ValidateProperty(value, nameof(Edad));
            }
        }

        private string usernameError;
        public string UsernameError
        {
            get => usernameError;
            set => SetProperty(ref usernameError, value);
        }

        private Color usernameErrorColor = Color.FromHex("#00000000");
        public Color UsernameErrorColor
        {
            get => usernameErrorColor;
            set => SetProperty(ref usernameErrorColor, value);
        }

        private string nombreError;
        public string NombreError
        {
            get => nombreError;
            set => SetProperty(ref nombreError, value);
        }

        private Color nombreErrorColor = Color.FromHex("#00000000");
        public Color NombreErrorColor
        {
            get => nombreErrorColor;
            set => SetProperty(ref nombreErrorColor, value);
        }

        private string contrasenaError;
        public string ContrasenaError
        {
            get => contrasenaError;
            set => SetProperty(ref contrasenaError, value);
        }

        private Color contrasenaErrorColor = Color.FromHex("#00000000");
        public Color ContrasenaErrorColor
        {
            get => contrasenaErrorColor;
            set => SetProperty(ref contrasenaErrorColor, value);
        }

        private string repetirContrasenaError;
        public string RepetirContrasenaError
        {
            get => repetirContrasenaError;
            set => SetProperty(ref repetirContrasenaError, value);
        }

        private Color repetirContrasenaErrorColor = Color.FromHex("#00000000");
        public Color RepetirContrasenaErrorColor
        {
            get => repetirContrasenaErrorColor;
            set => SetProperty(ref repetirContrasenaErrorColor, value);
        }

        private string edadError;
        public string EdadError
        {
            get => edadError;
            set => SetProperty(ref edadError, value);
        }

        private Color edadErrorColor = Color.FromHex("#00000000");
        public Color EdadErrorColor
        {
            get => edadErrorColor;
            set => SetProperty(ref edadErrorColor, value);
        }

        public ValidadorRegistro()
        {
            IrAInicioSesionCommand = new RelayCommand(IrAInicioSesion);
        }

        [RelayCommand]
        //el método sería sincrónico si quisiese pasar entre vistas
        public void validar()
        {
            ValidateAllProperties();
            ErroresRegistro.Clear();

            if (!camposVaciosMostrados)
            {
                var propertiesWithRequiredAttribute = GetType()
                    .GetProperties()
                    .Where(property => Attribute.IsDefined(property, typeof(RequiredAttribute)));

                camposVaciosMostrados = true;
            }

            //GetErrors(nameof(Username)).ToList().ForEach(f => ErroresRegistro.Add(f.ErrorMessage));
            //GetErrors(nameof(Nombre)).ToList().ForEach(f => ErroresRegistro.Add(f.ErrorMessage));
            //GetErrors(nameof(Contrasena)).ToList().ForEach(f => ErroresRegistro.Add(f.ErrorMessage));
            //GetErrors(nameof(Edad)).ToList().ForEach(f => ErroresRegistro.Add(f.ErrorMessage));

            //Miramos loe errores de forma individual
            UsernameError = GetErrors(nameof(Username)).FirstOrDefault()?.ErrorMessage;
            UsernameErrorColor = string.IsNullOrEmpty(UsernameError) ? Color.FromHex("#00000000") : Color.FromHex("#DAA03D");

            NombreError = GetErrors(nameof(Nombre)).FirstOrDefault()?.ErrorMessage;
            NombreErrorColor = string.IsNullOrEmpty(NombreError) ? Color.FromHex("#00000000") : Color.FromHex("#DAA03D");

            ContrasenaError = GetErrors(nameof(Contrasena)).FirstOrDefault()?.ErrorMessage;
            ContrasenaErrorColor = string.IsNullOrEmpty(ContrasenaError) ? Color.FromHex("#00000000") : Color.FromHex("#DAA03D");

            RepetirContrasenaError = string.Equals(Contrasena, RepetirContrasena) ? "" : "Las contraseñas no coinciden";
            RepetirContrasenaErrorColor = string.IsNullOrEmpty(RepetirContrasenaError) ? Color.FromHex("#00000000") : Color.FromHex("#DAA03D");

            EdadError = GetErrors(nameof(Edad)).FirstOrDefault()?.ErrorMessage;
            EdadErrorColor = string.IsNullOrEmpty(EdadError) ? Color.FromHex("#00000000") : Color.FromHex("#DAA03D");

            if (!string.IsNullOrEmpty(Username) && ValidarNombreUsuarioUnico())
            {
                UsernameError = "Nombre de usuario ya existe";
                ErroresRegistro.Add(UsernameError);
                UsernameErrorColor = Color.FromHex("#DAA03D");
            }

            if (ErroresRegistro.Count == 0 && string.IsNullOrEmpty(UsernameError) && string.IsNullOrEmpty(NombreError) &&
                string.IsNullOrEmpty(ContrasenaError) && string.IsNullOrEmpty(RepetirContrasenaError) && string.IsNullOrEmpty(EdadError))
            {
                // All validations passed, proceed to create the user
                if (!string.IsNullOrEmpty(Username) && ValidarNombreUsuarioUnico())
                {
                    ErroresRegistro.Add("Nombre de usuario ya existe");
                }
                else
                {
                    int numeroEdad;
                    if (int.TryParse(Edad, out numeroEdad))
                    {
                        // Create a new user instance
                        var newUser = new Modelo.Usuario
                        {
                            Username = Username,
                            Nombre = Nombre,
                            Contrasena = Modelo.Encriptador.ObtenerHash(Contrasena),
                            Edad = numeroEdad,
                        };

                        // Add the new user to the database
                        App.userRepositorio.add(newUser);

                        // Change the login page
                        var navegacionLogin = Application.Current.MainPage.Navigation;
                        navegacionLogin.PushAsync(new Vista.Login());
                    }
                }
            }
            else
            {
                var message = string.Join("\n", ErroresRegistro);
                Console.WriteLine(message);
            }
        }

        private bool ValidarNombreUsuarioUnico()
        {
            // Llama al repositorio para verificar si el nombre de usuario ya está en uso
            var usuarioExistente = App.userRepositorio.listarAlumnos().FirstOrDefault(u => u.Username == Username);

            // Retorna true si el nombre de usuario ya existe, de lo contrario, retorna false
            return usuarioExistente != null;
        }

        private void IrAInicioSesion()
        {
            var navigation = Application.Current.MainPage.Navigation;
            navigation.PushAsync(new Vista.Login());
        }

    }
}
