using Bogus;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleAccount.Commands.Login;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using HospitioApi.Test.TestConfigSettings;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAccount.Commands.LoginHandlerTestFixture;

namespace HospitioApi.Test.HandleAccount.Commands;
public class LoginHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public LoginHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    //[Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var jwtService = new JwtService(db, _fix.FakeJwtSettingsOptions, _fix.FakeJwtSettingsForGr4vyOptions, _fix.ResetPasswordOptions);
        var result = await _fix.BuildHandler(db, jwtService, _fix.FakeHttpContextAccessor)
         .Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Login successful.");
    }

    //[Fact]
    public async Task UserDoesntExists()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var jwtService = new JwtService(db, _fix.FakeJwtSettingsOptions, _fix.FakeJwtSettingsForGr4vyOptions, _fix.ResetPasswordOptions);

        var loginIn = new LoginIn("test@test.com", _fix.Fake.Internet.Password(10));

        var result = await _fix.BuildHandler(db, jwtService, _fix.FakeHttpContextAccessor)
         .Handle(new(loginIn), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Invalid login attempt.");
    }
}

public class LoginHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public LoginIn In { get; set; } = new LoginIn();
    public IHttpContextAccessor FakeHttpContextAccessor { get; set; } = A.Fake<IHttpContextAccessor>();
    public IOptions<JwtSettingsOptions> FakeJwtSettingsOptions { get; set; } = A.Fake<IOptions<JwtSettingsOptions>>();
    public IOptions<JwtSettingsForGr4vyOptions> FakeJwtSettingsForGr4vyOptions { get; set; } = A.Fake<IOptions<JwtSettingsForGr4vyOptions>>();
    public IOptions<ResetPasswordSettingsOptions> ResetPasswordOptions { get; set; } = A.Fake<IOptions<ResetPasswordSettingsOptions>>();
    public Faker Fake { get; set; } = new Faker();
    public LoginHandlerTestFixture()
    {
        var jwtSettingsOptionsFaker = new Faker<JwtSettingsOptions>()
            .RuleFor(m => m.JwtPemPrivateKey, TestConfigSettingsFile.TestJwtPemPrivateKey)
            .RuleFor(m => m.Issuer, f => f.Random.String2(10))
            .RuleFor(m => m.Audience, f => f.Random.String2(10))
            .RuleFor(m => m.RefreshTokenValidForHours, f => f.Random.Int(24, 48))
            .RuleFor(m => m.JwtValidForMinutes, f => f.Random.Int(60, 120));
        A.CallTo(() => FakeJwtSettingsOptions.Value).Returns(jwtSettingsOptionsFaker.Generate());
    }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();


        var loginFactory = LoginFactory.SeedSingle(db,out string password);
        In.Email = loginFactory.Email;
        In.Password = password;

    }

    public LoginHandler BuildHandler(ApplicationDbContext db, IJwtService jwtService, IHttpContextAccessor httpContextAccessor) =>
        new(db, Response, jwtService, httpContextAccessor);
}
