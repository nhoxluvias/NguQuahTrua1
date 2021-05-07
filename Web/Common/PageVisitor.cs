using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Common
{
    public static class PageVisitor
    {
        public static long Views = 0;
        public static void Add()
        {
            Views += 1;
        }

        public static void Remove()
        {
            if (Views > 0)
                Views -= 1;
        }
    }
}