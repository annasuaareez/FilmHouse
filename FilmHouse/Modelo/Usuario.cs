using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;
using Microsoft.VisualBasic;

namespace FilmHouse.Modelo
{
    [Table("Usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Nombre { get; set; }

        public string Contrasena { get; set; }

        public int Edad { get; set; }

        public Usuario() { }

        public Usuario(string username, string nombre, string contrasena, int edad) 
        { 
            this.Username = username;
            this.Nombre = nombre;
            this.Contrasena = contrasena;
            this.Edad = edad;
        }

    }
    
}
