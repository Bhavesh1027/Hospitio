using FakeItEasy;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomersByGuIds;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Queries.GetCustomersByGuIdsHandlerFixure;

namespace HospitioApi.Test.HandleCustomers.Queries
{
    public class GetCustomersByGuIdsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomersByGuIdsHandlerTest(ThisTestFixture fixture) => _fix = fixture;
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<Customers>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Out);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer successful.");
        }
        [Fact]
        public async Task NotFound_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            var Obj = _fix.Out;
            _fix.Out = null;
            A.CallTo(() => _dapper.GetSingle<Customers>(A<string>.Ignored, null, CancellationToken.None,
               CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Out);
            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.Out =  Obj;
        }
    }
    public class GetCustomersByGuIdsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomersByGuIdsIn In { get; set; } = new();
        public Customers Out { get;set; } = new Customers();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customers = CustomerFactory.SeedMany(db,3);
            In.guids = new List<Guid>();
            
            foreach(var customer in customers)
            {
                In.guids.Add(customer.Guid);
            }
        }
        public GetCustomersByGuIdsHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);

    }
}
