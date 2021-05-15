using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.DTO
{
    public class FilmInfo
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public string countryName { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public string languge { get; set; }
        public DateTime releaseDate { get; set; }
        public long upvote { get; set; }
        public long downvote { get; set; }
        public long view { get; set; }
        public string url { get; set; }

        public List<Category> Categories { get; set; }
    }
}