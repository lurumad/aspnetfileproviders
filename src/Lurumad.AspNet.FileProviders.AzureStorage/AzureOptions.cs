using System;

namespace Lurumad.AspNet.FileProviders.AzureStorage
{
    public class AzureOptions
    {
        public string ConnectionString { get; set; }
        public Uri Uri { get; set; }
        public string SasToken { get; set; }
        public string Container { get; set; }
    }
}
