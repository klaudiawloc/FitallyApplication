namespace Fitally.Common
{
    public static class DateHelper
    {
         public static string FormatDate(DateTime date)
        {
            return date.ToString("dd.MM.yyyy HH:mm");
        }

    }
}
