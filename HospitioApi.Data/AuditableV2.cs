using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data;

public interface IAuditableV2<TId>
{
    public TId Id { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public abstract class AuditableV2<TId> : IAuditableV2<TId>
{
    [Key]
    public virtual TId Id { get; set; } = default!;

    public virtual string? UpdatedBy { get; set; }

    [DefaultValue(true)]
    [Column(TypeName = "datetime")]
    public virtual DateTime UpdatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public virtual DateTime CreatedAt { get; set; }
}

public interface IAuditableV2_Deleted<TId>
{
    public long DeletedId { get; set; }
    public TId Id { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}

public abstract class AuditableV2_Deleted<TId> : IAuditableV2_Deleted<TId>
{
    [Key]
    public virtual long DeletedId { get; set; }
    public virtual TId Id { get; set; } = default!;
    public virtual string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public virtual DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public virtual DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public virtual DateTime DeletedAt { get; set; }
    public virtual string? DeletedBy { get; set; }
}

public abstract class AuditableV2 : AuditableV2<long> { }
public abstract class AuditableV2_Deleted : AuditableV2_Deleted<long?> { }
