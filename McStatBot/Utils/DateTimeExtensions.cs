using System;

namespace McStatBot.Utils
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTime(this long? millisecondsSinceEpoch)
        {
            var millisecondsToAdd = millisecondsSinceEpoch ?? 0;

            return EPOCH.AddMilliseconds(millisecondsToAdd);
        }

        public static bool IsEpoch(this DateTime dateTime)
        {
            return dateTime != null && dateTime.CompareTo(EPOCH) == 0;
        }
    }
}
