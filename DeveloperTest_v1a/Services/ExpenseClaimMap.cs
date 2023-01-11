namespace DeveloperTest_v1a.Services;

using CsvHelper.Configuration;
using DeveloperTest_v1a.Models;

public sealed class ExpenseClaimMap : ClassMap<ExpenseClaimData>
{
    public ExpenseClaimMap()
    {
        Map(m => m.Id);
        Map(m => m.Description);
        Map(m => m.Claimant);
        Map(m => m.Price);
        Map(m => m.Office);
        Map(m => m.Category);
    }
}