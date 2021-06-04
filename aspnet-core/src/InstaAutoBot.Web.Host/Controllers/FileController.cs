using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Abp.Web.Models;
using InstaAutoBot.Controllers;
using InstaAutoBot.Instagram.Storage.FileManagement;
using Microsoft.AspNetCore.Mvc;

namespace InstaAutoBot.Web.Host.Controllers
{
    public class FileController : InstaAutoBotControllerBase
    { 
        private readonly IFileStorageManager _fileStorageManager;

        public FileController( 
            IFileStorageManager fileStorageManager
        )
        { 
            _fileStorageManager = fileStorageManager;
        }

        [HttpPost]
        public JsonResult UploadFiles()
        {
            try
            {
                var files = Request.Form.Files;
                var instaDataFileTypeString = Request.Form["instaDataFileType"];
                var instaDataFileType = (InstaDataFileType)Enum.Parse(typeof(InstaDataFileType), instaDataFileTypeString);

                //Check input
                if (files == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                List<UploadFileOutput> filesOutput = new List<UploadFileOutput>();

                foreach (var file in files)
                {
                    #region  FileSize Validation

                    var allowedFileSize = InstaDataFileTypeSettings.GetAllowedFileSizeInMb(instaDataFileType);

                    if (ConvertBytesToMegabytes(file.Length) > allowedFileSize) //1MB
                    { throw new UserFriendlyException($"Invalid File Size. Allowed File Size {allowedFileSize}MB"); }

                    #endregion

                    #region  Extension Validation
                    var uploadedFileExtension = Path.GetExtension(file.FileName).ToLower();
                    var allowedFileExtensions = InstaDataFileTypeSettings.GetAllowedFileExtensions(instaDataFileType);
                    var isValidExtension = allowedFileExtensions
                                                .Split(',')
                                                .Select(x => x.ToLower())
                                                .Contains(uploadedFileExtension);

                    if (isValidExtension == false)
                    {
                        throw new UserFriendlyException($"Invalid Extension. Allowed Extension {allowedFileExtensions}");
                    }
                    #endregion

                    var newFileName = _fileStorageManager
                            .UploadFile(
                                file.OpenReadStream(),
                                file.FileName,
                                instaDataFileType);

                    filesOutput.Add(new UploadFileOutput
                    {
                        Id = Guid.Empty,
                        FileName = newFileName
                    });
                }

                return Json(new AjaxResponse(filesOutput));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
