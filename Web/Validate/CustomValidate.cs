using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Web.Validate
{
    public class CustomValidate
    {
        public static void InitCustomValidate(CustomValidator customValidator, ServerValidateEventArgs args)
        {
            customValidator.ErrorMessage = "";
            customValidator.ServerValidate = new ServerValidateEventHandler();
            customValidator.ClientValidationFunction = "";

        }

        public static bool EmailValidate()
        {
            return false;
        }

        public static bool PasswordValidate()
        {
            return false;
        }

        public static bool RePasswordValidate()
        {
            return false;
        }

        public static bool UsernameValidate()
        {
            return false;
        }

        public static bool PhoneNumberValidate()
        {
            return false;
        }

        public static bool CardNumberValidate()
        {
            return false;
        } 

        public static bool CVVValidate()
        {
            return false;
        }

        public static bool ExpirationDate()
        {
            return false;
        }
    }
}