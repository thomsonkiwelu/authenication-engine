namespace authentication_engine.Shared
{
    public class DateTimeHelper
    {
        public static DateTime? ParseNullableDate(string dateString)
        {
            if (!DateTime.TryParse(dateString, out var dateTime))
                return null;

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public static DateTime? GetMinDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var minValue = ParseNullableDate($"{value} 00:00:00");

            return minValue;
        }

        public static DateTime? GetMaxDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var maxValue = ParseNullableDate($"{value} 23:59:59");

            return maxValue;
        }
    }
}
