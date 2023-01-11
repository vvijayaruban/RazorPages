namespace DeveloperTest_v1a.Pages;

using DeveloperTest_v1a.Models;
using DeveloperTest_v1a.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private readonly IDataService _dataService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        ILogger<IndexModel> logger,
        IDataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    [BindProperty] public PageFilter PageFilter { get; set; }

    public async Task<IActionResult> OnGet()
    {
        await Paginate();
        return Page();
    }

    private async Task Paginate()
    {
        var claims = await _dataService.LoadData();
        var pageFilter = PageFilter ??= new PageFilter();
        pageFilter.SetSource(claims);
        pageFilter.Filter();
    }

    public async Task<IActionResult> OnPostFilter()
    {
        await Paginate();
        return Page();
    }

    public async Task<IActionResult> OnPostPreviousClick()
    {
        PageFilter.MovePrevious();
        ModelState.Clear();
        await Paginate();
        return Page();
    }

    public async Task<IActionResult> OnPostNextClick()
    {
        PageFilter.MoveNext();
        ModelState.Clear();
        await Paginate();
        return Page();
    }
}