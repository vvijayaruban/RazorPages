namespace DeveloperTest_v1a.Tests;

using AutoFixture;
using DeveloperTest_v1a.Models;
using DeveloperTest_v1a.Services;
using NUnit.Framework;
using Shouldly;

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

[TestFixture]
public class PageFilterTests
{
    [TestCase(0)]
    [TestCase(-1)]
    public void PageSizeShouldBeANaturalNumber(int pageSize)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PageFilter(0));
    }

    [Test]
    public void MoveNextShouldGoToTheNextPage()
    {
        // Arrange
        var fixture = new Fixture();

        var expenseClaims = new List<ExpenseClaim>();
        fixture.AddManyTo(expenseClaims, 20);
        var pageFilter = new PageFilter(5);
        pageFilter.SetSource(expenseClaims);

        // Act
        pageFilter.MoveNext();

        // Assert
        pageFilter.PageIndex.ShouldBe(1);
    }

    [Test]
    public void TheFilterShouldFilterOutTheCurrentCategory()
    {
        // Arrange
        var fixture = new Fixture();
        var category = $"{Guid.NewGuid()}-{DateTime.UtcNow}";
        var expenseClaims = new List<ExpenseClaim>();
        fixture.AddManyTo(expenseClaims,20);
        expenseClaims.Add(fixture.Build<ExpenseClaim>().With(x => x.Category,category).Create());
        var pageFilter = new PageFilter(5){ Term = category };
        pageFilter.SetSource(expenseClaims);

        // Act
        pageFilter.Filter();

        // Assert
        pageFilter.Results.Count.ShouldBe(1);
        pageFilter.Results.Single().Category.ShouldBe(category);
    }


    [Test]
    public void TheFilterShouldFilterOutTheCurrentClaimant()
    {
        // Arrange
        var fixture = new Fixture();
        var searchTerm = $"Claimant-{Guid.NewGuid()}";
        var createClaimant = () => $"{searchTerm}-{DateTime.UtcNow}";
        var expenseClaims = new List<ExpenseClaim>();
        fixture.AddManyTo(expenseClaims, 20);
        var claim01 = fixture.Build<ExpenseClaim>().With(x => x.Claimant, createClaimant).Create();
        expenseClaims.Add(claim01);
        var claim02 = fixture.Build<ExpenseClaim>().With(x => x.Claimant, createClaimant).Create();
        expenseClaims.Add(claim02);
        var pageFilter = new PageFilter(5) {Term = searchTerm };
        pageFilter.SetSource(expenseClaims);

        // Act
        pageFilter.Filter();

        // Assert
        pageFilter.Results.Count.ShouldBe(2);
        pageFilter.Results.All(x => x.Claimant.Contains(searchTerm)).ShouldBeTrue();
    }

    [Test]
    public void TheFilterShouldFilterOutTheCurrentOffice()
    {
        // Arrange
        var fixture = new Fixture();
        var searchTerm = $"Claimant-{Guid.NewGuid()}";
        var createOffice = () => $"{searchTerm}-{DateTime.UtcNow}";
        var expenseClaims = new List<ExpenseClaim>();
        fixture.AddManyTo(expenseClaims, 20);
        var claim01 = fixture.Build<ExpenseClaim>().With(x => x.Office, createOffice).Create();
        expenseClaims.Add(claim01);
        var claim02 = fixture.Build<ExpenseClaim>().With(x => x.Office, createOffice).Create();
        expenseClaims.Add(claim02);
        var claim03 = fixture.Build<ExpenseClaim>().With(x => x.Office, searchTerm).Create();
        expenseClaims.Add(claim03);
        var pageFilter = new PageFilter(5) { Term = searchTerm };
        pageFilter.SetSource(expenseClaims);

        // Act
        pageFilter.Filter();

        // Assert
        pageFilter.Results.Count.ShouldBe(3);
        pageFilter.Results.All(x => x.Office.Contains(searchTerm)).ShouldBeTrue();
    }

    [Test]
    public void MoveNextShouldAlwaysBeLowerThanTheTotalPage()
    {
        // Arrange
        var fixture = new Fixture();

        var expenseClaims = new List<ExpenseClaim>();
        fixture.AddManyTo(expenseClaims, 20);
        var pageFilter = new PageFilter(10){ PageIndex = 1};
        pageFilter.SetSource(expenseClaims);

        // Act
        pageFilter.MoveNext();

        // Assert
        pageFilter.PageIndex.ShouldBe(1);
    }

    [Test]
    public void MovePreviousShouldGoToThePreviousPage()
    {
        // Arrange
        var fixture = new Fixture();

        var expenseClaims = new List<ExpenseClaim>();
        fixture.AddManyTo(expenseClaims, 20);
        var pageFilter = new PageFilter(5){ PageIndex = 2 };
        pageFilter.SetSource(expenseClaims);

        // Act
        pageFilter.MovePrevious();

        // Assert
        pageFilter.PageIndex.ShouldBe(1);
    }

    [Test]
    public void MovePreviousShouldNotGoToBelowZero()
    {
        // Arrange
        var fixture = new Fixture();

        var expenseClaims = new List<ExpenseClaim>();
        fixture.AddManyTo(expenseClaims, 20);
        var pageFilter = new PageFilter(5) { PageIndex = 0 };
        pageFilter.SetSource(expenseClaims);

        // Act
        pageFilter.MovePrevious();

        // Assert
        pageFilter.PageIndex.ShouldBe(0);
    }
}