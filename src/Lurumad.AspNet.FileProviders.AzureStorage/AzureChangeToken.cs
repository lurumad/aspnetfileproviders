using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace Lurumad.AspNet.FileProviders.AzureStorage
{
    public class AzureChangeToken : IChangeToken
    {
        private readonly IListBlobItem result;

        public AzureChangeToken(IListBlobItem result)
        {
            this.result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public bool HasChanged
        {
            get
            {
                if (result is CloudBlockBlob cloudBlobBlock && cloudBlobBlock.ExistsAsync().Result)
                {
                    cloudBlobBlock.FetchAttributesAsync().Wait();
                    cloudBlobBlock.Metadata.TryGetValue("LastContentMD5", out string lastContentMD5);
                    return cloudBlobBlock.Properties.ContentMD5 != lastContentMD5;
                }

                return false;
            }
        }

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return new EmptyDispose();
        }


        internal class EmptyDispose : IDisposable
        {
            public void Dispose()
            {
                
            }
        }
    }
}
