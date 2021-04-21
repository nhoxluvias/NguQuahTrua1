using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace Web.Validation
{
    public class CustomValidation
    {
        public static void Init(CustomValidator customValidator, string controlToValidate, string errorMessage, bool validateEmptyText, string clientValidationFunction, ServerValidateEventHandler serverValidateEventHandler)
        {
            customValidator.ErrorMessage = errorMessage;
            customValidator.ValidateEmptyText = validateEmptyText;
            customValidator.ControlToValidate = controlToValidate;
            customValidator.ServerValidate += serverValidateEventHandler;
            customValidator.ClientValidationFunction = clientValidationFunction;

        }

        public static void EmailValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            string value = args.Value;
            if (Regex.IsMatch(value, "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@" + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$"))
                args.IsValid = true;
        }

        public static void PasswordValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})");
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
            args.IsValid = Regex.IsMatch(args.Value, "^[a-z0-9_-]{3,15}$");
        }

        public static void PhoneNumberValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{3,15}$");
        }

        public static void CardNumberValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{16,19}$");
        } 

        public static void CVVValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{3}$");
        }

        public static void ExpirationDate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{3}$");
        }
    }
}