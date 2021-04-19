using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace Web.Validate
{
    public class CustomValidate
    {
        //public static void InitCustomValidate(CustomValidator customValidator, ServerValidateEventArgs args)
        //{
        //    customValidator.ErrorMessage = "";
        //    customValidator.ServerValidate = new ServerValidateEventHandler();
        //    customValidator.ClientValidationFunction = "";

        //}

        public static void EmailValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            string value = args.Value;
            if (Regex.IsMatch(value, "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@" + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$"))
                args.IsValid = true;
        }

        public static void PasswordValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            string value = args.Value;
            if (Regex.IsMatch(value, @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})"))
                args.IsValid = true;
        }

        public static void RePasswordValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            string value = args.Value;
            if (Regex.IsMatch(value, @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})"))
                args.IsValid = true;
        }

        public static void UsernameValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            string value = args.Value;
            if (Regex.IsMatch(value, "^[a-z0-9_-]{3,15}$"))
                args.IsValid = true;
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