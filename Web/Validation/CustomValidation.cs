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

        public static void ValidateEmail(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(
                args.Value,
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
            );
        }

        public static void ValidatePassword(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9a-zA-Z$%#@!*+-,.?/]{6,30}$");
        }

        public static void ValidateUsername(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[a-z0-9_-]{3,15}$");
        }

        public static void ValidatePhoneNumber(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{3,15}$");
        }

        public static void ValidateConfirmCode(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{6,12}$");
        }

        public static void ValidateCardNumber(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{16,19}$");
        }

        public static void ValidateCVV(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{3}$");
        }

        public static void ValidateAccountName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[A-Za-z0-9 ]{2,}$");
        }

        public static void ValidateExpirationDate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"(?:0[1-9]|1[0-2])\/[0-9]{2}$");
        }
    }
}