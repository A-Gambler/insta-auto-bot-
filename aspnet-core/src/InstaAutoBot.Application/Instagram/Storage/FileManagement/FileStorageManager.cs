using System;
using System.Collections.Generic;
using System.IO;
using Abp.Domain.Services;
using Abp.Extensions;

namespace InstaAutoBot.Instagram.Storage.FileManagement
{
    public interface IFileStorageManager : IDomainService
    {
        string UploadFile(Stream stream, string fileName, InstaDataFileType instaDataFileType, bool autoGenerateName = true);

        string UploadFileText(string text, string fileName, InstaDataFileType instaDataFileType, bool autoGenerateName = true);

  
        void DeleteFiles(List<string> blobNameList);

        string DownloadFileText(string fileName, InstaDataFileType instaDataFileType);
        string GenerateBlobUrl(string fileName, InstaDataFileType instaDataFileType);
    }

    public class FileStorageManager : InstaAutoBotDomainServiceBase, IFileStorageManager
    {
        // TODO: move to proper file extension consts file
        private const string CsvFileExtension = ".csv";
        private readonly IVirtualFileStorageManager _virtualFileStorageManager;

        public FileStorageManager(IVirtualFileStorageManager virtualFileStorageManager)
        {
            _virtualFileStorageManager = virtualFileStorageManager;
        }

        public string UploadFileText(string text, string fileName, InstaDataFileType instaDataFileType, bool autoGenerateName = true)
        {
            var extension = Path.GetExtension(fileName);
            var newFileName = fileName;

            if (autoGenerateName)
                newFileName = $"{Guid.NewGuid():N}{extension}";

            var mimeType = extension.GetMimeType();
            var blobFolderName = InstaDataFileTypeSettings.GetBlobFolderName(instaDataFileType);
            var blobName = $"{blobFolderName.EnsureEndsWith('\\')}{newFileName}";

            _virtualFileStorageManager.UploadFromString(text, blobName, mimeType);

            return newFileName;
        }

        public string UploadFile(Stream stream, string fileName, InstaDataFileType instaDataFileType, bool autoGenerateName = true)
        {
            var extension = Path.GetExtension(fileName);
            var newFileName = fileName;

            if (autoGenerateName)
                newFileName = $"{Guid.NewGuid():N}{extension}";

            var mimeType = extension.GetMimeType();
            var blobFolderName = InstaDataFileTypeSettings.GetBlobFolderName(instaDataFileType);
            var blobName = $"{blobFolderName.EnsureEndsWith('/')}{newFileName}";

            _virtualFileStorageManager.UploadFromStream(stream, blobName, mimeType);

            return newFileName;
        }

         
        public void DeleteFiles(List<string> blobNameList)
        {
            _virtualFileStorageManager.DeleteBlob(blobNameList);
        }


        public string DownloadFileText(string fileName, InstaDataFileType instaDataFileType)
        {
            var blobFolderName = InstaDataFileTypeSettings.GetBlobFolderName(instaDataFileType);
            var blobName = $"{blobFolderName.EnsureEndsWith('/')}{fileName}";

            return _virtualFileStorageManager.DownloadBlobString(blobName);
        }


        public string GenerateBlobUrl(string fileName, InstaDataFileType instaDataFileType)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            var blobFolderName = InstaDataFileTypeSettings.GetBlobFolderName(instaDataFileType);
            var blobName = $"{blobFolderName.EnsureEndsWith('/')}{fileName}";

            return _virtualFileStorageManager.GenerateBlobUrl(blobName);
        }

    }
}
