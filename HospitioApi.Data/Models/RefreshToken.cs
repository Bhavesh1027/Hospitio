using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class RefreshToken
{
    public RefreshToken() { }
    public RefreshToken(int userId, string token, DateTime expires, string? remoteIpAddress)
    {
        var UtcNow = DateTime.UtcNow;
        UserId = userId;
        CreatedUtc = UtcNow;
        Token = token;
        ExpiresUtc = expires;
        CreatedByIp = remoteIpAddress;
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedUtc { get; set; }
    [MaxLength(256)]
    public string Token { get; set; } = null!;
    public DateTime ExpiresUtc { get; set; }
    [MaxLength(128)]
    public string? CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    [MaxLength(128)]
    public string? RevokedByIp { get; set; }
    [MaxLength(256)]
    public string? ReplacedByToken { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
