using System;
using System.Linq;

namespace InstaAutoBot.Instagram.Storage.FileManagement
{
    [AttributeUsage(AttributeTargets.All)]
    public class InstaDataFileTypeSettings : Attribute
    {
        public InstaDataFileTypeSettings(
            string blobFolderName,
            string allowedFileExtensions,
            double allowedFileSizeInMb)
        {
            BlobFolderName = blobFolderName;
            AllowedFileExtensions = allowedFileExtensions;
            AllowedFileSizeInMb = allowedFileSizeInMb;
        }


        public string AllowedFileExtensions { get; set; }
        public static string GetAllowedFileExtensions(InstaDataFileType type)
        {
            return GetAttribute(type).AllowedFileExtensions;
        }


        public double AllowedFileSizeInMb { get; set; }
        public static double GetAllowedFileSizeInMb(InstaDataFileType type)
        {
            return GetAttribute(type).AllowedFileSizeInMb;
        }



        public string BlobFolderName { get; set; }
        public static string GetBlobFolderName(InstaDataFileType type)
        {
            return GetAttribute(type).BlobFolderName;
        }


        private static InstaDataFileTypeSettings GetAttribute(InstaDataFileType type)
        {

            var memberInfo = typeof(InstaDataFileType).GetMember(type.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var attribute = (InstaDataFileTypeSettings)
                    memberInfo.GetCustomAttributes(typeof(InstaDataFileTypeSettings), false)
                        .FirstOrDefault();
                return attribute;
            }

            return null;
        }
    }
}
