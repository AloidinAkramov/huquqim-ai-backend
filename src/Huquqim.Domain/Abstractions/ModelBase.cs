using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Barcha entity'lar uchun asosiy klass: Id va yumshoq o'chirish (soft delete).
/// </summary>
public abstract class ModelBase<TId> where TId : struct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; }

    public bool IsDeleted { get; set; }
}
