using Azure.Core;
using FakeItEasy;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.UpdateCustomersPropertiesInfo;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using System.Reflection;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPropertiesInfo.Commands.UpdateCustomersPropertiesInfoHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersPropertiesInfo.Commands;

public class UpdateCustomersPropertiesInfoHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly ITestOutputHelper _output;

    public UpdateCustomersPropertiesInfoHandlerTest(ThisTestFixture fixture, ITestOutputHelper output)
    {
        _fix = fixture;
        _output = output;
    }

    [Fact]
    public async Task Update_Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.customer.Id.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer property info Successful");
    }

    [Fact]
    public async Task Add_Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.customer.Id.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer property info successful.");

        _fix.In.Id = actualId;
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;
        _fix.PropertyInfo.IsActive = true;
        _fix.customerProperyInformationFactory.Update(db, _fix.PropertyInfo);

        var result = await _fix.BuildHandler(db).Handle(new UpdateCustomersPropertiesInfoRequest(_fix.In, _fix.customer.Id.ToString()), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"{_fix.customer.BusinessName} is already exists in customer property information.");

        _fix.In.Id = actualId;
        _fix.PropertyInfo.IsActive = false;
        _fix.customerProperyInformationFactory.Update(db, _fix.PropertyInfo);
    }
}

public class UpdateCustomersPropertiesInfoHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomersPropertiesInfoIn In { get; set; } = new UpdateCustomersPropertiesInfoIn();
    public Customer customer { get; set; } = new();
    public CustomerPropertyInformation PropertyInfo { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        customer = CustomerFactory.SeedSingle(db);
        var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        PropertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);

        In.Id = PropertyInfo.Id;
        In.CustomerGuestAppBuilderId = guestAppBulder.Id;
        In.WifiUsername = PropertyInfo.WifiUsername;
        In.WifiPassword = PropertyInfo.WifiPassword;
        In.Overview = PropertyInfo.Overview;
        In.CheckInPolicy = PropertyInfo.CheckInPolicy;
        In.TermsAndConditions = PropertyInfo.TermsAndConditions;
        In.Street = PropertyInfo.Street;
        In.StreetNumber = PropertyInfo.StreetNumber;
        In.City = PropertyInfo.City;
        In.Postalcode = PropertyInfo.Postalcode;
        In.Country = PropertyInfo.Country;

    }

    public UpdateCustomersPropertiesInfoHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}


