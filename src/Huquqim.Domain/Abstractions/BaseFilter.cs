namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Sahifalash uchun asosiy filtr. Barcha ro'yxat so'rovlari shundan meros oladi.
/// </summary>
public record BaseFilter
{
    private int _pageIndex = 1;
    private int _pageSize = 10;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is <= 0 or > 100 ? 10 : value;
    }

    public string? Search { get; set; }
}
