using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Migrations;

namespace Web.Test
{
    public partial class Form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Model.Test2.Value;
            RunMigration();
        }

        public void RunMigration()
        {
            RoleMigration roleMigration = new RoleMigration();
            roleMigration.AddDataAndRun();
        }

        protected void TxtInputValidator(object source, ServerValidateEventArgs args)
        {
            string str = args.Value;
            if (str != "chanh")
                args.IsValid = false;
            else
                args.IsValid = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Model.Test2.Value = "giá trị ngẫu nhiên: " + new Random().Next();
        }
    }
}