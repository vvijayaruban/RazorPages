namespace DeveloperTest_v1a.Services;

using DeveloperTest_v1a.Models;

public interface IDataService
{
    Task<IEnumerable<ExpenseClaim>> LoadData();
}