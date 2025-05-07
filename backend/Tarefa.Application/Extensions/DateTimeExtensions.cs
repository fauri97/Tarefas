namespace Tarefa.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToBrasiliaTime(this DateTime utcDate)
        {
            var timeZoneId = OperatingSystem.IsWindows()
                ? "E. South America Standard Time"
                : "America/Sao_Paulo";

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZone);
        }
    }
}