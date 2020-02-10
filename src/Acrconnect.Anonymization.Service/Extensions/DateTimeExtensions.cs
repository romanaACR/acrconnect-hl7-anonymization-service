using System;

namespace AcrConnect.Anonymization.Service.Extensions
{
    public static class DateTimeUtility
    {
        public static long ToUnixTimestamp(this DateTime date)
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var utc = date.ToUniversalTime();
            var offset = utc - epochStart;
            var totalNumberOfSeconds = Convert.ToInt64(offset.TotalSeconds);
            return totalNumberOfSeconds;
        }

        public static DateTime CreateDate(this Int64 timestamp)
        {
            var offset = TimeSpan.FromSeconds(Convert.ToDouble(timestamp));
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var utc = epochStart + offset;
            var localTime = utc.ToLocalTime();
            return localTime;
        }
    }
}
