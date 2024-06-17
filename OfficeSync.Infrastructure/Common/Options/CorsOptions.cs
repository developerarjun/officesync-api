namespace OfficeSync.Infrastructure.Common.Options
{
    public class CorsOptions
    {
        public const string SECTION_NAME = "CORS";
        public string[] AllowedOrigins { get; set; }
        public string[] ExposedHeaders { get; set; }
    }
}
