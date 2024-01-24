using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmHouse.Modelo
{
    [Table("UsuarioPeliculaFavorita")]
    public class UsuarioPeliculaFavorita
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int PeliculaId { get; set; }

        public UsuarioPeliculaFavorita() { }

        public UsuarioPeliculaFavorita(int usuarioID, int peliculaID)
        {
            UsuarioId = usuarioID;
            PeliculaId = peliculaID;
        }
    }
}
