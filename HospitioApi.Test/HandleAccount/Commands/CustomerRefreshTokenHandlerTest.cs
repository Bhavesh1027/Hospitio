using Bogus;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleAccount.Commands.CustomerRefreshToken;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using HospitioApi.Test.TestConfigSettings;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAccount.Commands.CustomerRefreshTokenHandlerTestFixture;

namespace HospitioApi.Test.HandleAccount.Commands;

public class CustomerRefreshTokenHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CustomerRefreshTokenHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task RefreshTokenSuccess()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var jwtService = new JwtService(db, _fix.FakeJwtSettingsOptions, _fix.FakeJwtSettingsForGr4vyOptions, _fix.ResetPasswordOptions);
      
        var customerUser = db.CustomerUsers.Single(u => u.Email == _fix.TestUserEmail);

        var refreshTokenIn = new CustomerRefreshTokenIn { TokenValue = _fix.Fake.Random.String2(20) };

        var refreshToken = new CustomerUserRefreshToken(customerUser.Id, refreshTokenIn.TokenValue, DateTime.UtcNow.AddHours(1), _fix.Fake.Internet.Ip());
        db.CustomerUserRefreshTokens.Add(refreshToken);
        db.SaveChanges();

        refreshTokenIn.TokenId = refreshToken.Id;

        var result = await _fix.BuildHandler(db, jwtService, _fix.FakeHttpContextAccessor)
            .Handle(new(refreshTokenIn), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Token refreshed successfully.");
        var refreshTokenOut = (CustomerRefreshTokenOut)result.Response;
        Assert.True(!string.IsNullOrWhiteSpace(refreshTokenOut.AccessToken.Jwt));
        Assert.True(!string.IsNullOrWhiteSpace(refreshTokenOut.RefreshToken.Token));
        Assert.True(refreshTokenOut.RefreshToken.Expires.DateTimeUTC > DateTime.UtcNow);
        Assert.True(refreshTokenOut.RefreshToken.TokenId > 0);
        Assert.True(refreshTokenOut.RefreshToken.TokenId != refreshTokenIn.TokenId);
        Assert.NotNull(db.CustomerUserRefreshTokens.SingleOrDefault(t => t.Id == refreshTokenOut.RefreshToken.TokenId));
    }

    [Fact]
    public async Task RefreshError_TokenAlreadyRevoked()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var jwtService = new JwtService(db, _fix.FakeJwtSettingsOptions, _fix.FakeJwtSettingsForGr4vyOptions, _fix.ResetPasswordOptions);

        var user = db.CustomerUsers.Single(u => u.Email == _fix.TestUserEmail);

        var refreshTokenIn = new CustomerRefreshTokenIn { TokenValue = _fix.Fake.Random.String2(20) };

        var refreshToken = new CustomerUserRefreshToken(user.Id, refreshTokenIn.TokenValue, DateTime.UtcNow.AddHours(1), _fix.Fake.Internet.Ip())
        {
            Revoked = DateTime.UtcNow
        };

        db.CustomerUserRefreshTokens.Add(refreshToken);
        db.SaveChanges();

        refreshTokenIn.TokenId = refreshToken.Id;

        var result = await _fix.BuildHandler(db, jwtService, _fix.FakeHttpContextAccessor)
            .Handle(new(refreshTokenIn), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.Equal("Invalid refresh token.", result.Failure!.Message);
        Assert.NotNull(db.CustomerUserRefreshTokens.SingleOrDefault(t => t.Id == refreshTokenIn.TokenId));
    }
}

public class CustomerRefreshTokenHandlerTestFixture : DbFixture
{
    public Faker Fake { get; set; } = new Faker();
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CustomerRefreshTokenIn In { get; set; } = new CustomerRefreshTokenIn();
    public IHttpContextAccessor FakeHttpContextAccessor { get; set; } = A.Fake<IHttpContextAccessor>();
    public IOptions<JwtSettingsOptions> FakeJwtSettingsOptions { get; set; } = A.Fake<IOptions<JwtSettingsOptions>>();
    public IOptions<JwtSettingsForGr4vyOptions> FakeJwtSettingsForGr4vyOptions { get; set; } = A.Fake<IOptions<JwtSettingsForGr4vyOptions>>();
    public IOptions<ResetPasswordSettingsOptions> ResetPasswordOptions { get; set; } = A.Fake<IOptions<ResetPasswordSettingsOptions>>();

    public CustomerRefreshTokenHandlerTestFixture()
    {
        var jwtSettingsOptionsFaker = new Faker<JwtSettingsOptions>()
            .RuleFor(m => m.JwtPemPrivateKey, TestConfigSettingsFile.TestJwtPemPrivateKey)
            .RuleFor(m => m.Issuer, f => f.Random.String2(10))
            .RuleFor(m => m.Audience, f => f.Random.String2(10))
            .RuleFor(m => m.RefreshTokenValidForHours, f => f.Random.Int(24, 48))
            .RuleFor(m => m.JwtValidForMinutes, f => f.Random.Int(60, 120));
        A.CallTo(() => FakeJwtSettingsOptions.Value).Returns(jwtSettingsOptionsFaker.Generate());
    }
    public string TestUserEmail { get; set; } = string.Empty;
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
      
        var customerLevel = CustomerLevelFactory.SeedSingle(db);
        var user = CustomerUserFactory.SeedSingle(db,customerLevel);
        TestUserEmail = user.Email;

    }

    public CustomerRefreshTokenHandler BuildHandler(ApplicationDbContext db,IJwtService jwtService,IHttpContextAccessor httpContextAccessor)=>
          new (db,Response,jwtService, httpContextAccessor);
}