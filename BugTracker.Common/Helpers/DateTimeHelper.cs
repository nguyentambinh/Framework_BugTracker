using System;

namespace BugTracker.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime NowVN()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SE Asia Standard Time");
        }
    }
}