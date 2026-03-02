using Bogus;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Moq;
using HospitioApi.Core.HandleLeads.Queries.GetLeads;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.MultiTenancy;
using System.Data;
using Xunit;
using static HospitioApi.Core.HandleLeads.Queries.GetLeads.GetLeadsOut;
using ThisTestFixture = HospitioApi.Test.HandleLeads.Queries.GetLeadsHandlerTestFixture;

namespace HospitioApi.Test.HandleLeads.Queries
{
    public class GetLeadsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetLeadsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            A.CallTo(() => _dapper.GetAll<LeadsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.LeadsOut);

            var result = await _fix.BuildHandler(_dapper,db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get leads successful.");

            var LeadsOut = (GetLeadsOut)result.Response;
            Assert.NotNull(LeadsOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(_dapper.Object,db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetLeadsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<LeadsOut> LeadsOut { get; set; } = new();
        public GetLeadsIn In { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var user = UserFactory.SeedSingle(db);
            var leads = LeadsFactory.SeedMany(db, user.Id,1);

            In.SearchValue = "test";

            foreach (var lead in leads)
            {
                LeadsOut obj = new()
                {
                    Id = lead.Id,
                    Name = lead.FirstName+' '+lead.LastName,
                    Email = lead.Email,
                    Company = lead.Company,
                    Comment = lead.Comment,
                    PhoneNumber = lead.PhoneNumber,
                    ContactFor = user.Id,
                    IsActive = lead.IsActive
                };
                LeadsOut.Add(obj);
            }
        }

        public GetLeadsHandler BuildHandler(IDapperRepository _dapper, ApplicationDbContext db) =>
            new(_dapper, db,Response);
    }
}

