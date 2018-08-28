using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace Lurumad.AspNet.FileProviders.AzureStorage
{
    public class AzureBlobStorageFileProvider : IFileProvider
    {
        private readonly CloudBlobContainer container;

        public AzureBlobStorageFileProvider(AzureOptions options)
        {
            CloudBlobClient cloudBlobClient;
            if (!String.IsNullOrWhiteSpace(options.ConnectionString) && CloudStorageAccount.TryParse(options.ConnectionString, out CloudStorageAccount cloudStorageAccount))
            {
                cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            }
            else if (options.Uri != null && !String.IsNullOrWhiteSpace(options.SasToken))
            {
                cloudBlobClient = new CloudBlobClient(options.Uri, new StorageCredentials(options.SasToken));
            }
            else
            {
                throw new ArgumentException($"Please provide {nameof(options.ConnectionString)} or {nameof(options.Uri)} + {nameof(options.SasToken)}");
            }

            container = cloudBlobClient.GetContainerReference(options.Container);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var directory = container.GetDirectoryReference(NormalizeAzureStoragePath(subpath));
            var directoryInfo = new AzureBlobStorageDirectoryInfo(directory);
            return directoryInfo.Exists ? directoryInfo as IDirectoryContents : new NotFoundDirectoryContents();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var cloudBlockBlob = container.GetBlockBlobReference(NormalizeAzureStoragePath(subpath));
            var fileInfo = new AzureBlobStorageFileInfo(cloudBlockBlob);
            return fileInfo.Exists ? fileInfo as IFileInfo : new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            var cloudBlockBlob = container.GetBlockBlobReference(NormalizeAzureStoragePath(filter));
            return new AzureBlobStorageChangeToken(cloudBlockBlob);
        }

        private string NormalizeAzureStoragePath(string subpath)
        {
            if (subpath.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                return subpath.Substring(1);
            }

            return subpath;
        }
    }
}
