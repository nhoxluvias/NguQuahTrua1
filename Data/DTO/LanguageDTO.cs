using System;

namespace Data.DTO
{
    public class LanguageInfoForAdmin
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class LanguageInfoForUser
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class LanguageCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class LanguageUpdate
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
