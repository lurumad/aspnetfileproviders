using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lurumad.AspNet.FileProviders.AzureStorage
{
    internal class AzureBlobStorageDirectoryInfo : IDirectoryContents
    {
        private readonly CloudBlobDirectory directory;

        public AzureBlobStorageDirectoryInfo(CloudBlobDirectory directory)
        {
            this.directory = directory;
            var blogResultSegment = directory.ListBlobsSegmentedAsync(null).Result;
            Exists = blogResultSegment.Results != null && blogResultSegment.Results.Any();
        }

        public bool Exists { get; }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return GetContent().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetContent().GetEnumerator();
        }

        private IEnumerable<IFileInfo> GetContent()
        {
            var continuationToken = new BlobContinuationToken();

            do
            {
                var response = directory.ListBlobsSegmentedAsync(continuationToken).Result;
                continuationToken = response.ContinuationToken;
                foreach (var result in response.Results)
                {
                    yield return new AzureBlobStorageFileInfo(result);
                }
            } while (continuationToken != null);
        }
    }
}