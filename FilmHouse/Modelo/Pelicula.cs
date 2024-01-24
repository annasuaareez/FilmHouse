using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmHouse.Modelo
{
    [Table("Peliculas")]
    public class Pelicula
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string PosterPath { get; set; }

        public string BackdropPath { get; set; }

        public string OriginalTitle { get; set; }

        public string ReleaseDate { get; set; }

        public string Overview { get; set; }

        public double VoteAverage { get; set; }

        public string Tagline { get; set; }

        public Pelicula() { }

        public Pelicula(string posterPath, string backdropPath, string originalTitle, string releaseDate, string overview, double voteAverage, string tagline)
        {
            this.PosterPath = posterPath;
            this.BackdropPath = backdropPath;
            this.OriginalTitle = originalTitle;
            this.ReleaseDate = releaseDate;
            this.Overview = overview;
            this.VoteAverage = voteAverage;
            this.Tagline = tagline;
        }
    }
}