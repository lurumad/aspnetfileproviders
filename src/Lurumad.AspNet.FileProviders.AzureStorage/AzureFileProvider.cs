using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace Lurumad.AspNet.FileProviders.AzureStorage
{
    public class AzureFileProvider : IFileProvider
    {
        private readonly CloudBlobContainer container;

        public AzureFileProvider(Action<AzureOptions> options)
        {
            var setup = new AzureOptions();
            options(setup);
            var account = CloudStorageAccount.Parse(setup.ConnectionString);
            var client = account.CreateCloudBlobClient();
            container = client.GetContainerReference(setup.ContainerName);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var directory = container.GetDirectoryReference(NormalizeAzureStoragePath(subpath));
            var directoryInfo = new AzureDirectoryInfo(directory);
            return directoryInfo.Exists ? directoryInfo as IDirectoryContents : new NotFoundDirectoryContents();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var cloudBlockBlob = container.GetBlockBlobReference(NormalizeAzureStoragePath(subpath));
            var fileInfo = new AzureFileInfo(cloudBlockBlob);
            return fileInfo.Exists ? fileInfo as IFileInfo : new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            var cloudBlockBlob = container.GetBlockBlobReference(NormalizeAzureStoragePath(filter));
            return new AzureChangeToken(cloudBlockBlob);
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
