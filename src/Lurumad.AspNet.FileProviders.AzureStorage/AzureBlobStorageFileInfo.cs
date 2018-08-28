using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace Lurumad.AspNet.FileProviders.AzureStorage
{
    public class AzureBlobStorageFileInfo : IFileInfo
    {
        private readonly CloudBlockBlob cloudBlobBlock;

        public AzureBlobStorageFileInfo(IListBlobItem result)
        {
            if (result is CloudBlockBlob cloudBlobBlock)
            {
                this.cloudBlobBlock = cloudBlobBlock;
                Exists = cloudBlobBlock.ExistsAsync().Result;
                Length = cloudBlobBlock.Properties.Length;
                PhysicalPath = cloudBlobBlock.Uri.ToString();
                Name = cloudBlobBlock.Name;
                LastModified = cloudBlobBlock.Properties.LastModified ?? DateTimeOffset.MinValue;
                IsDirectory = false;
                if (Exists)
                {
                    cloudBlobBlock.Metadata["LastContentMD5"] = cloudBlobBlock.Properties.ContentMD5;
                    cloudBlobBlock.SetMetadataAsync().Wait();
                }
            }
        }

        public bool Exists { get; }

        public long Length { get; }

        public string PhysicalPath { get; }

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public bool IsDirectory { get; } = true;

        public Stream CreateReadStream()
        {
            var stream = new MemoryStream();
            cloudBlobBlock.DownloadToStreamAsync(stream).Wait();
            stream.Position = 0;
            return stream;
        }
    }
}
