namespace InstaAutoBot.Instagram.Storage.FileManagement
{
    public enum InstaDataFileType
    {
        [InstaDataFileTypeSettings(
               blobFolderName: "PostTemplates\\",
               allowedFileExtensions: ".zip,.Zip",
               allowedFileSizeInMb: 1000)]
        PostTemplates = 0,
    }
}
