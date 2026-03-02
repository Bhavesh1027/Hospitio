
using Bogus;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleAccount.Commands.CustomerLogin;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using HospitioApi.Test.TestConfigSettings;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAccount.Commands.CustomerLoginHandlerTestFixture;

namespace HospitioApi.Test.HandleAccount.Commands;

public class CustomerLoginHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CustomerLoginHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    //[Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var jwtService = new JwtService(db, _fix.FakeJwtSettingsOptions, _fix.FakeJwtSettingsForGr4vyOptions, _fix.ResetPasswordOptions);
        //var loginIn = new CustomerLoginIn(_fix.TestUserEmail, _fix.Fake.Internet.Password(10));
        var loginIn = new CustomerLoginIn(_fix.TestUserEmail, _fix.TestUserPassword);
        var result = await _fix.BuildHandler(db, jwtService, _fix.FakeHttpContextAccessor)
         .Handle(new(loginIn!), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer login successful.");
       

        var loginOut = (CustomerLoginOut)result.Response;
        Assert.True(!string.IsNullOrWhiteSpace(loginOut.AccessToken.Jwt));
        Assert.True(!string.IsNullOrWhiteSpace(loginOut.RefreshToken.Token));
        Assert.True(loginOut.RefreshToken.Expires.DateTimeUTC > DateTime.UtcNow);
        Assert.True(loginOut.RefreshToken.TokenId > 0);
       // Assert.NotNull(db.RefreshTokens.SingleOrDefault(t => t.Id == loginOut.RefreshToken.TokenId));
    }

    [Fact]
    public async Task UserDoesntExists()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var jwtService = new JwtService(db, _fix.FakeJwtSettingsOptions, _fix.FakeJwtSettingsForGr4vyOptions, _fix.ResetPasswordOptions);
     
        var loginIn = new CustomerLoginIn(_fix.TestUserEmail,_fix.Fake.Internet.Password(10));
        
        var result = await _fix.BuildHandler(db, jwtService, _fix.FakeHttpContextAccessor)
         .Handle(new(loginIn), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Invalid CustomerLogin attempt.");
    }
}

public class CustomerLoginHandlerTestFixture : DbFixture
{
    public Faker Fake { get; set; } = new Faker();
    public IOptions<JwtSettingsOptions> FakeJwtSettingsOptions { get; set; } = A.Fake<IOptions<JwtSettingsOptions>>();
    public IOptions<JwtSettingsForGr4vyOptions> FakeJwtSettingsForGr4vyOptions { get; set; } = A.Fake<IOptions<JwtSettingsForGr4vyOptions>>();
    public IOptions<ResetPasswordSettingsOptions> ResetPasswordOptions { get; set; } = A.Fake<IOptions<ResetPasswordSettingsOptions>>();
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    
    public IHttpContextAccessor FakeHttpContextAccessor { get; set; } = A.Fake<IHttpContextAccessor>();

    public IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions { get; set; } = A.Fake<IOptions<MiddlewareApiSettingsOptions>>();

    //public CustomerLoginIn In { get; set; } = new CustomerLoginIn();

    public CustomerLoginHandlerTestFixture()
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
    public string TestUserPassword { get; set; } = string.Empty;
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerLoginFactory = CustomerLoginFactory.SeedSingle(db);
        //In.Email= customerLoginFactory.Email;
        //In.Password= customerLoginFactory.Password;
        TestUserEmail=customerLoginFactory.Email;
        TestUserPassword = customerLoginFactory.Password;
    }

    public CustomerLoginHandler BuildHandler(ApplicationDbContext db, IJwtService jwtService, IHttpContextAccessor httpContextAccessor) =>
        new(db, Response, jwtService, httpContextAccessor, middlewareApiSettingsOptions);
}