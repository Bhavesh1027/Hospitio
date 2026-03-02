using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data;

public interface IAuditable<TId>
{
    public TId Id { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? CreatedBy { get; set; }



}

public abstract class Auditable<TId> : IAuditable<TId>
{
    [Key, Column(Order = 0)]
    public virtual TId Id { get; set; } = default!;

    public virtual bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public virtual DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public virtual DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public virtual DateTime? DeletedAt { get; set; }

    public virtual int? CreatedBy { get; set; }

}

public abstract class Auditable : Auditable<int> { }