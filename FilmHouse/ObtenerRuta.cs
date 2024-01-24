using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmHouse
{
    internal class ObtenerRuta
    {
        public static string devolverRuta(String nombreBD)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, nombreBD);
        }
    }
}
