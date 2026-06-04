namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Sahifalangan ro'yxat natijasi.
/// </summary>
public record PagedList<T>
{
    public PagedList(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public List<T> Items { get; set; }

    public int TotalCount { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }

    public bool HasPrevious => PageIndex > 1;

    public bool HasNext => PageIndex < TotalPages;
}
