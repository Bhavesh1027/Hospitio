using FakeItEasy;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByGuId;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByIdForHospitio;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Queries.GetCustomerByGuIdHandlerFixure;

namespace HospitioApi.Test.HandleCustomers.Queries
{
    public class GetCustomerByGuIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomerByGuIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<CustomerByGuIdOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.Out);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer successful.");
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            CustomerByGuIdOut? obj = null;

            A.CallTo(() => _dapper.GetSingle<CustomerByGuIdOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(obj);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }
    public class GetCustomerByGuIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerByGuIdIn In { get; set; } = new();
        public CustomerByGuIdOut Out { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var custmers = CustomerFactory.SeedSingle(db);

            Out.Id = custmers.Id;
            Out.Guid = custmers.Guid;
            Out.NoOfRooms = custmers.NoOfRooms;
            
        }
        public GetCustomerByGuIdHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
    }
}
