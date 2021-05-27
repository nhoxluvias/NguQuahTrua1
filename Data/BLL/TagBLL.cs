using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class TagBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;

        public TagBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
        }

        public TagBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
        }

    }
}
