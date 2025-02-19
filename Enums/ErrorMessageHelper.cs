using UsersAuth.Enums;
namespace UsersAuth.Helpers
{
    public static class ErrorMessageHelper
    {
        public static string GetErrorMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.UsernameOrPasswordEmpty:
                    return "Username or password cannot be empty.";
                case ErrorCode.InvalidPassword:
                    return "Password must be at least 8 characters long and contain both letters and numbers.";
                case ErrorCode.UserAlreadyExists:
                    return "User already exists.";
                case ErrorCode.RegistrationFailed:
                    return "User registration failed.";
                case ErrorCode.Success:
                    return "Operation was successful.";
                default:
                    return "An unknown error occurred.";
            }
        }
    }
}
