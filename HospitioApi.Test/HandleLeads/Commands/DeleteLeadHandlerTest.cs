using Azure.Core;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using HospitioApi.Core.HandleLeads.Commands.DeleteLead;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleLeads.Commands.DeleteLeadHandlerTestFixture;

namespace HospitioApi.Test.HandleLeads.Commands
{
    public class DeleteLeadHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        private readonly ITestOutputHelper _output;

        public DeleteLeadHandlerTest(ThisTestFixture fixture, ITestOutputHelper output) {
            _fix = fixture;
            _output = output;
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete lead successful.");
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualId = _fix.In.LeadId;
            /*_fix.In.LeadId = 0;*/

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Lead with id {actualId} not found or user doesn't have the rights to delete it");

            _fix.In.LeadId = actualId;
        }
    }

    public class DeleteLeadHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DeleteLeadIn In { get; set; } = new DeleteLeadIn();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var user = UserFactory.SeedSingle(db);
            var lead = LeadsFactory.SeedSingle(db, user.Id);

            In.LeadId = lead.Id;
        }

        public DeleteUnitHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}

