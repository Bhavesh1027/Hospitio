namespace HospitioApi.Shared;

public class DateTimeUtcUnixEpoch
{
    public DateTimeUtcUnixEpoch(DateTime dateTimeUtc)
    {
        DateTimeUTC = dateTimeUtc;
        UnixEpoch = DateToUnixEpoch(dateTimeUtc);
    }

    public DateTime DateTimeUTC { get; set; }
    public long UnixEpoch { get; set; }

    /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
    private static long DateToUnixEpoch(DateTime dateTimeUtc)
    {
        return (long)Math.Round((dateTimeUtc.ToUniversalTime() -
                 new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}
