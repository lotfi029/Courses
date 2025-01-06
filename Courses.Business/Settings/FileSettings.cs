namespace Courses.Business.Settings;
public static class FileSettings
{
    public const int MaxFileSizeInMB = 1;
    public const int MaxFileSizeInByte = MaxFileSizeInMB * 1024 * 1024;

    public static readonly string[] BlockedSignatures = ["4D-5A", "2F-2A", "D0-CF"];

    public static readonly string[] AllowedImageExtensions = [".jpg", "jpeg", ".png"];
    public static readonly string[] AllowedVideoExtensions = [".mp4", ".avi", ".mov", ".wmv", ".mkv", ".flv", ".webm", ".m4v"];

    public static readonly string SizeErrorMessage
        = $"this file must be less than or equalt to {MaxFileSizeInMB} MB, or {MaxFileSizeInByte} Byte";
    
    public static readonly string ContentTypeErrorMessage
        = "not allowed content type";

    public static readonly string ImageErrorMessage
        = $"only extension allowed is {string.Join(", ", AllowedImageExtensions)}";

    public static readonly string VideoErrorMessage
        = $"only extension allowed is {string.Join(", ", AllowedVideoExtensions)}";
}
