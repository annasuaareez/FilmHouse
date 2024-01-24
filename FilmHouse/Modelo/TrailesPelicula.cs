using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmHouse.Modelo
{
    internal class TrailesPelicula
    {
        [JsonProperty("results")]
        public List<Video> Videos { get; set; }

        public class Video
        {
            [JsonProperty("iso_639_1")]
            public string IsoLanguage { get; set; }

            [JsonProperty("iso_3166_1")]
            public string IsoCountry { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("site")]
            public string Site { get; set; }

            [JsonProperty("size")]
            public int Size { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("official")]
            public bool IsOfficial { get; set; }

            [JsonProperty("published_at")]
            public DateTime PublishedAt { get; set; }

            [JsonProperty("id")]
            public string VideoId { get; set; }
        }
    }
}

