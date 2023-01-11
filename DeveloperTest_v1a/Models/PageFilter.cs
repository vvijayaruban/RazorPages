namespace DeveloperTest_v1a.Models;

public class PageFilter
{
    private const int DefaultPageSize = 20;

    private List<ExpenseClaim>? _source;

    public PageFilter(int pageSize)
    {
        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size should be a natural number.");
        }

        PageSize = pageSize;
        PageIndex = 0;
        Results = new ExpenseClaim[] { };
    }

    public PageFilter() : this(DefaultPageSize)
    {
    }

    public int PageSize { get; }
    public int? PageIndex { get; set; }

    public int TotalPages
    {
        get
        {
            if (_source == null)
            {
                return 0;
            }

            return _source.Count / PageSize;
        }
    }

    public string? Term { get; set; }

    public IReadOnlyCollection<ExpenseClaim> Results { get; private set; }

    public void MoveNext()
    {
        if (PageIndex + 1 == TotalPages) return;
        PageIndex++;
    }

    public void MovePrevious()
    {
        if (PageIndex == 0) return;
        PageIndex--;
    }

    public void SetSource(IEnumerable<ExpenseClaim> source)
    {
        _source = source.ToList();
    }

    public void Filter()
    {
        IEnumerable<ExpenseClaim> TryFilter()
        {
            var source = _source ??= new List<ExpenseClaim>();

            if (string.IsNullOrWhiteSpace(Term))
            {
                return source;
            }

            return source.Where(claim =>
                Contains(claim.Category,Term) || Contains(claim.Claimant,Term) || Contains(claim.Office,Term));
        }

        Results = TryFilter().Skip(PageSize * PageIndex ?? 0).Take(PageSize).ToArray();
    }

    private static bool Contains(string? fieldValue, string term)
    {
        return !string.IsNullOrWhiteSpace(fieldValue) && fieldValue.Contains(term, StringComparison.CurrentCultureIgnoreCase);
    }
}