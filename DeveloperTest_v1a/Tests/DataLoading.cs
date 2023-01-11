namespace DeveloperTest_v1a.Tests;

using DeveloperTest_v1a.Services;
using NUnit.Framework;

[TestFixture]
public class DataLoading
{
    [Test]
    public async Task DataServiceReturnsData()
    {
        var dataService = new DataService();
        var data = await dataService.LoadData();

        Assert.IsTrue(data != null);
        Assert.IsTrue(data.Any());
    }
}