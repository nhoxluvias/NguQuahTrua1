using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    internal class UserActivity
    {
        public string filmId { get; set; }
        public string userId { get; set; }
        public bool upvoted { get; set; }
        public bool downvoted { get; set; }
        public bool viewed { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
