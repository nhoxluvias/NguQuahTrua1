using System;
using System.Collections.Generic;

namespace Data.DTO
{
    public class FilmInfo
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public CountryInfo country { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public LanguageInfo language { get; set; }
        public string releaseDate { get; set; }
        public long upvote { get; set; }
        public long downvote { get; set; }
        public long views { get; set; }
        public string url { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
        public List<CategoryInfo> Categories { get; set; }
    }

    public class FilmCreation
    {
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public int countryId { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public int languageId { get; set; }
        public string releaseDate { get; set; }
    }

    public class FilmUpdate
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public int countryId { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public int languageId { get; set; }
        public string releaseDate { get; set; }
    }
}
