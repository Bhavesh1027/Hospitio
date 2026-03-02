using Azure.Core;
using HospitioApi.Core.HandleLeads.Commands.EditLead;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleLeads.Commands.EditLeadHandlerTestFixture;

namespace HospitioApi.Test.HandleLeads.Commands
{

    public class EditLeadHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        private readonly ITestOutputHelper _output;

        public EditLeadHandlerTest(ThisTestFixture fixture, ITestOutputHelper output) { 
            _fix = fixture;
            _output = output;
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Lead edited successfully.");
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualId = _fix.In.Id;
            var actualContactFor = _fix.In.ContactFor;
            _fix.In.ContactFor = 0;
            _fix.In.Id = 0;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Lead could not be found.");

            _fix.In.Id = actualId;
            _fix.In.ContactFor = actualContactFor;
        }
    }

    public class EditLeadHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public EditLeadIn In { get; set; } = new EditLeadIn();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var user = UserFactory.SeedSingle(db);
            var lead = LeadsFactory.SeedSingle(db, user.Id);

            In.Id = lead.Id;
            In.FirstName = "Test";
            In.LastName = "Test";
            In.Company = "Test Company";
            In.Comment = "This is test comment";
            In.Email = "test@gmail.com";
            In.PhoneCountry = "in";
            In.PhoneNumber = "1234567890";
            In.ContactFor = user.Id;
            In.IsActive = true;
        }

        public EditLeadHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}
