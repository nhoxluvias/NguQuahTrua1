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

        public static bool EmailValidate(object source, ServerValidateEventArgs args)
        {
            return false;
        }

        public static bool PasswordValidate(object source, ServerValidateEventArgs args)
        {
            return false;
        }

        public static bool RePasswordValidate(object source, ServerValidateEventArgs args)
        {
            return false;
        }

        public static bool UsernameValidate(object source, ServerValidateEventArgs args)
        {
            return false;
        }

        public static bool PhoneNumberValidate(object source, ServerValidateEventArgs args)
        {
            return false;
        }

        public static bool CardNumberValidate(object source, ServerValidateEventArgs args)
        {
            return false;
        } 

        public static bool CVVValidate(object source, ServerValidateEventArgs args)
        {
            return false;
        }

        public static bool ExpirationDate(object source, ServerValidateEventArgs args)
        {
            return false;
        }
    }
}