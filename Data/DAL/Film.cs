using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    internal class Film
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public int countryId { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public int langugeId { get; set; }
        public DateTime releaseDate { get; set; }
        public long upvote { get; set; }
        public long downvote { get; set; }
        public long view { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
