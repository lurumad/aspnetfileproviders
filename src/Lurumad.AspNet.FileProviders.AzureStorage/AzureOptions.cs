namespace Lurumad.AspNet.FileProviders.AzureStorage
{
    public class AzureOptions
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string TableName { get; set; }
    }
}
