using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Test
{
    public partial class Form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void TxtInputValidator(object source, ServerValidateEventArgs args)
        {
            string str = args.Value;
            if (str != "chanh")
                args.IsValid = false;
            else
                args.IsValid = true;
        }
    }
}