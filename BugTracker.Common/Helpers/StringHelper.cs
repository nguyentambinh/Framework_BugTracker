namespace BugTracker.Common.Helpers
{
    public static class StringHelper
    {
        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}