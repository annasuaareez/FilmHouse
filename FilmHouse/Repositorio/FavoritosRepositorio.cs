using FilmHouse.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilmHouse.Repositorios
{
    public class FavoritosRepositorio
    {
        private String _ruta;
        private SQLiteConnection conexion;

        public FavoritosRepositorio(String ruta)
        {
            _ruta = ruta;
            conexion = new SQLiteConnection(ruta);
            System.Diagnostics.Debug.WriteLine($"La ruta es {_ruta}");

            if (!conexion.TableMappings.Any(e => e.MappedType.Name == "PeliculaFavorita"))
            {
                conexion.CreateTable<UsuarioPeliculaFavorita>();
            }
        }

        // CRUD
        public void AddPeliculaFav(UsuarioPeliculaFavorita peliculaFavorita)
        {
            System.Diagnostics.Debug.WriteLine($"UsuarioId: {peliculaFavorita.UsuarioId}, PeliculaId: {peliculaFavorita.PeliculaId}");
            conexion.Insert(peliculaFavorita);
        }

        public void RemovePeliculaFav(UsuarioPeliculaFavorita peliculaFavorita)
        {
            System.Diagnostics.Debug.WriteLine($"UsuarioId: {peliculaFavorita.UsuarioId}, PeliculaId: {peliculaFavorita.PeliculaId}");
            conexion.Delete(peliculaFavorita);
            
        }

        public ObservableCollection<UsuarioPeliculaFavorita> ListarPeliculasFavoritas()
        {
            List<UsuarioPeliculaFavorita> lista = conexion.Table<UsuarioPeliculaFavorita>().ToList();
            ObservableCollection<UsuarioPeliculaFavorita> listaPeliculasFavoritas = new ObservableCollection<UsuarioPeliculaFavorita>(lista);
            return listaPeliculasFavoritas;
        }
    }
}
