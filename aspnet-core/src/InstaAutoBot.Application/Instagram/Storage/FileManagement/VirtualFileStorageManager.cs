using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Abp.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace InstaAutoBot.Instagram.Storage.FileManagement
{
    public interface IVirtualFileStorageManager : IDomainService
    {
        void UploadFromString(string text, string blobName, string contentType);

        void UploadFromStream(Stream stream, string blobName, string contentType);

        string DownloadBlobString(string blobName);

        bool IsBlobExist(string blobName);


        void DeleteBlob(List<string> blobNames);

        Stream DownloadBlobStream(string blobName);

        string GenerateBlobUrl(string blobName);

    }

    public class VirtualFileStorageManager : InstaAutoBotDomainServiceBase, IVirtualFileStorageManager
    {
        private readonly IConfiguration _appConfiguration;
        public string FileLocationDirectory => _appConfiguration["App:FileLocationDirectory"];

        public VirtualFileStorageManager(IConfiguration configurationAccessor)
        {
            _appConfiguration = configurationAccessor;
        }

        public void UploadFromString(string text, string blobName, string contentType)
        {
            UploadFromStream(new MemoryStream(Encoding.UTF8.GetBytes(text)), blobName, contentType);
        }

        public void UploadFromStream(Stream stream, string blobName, string contentType)
        {
            if (stream == null)
                return;


            var path = Path.Combine(FileLocationDirectory, blobName);
            var directory = Path.GetDirectoryName(path);

            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(outputFileStream);
            }
        }

        public bool IsBlobExist(string blobName)
        {
            return false;
            //var container = new BlobContainerClient(ConnectionString, ContainerName);
            //var blockBlob = container.GetBlobClient(blobName);
            //return blockBlob.Exists();
        }

        public void DeleteBlob(string blobName)
        {
            //var container = new BlobContainerClient(ConnectionString, ContainerName);
            //var blockBlob = container.GetBlobClient(blobName);
            //blockBlob.Delete();
        }

        public void DeleteBlob(List<string> blobNames)
        {
            foreach (var blobName in blobNames)
            {
                DeleteBlob(blobName);
            }
        }


        public Stream DownloadBlobStream(string blobName)
        {
            try
            {
                return null;
                //var container = new BlobContainerClient(ConnectionString, ContainerName);
                //var blockBlob = container.GetBlobClient(blobName);
                //var response = blockBlob.Download();
                //return response.Value.Content;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string DownloadBlobString(string blobName)
        {
            try
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }



        public string GenerateBlobUrl(string blobName)
        {
            return string.Empty;
            //return Flurl.Url.Combine(BaseUrl, ContainerName, blobName);
        }

    }
}
