using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class CustomerUserRefreshToken
{
    public CustomerUserRefreshToken() { }
    public CustomerUserRefreshToken(int customerUserId, string token, DateTime expires, string? remoteIpAddress)
    {
        var UtcNow = DateTime.UtcNow;
        CustomerUserId = customerUserId;
        CreatedUtc = UtcNow;
        Token = token;
        ExpiresUtc = expires;
        CreatedByIp = remoteIpAddress;
    }
    public int Id { get; set; }
    public int CustomerUserId { get; set; }
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
    [ForeignKey("CustomerUserId")]
    public virtual CustomerUser CustomerUser { get; set; } = null!;
}
