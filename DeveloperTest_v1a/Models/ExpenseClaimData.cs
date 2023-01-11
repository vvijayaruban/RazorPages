namespace DeveloperTest_v1a.Models;

public class ExpenseClaimData
{

    public int Id { get; init; }
    public string Claimant { get; init; }
    public string Description { get; init; }
    public string Office { get; init; }
    public string Category { get; init; }
    public decimal Price { get; init; }

}