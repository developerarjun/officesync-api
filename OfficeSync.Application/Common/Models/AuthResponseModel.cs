using OfficeSync.Domain.Enumerations;

namespace OfficeSync.Application.Common.Models
{
    public class AuthResponseModel
    {
        public string FullName { get; set; }
        public AuthTokenType TokenType { get; set; }
        public string Token { get; set; }
    }
}
