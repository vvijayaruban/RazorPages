namespace DeveloperTest_v1a.Services;

using System.Globalization;
using CsvHelper;
using DeveloperTest_v1a.Models;

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