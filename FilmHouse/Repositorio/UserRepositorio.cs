using FilmHouse.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilmHouse.Repositorios
{
    public class UserRepositorio
    {
        private String _ruta;

        //las sentencias sql se las hago a ""
        private SQLiteConnection conexion;
        //singleton
        public UserRepositorio(String ruta)
        {
            _ruta = ruta;
            conexion = new SQLiteConnection(ruta);
            System.Diagnostics.Debug.WriteLine($"La ruta es {_ruta}");

            if (!conexion.TableMappings.Any(e => e.MappedType.Name == "Alumno"))
            {
                conexion.CreateTable<Usuario>();
            }
        }

        //CRUD
        //no hace falta refernciar la tabla otra vez
        public void add(Usuario usuario)
        {
            conexion.Insert(usuario);
        }

        public ObservableCollection<Usuario> listarAlumnos()
        {
            List<Usuario> lista = conexion.Table<Usuario>().ToList();
            ObservableCollection<Usuario> listaUsuarios = new ObservableCollection<Usuario>(lista);
            return listaUsuarios;
        }

    }
}
