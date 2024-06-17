namespace OfficeSync.Application.Common.Exceptions
{
    public class RequiresTwoFactorException : Exception
    {
        public RequiresTwoFactorException() { }

        public RequiresTwoFactorException(string message) : base(message) { }

        public RequiresTwoFactorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
