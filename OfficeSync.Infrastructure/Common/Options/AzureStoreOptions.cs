namespace OfficeSync.Infrastructure.Common.Options
{
    public class AzureStoreOptions
    {
        public const string SECTION_NAME = "AzureStore";
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}
