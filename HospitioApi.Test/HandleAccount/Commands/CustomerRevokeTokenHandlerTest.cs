using Bogus;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using HospitioApi.Core.HandleAccount.Commands.CustomerRevokeToken;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAccount.Commands.CustomerRevokeTokenHandlerTestFixture;

namespace HospitioApi.Test.HandleAccount.Commands;


public class CustomerRevokeTokenHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CustomerRevokeTokenHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task RevokeSuccess()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var user = db.CustomerUsers.Single(u => u.Email == _fix.TestUserEmail);

        var revokeTokenIn = new CustomerRevokeTokenIn { TokenValue = _fix.Fake.Random.String2(20) };

        var refreshTokenToRevoke = new CustomerUserRefreshToken(user.Id, revokeTokenIn.TokenValue, DateTime.UtcNow.AddHours(1), _fix.Fake.Internet.Ip());
        db.CustomerUserRefreshTokens.Add(refreshTokenToRevoke);
        db.SaveChanges();

        revokeTokenIn.TokenId = refreshTokenToRevoke.Id;

        var result = await _fix.BuildHandler(db, _fix.FakeHttpContextAccessor)
            .Handle(new(revokeTokenIn), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Token revoked successfully.");
       // var revokeTokenOut = (CustomerRevokeTokenOut)result.Response;
       // Assert.NotNull(db.CustomerUserRefreshTokens.SingleOrDefault(t => t.Id == revokeTokenIn.TokenId && t.Revoked != null));
    }

    [Fact]
    public async Task RevokeError_TokenIdNotFound()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var user = db.CustomerUsers.Single(u => u.Email == _fix.TestUserEmail);
        var revokeTokenIn = new CustomerRevokeTokenIn(_fix.Fake.Random.Int(9999, 999999), _fix.Fake.Random.String2(20));

        var result = await _fix.BuildHandler(db, _fix.FakeHttpContextAccessor)
            .Handle(new(revokeTokenIn), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.Equal("Invalid token.", result.Failure!.Message);
        Assert.Null(db.CustomerUserRefreshTokens.SingleOrDefault(t => t.Id == revokeTokenIn.TokenId));
    }

    [Fact]
    public async Task RevokeError_TokenAlreadyRevoked()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var user = db.CustomerUsers.Single(u => u.Email == _fix.TestUserEmail);

        var revokeTokenIn = new CustomerRevokeTokenIn { TokenValue = _fix.Fake.Random.String2(20) };

        var refreshTokenToRevoke = new CustomerUserRefreshToken(user.Id, revokeTokenIn.TokenValue, DateTime.UtcNow.AddHours(1), _fix.Fake.Internet.Ip())
        {
            Revoked = DateTime.UtcNow
        };

        db.CustomerUserRefreshTokens.Add(refreshTokenToRevoke);
        db.SaveChanges();

        revokeTokenIn.TokenId = refreshTokenToRevoke.Id;

        var result = await _fix.BuildHandler(db, _fix.FakeHttpContextAccessor)
            .Handle(new(revokeTokenIn), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.Equal("Invalid token.", result.Failure!.Message);
        Assert.NotNull(db.CustomerUserRefreshTokens.SingleOrDefault(t => t.Id == revokeTokenIn.TokenId));
    }

    [Fact]
    public void ValidationSuccess()
    {
        var revokeTokenInFaker = new Faker<CustomerRevokeTokenIn>()
            .CustomInstantiator(f => new(f.Random.Int(1, 100), f.Random.String2(10)));

        var revokeTokenInsA = revokeTokenInFaker.Generate(50);
        foreach (var @in in revokeTokenInsA)
        {
            var request = new CustomerRevokeTokenHandlerRequest(@in);
            var validator = new CustomerRevokeTokenValidator();

            var validationResult = validator.Validate(request);
            Assert.True(validationResult.IsValid);
        }

        var revokeTokenInsB = revokeTokenInFaker.Generate(50);
        foreach (var @in in revokeTokenInsB)
        {
            var request = new CustomerRevokeTokenHandlerRequest(@in);
            var validator = new CustomerRevokeTokenValidator();

            var validationResult = validator.Validate(request);
            Assert.True(validationResult.IsValid);
        }

        var revokeTokenInsC = revokeTokenInFaker.Generate(50);
        foreach (var @in in revokeTokenInsC)
        {
            var request = new CustomerRevokeTokenHandlerRequest(@in);
            var validator = new CustomerRevokeTokenValidator();

            var validationResult = validator.Validate(request);
            Assert.True(validationResult.IsValid);
        }
    }

    [Fact]
    public void ValidationFailure_TokenValueCannotBeEmpty()
    {
        const string errorMessage = "'Token Value' must not be empty.";
        var validator = new CustomerRevokeTokenValidator();

        var inA = new CustomerRevokeTokenIn(_fix.Fake.Random.Int(1, 100), string.Empty);
        var requestA = new CustomerRevokeTokenHandlerRequest(inA);
        var validationResultA = validator.Validate(requestA);
        Assert.False(validationResultA.IsValid);
        Assert.Contains(validationResultA.Errors.Select(e => e.ErrorMessage), m => m == errorMessage);

        var inB = new CustomerRevokeTokenIn(_fix.Fake.Random.Int(1, 100), null!);
        var requestB = new CustomerRevokeTokenHandlerRequest(inB);
        var validationResultB = validator.Validate(requestB);
        Assert.False(validationResultB.IsValid);
        Assert.Contains(validationResultB.Errors.Select(e => e.ErrorMessage), m => m == errorMessage);
    }

    [Fact]
    public void ValidationFailure_TokenIdCannotBeEmpty()
    {
        const string errorMessage = "'Token Id' must not be empty.";
        var validator = new CustomerRevokeTokenValidator();

        var @in = new CustomerRevokeTokenIn(default, _fix.Fake.Random.String2(10));
        var request = new CustomerRevokeTokenHandlerRequest(@in);
        var validationResult = validator.Validate(request);
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors.Select(e => e.ErrorMessage), m => m == errorMessage);
    }
}

public class CustomerRevokeTokenHandlerTestFixture : DbFixture
{
    public Faker Fake { get; set; } = new Faker();
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public IHttpContextAccessor FakeHttpContextAccessor { get; set; } = A.Fake<IHttpContextAccessor>();

    public string TestUserEmail { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        /** Seed db data that is common for all the tests in this file **/

        /** Seed Clinician Users **/
        var user = CustomerUserFactory.SeedSingle(db);
        TestUserEmail = user.Email;
    }

    public CustomerRevokeTokenHandler BuildHandler(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor) =>
        new(db, Response, httpContextAccessor);
}
