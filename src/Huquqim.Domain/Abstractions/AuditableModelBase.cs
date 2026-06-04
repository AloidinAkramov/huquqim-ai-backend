namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Audit ustunlarini qo'shuvchi asosiy klass: yaratilgan/yangilangan vaqt va kim.
/// </summary>
public abstract class AuditableModelBase<TId> : ModelBase<TId> where TId : struct
{
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }
}
