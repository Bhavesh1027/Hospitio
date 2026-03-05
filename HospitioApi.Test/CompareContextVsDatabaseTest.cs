using Xunit;
using Xunit.Abstractions;

namespace HospitioApi.Test;

public class CompareContextVsDatabaseTest
{
    private readonly ITestOutputHelper _output;

    public CompareContextVsDatabaseTest(ITestOutputHelper output)
    {
        _output = output;
    }
    //[Fact(Skip = "For development use only")]
    //[Fact]
    public void Compare()
    {
        var LOCAL_ConnectionString = "<--Your Key -->";
        var UAT_ConnectionString = "";
        var PROD_ConnectionString = "";

        var connectionStrings = new Dictionary<string, string> {
            { nameof(LOCAL_ConnectionString), LOCAL_ConnectionString },
            { nameof(UAT_ConnectionString), UAT_ConnectionString },
            { nameof(PROD_ConnectionString ), PROD_ConnectionString }
        };

        string totalErrors = string.Empty;
        List<string> pendingmigrations = new List<string>();

        //foreach (var connectionString in connectionStrings)
        //{
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseSqlServer(connectionString.Value)
        //        .Options;

        //    using var context = new ApplicationDbContext(options);
        //    var comparer = new CompareEfSql();

        //    var hasErrors = comparer.CompareEfWithDb(context);
        //    string allErrors = comparer.GetAllErrors;
        //    _output.WriteLine($"======================={connectionString.Key}=======================");
        //    _output.WriteLine(allErrors);
        //    totalErrors += allErrors;

        //    pendingmigrations = context.Database.GetPendingMigrations().ToList();

        //    if (pendingmigrations.Any())
        //    {
        //        _output.WriteLine($"======{connectionString.Key} Pending Migrations To be Applied======");
        //        pendingmigrations.ForEach(migration => _output.WriteLine(migration));
        //    }
        //}

        Assert.Empty(totalErrors);
        Assert.True(pendingmigrations.Count() == 0);
    }
}