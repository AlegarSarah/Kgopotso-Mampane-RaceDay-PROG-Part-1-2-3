namespace RaceDay.Web.Helpers;

public static class Fmt
{
    public static string Ordinal(int n)
    {
        var suffix = (n % 100) is >= 11 and <= 13 ? "th" : (n % 10) switch { 1 => "st", 2 => "nd", 3 => "rd", _ => "th" };
        return $"{n}{suffix}";
    }
    public static string Time(TimeSpan t) => t.ToString(@"hh\:mm\:ss");
    public static string Date(DateTime d) => d.ToString("ddd, d MMM yyyy HH:mm");
}