using Bogus;
using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleLeads.Queries.GetLeadById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleLeads.Queries.GetLeadByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleLeads.Queries
{
    public class GetLeadByIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetLeadByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<LeadByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.LeadByIdOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.LeadByIdOut.Id }), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get lead successful.");

            var leadByIdOut = (GetLeadByIdOut)result.Response;
            Assert.NotNull(leadByIdOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var actualId = _fix.LeadByIdOut.Id;
            _fix.LeadByIdOut.Id = 0;

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.LeadByIdOut.Id }), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");

            _fix.LeadByIdOut.Id = actualId;
        }
    }

    public class GetLeadByIdHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public LeadByIdOut LeadByIdOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var user = UserFactory.SeedSingle(db);
            var lead = LeadsFactory.SeedSingle(db, user.Id);

            LeadByIdOut = new()
            {
                Id = lead.Id,
                FirstName = lead.FirstName,
                LastName = lead.LastName,
                Email = lead.Email,
                Company = lead.Company,
                Comment = lead.Comment,
                PhoneCountry = lead.PhoneCountry,
                PhoneNumber = lead.PhoneNumber,
                ContactFor = user.Id,
                IsActive = lead.IsActive,
            };
        }

        public GetLeadByIdHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}
