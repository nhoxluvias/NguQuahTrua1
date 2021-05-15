using System;

namespace Data.DTO
{
    public class DirectorInfoForAdmin
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class DirectorInfoForUser
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class DirectorCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class DirectorUpdate
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
