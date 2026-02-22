using System.Configuration;

namespace BugTracker.Common.Settings
{
    public static class AppSettings
    {
        public static string UploadFolder
            => ConfigurationManager.AppSettings["UploadFolder"];
    }
}