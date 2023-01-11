namespace DeveloperTest_v1a.Services;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DeveloperTest_v1a.Models;

public interface IDataService
{
    Task<IEnumerable<ExpenseClaim>> LoadData();
}

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

public class DataService : IDataService
{
    private List<ExpenseClaim> _expenseClaims;

    public DataService()
    {
        _expenseClaims = new List<ExpenseClaim>();
    }

    public async Task<IEnumerable<ExpenseClaim>> LoadData()
    {
        using var reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<ExpenseClaimMap>();
        _expenseClaims = new List<ExpenseClaim>();
        await foreach (var claimData in csv.GetRecordsAsync<ExpenseClaimData>())
        {
            _expenseClaims.Add(new ExpenseClaim(claimData.Id, claimData.Claimant, claimData.Description,
                claimData.Office, claimData.Category, claimData.Price));
        }

        return _expenseClaims;
    }
}