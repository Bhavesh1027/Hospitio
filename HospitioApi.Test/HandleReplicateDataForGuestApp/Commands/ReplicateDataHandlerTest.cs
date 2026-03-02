using FakeItEasy;
using HospitioApi.Core.HandleReplicateDataForGuestApp.Commands.ReplicateGuestData;
using HospitioApi.Core.HandleReplicateDateForGuestApp.Commands.ReplicateGuestData;
using HospitioApi.Core.HandleTicket.Commands.UpdateTicketPriority;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleReplicateDataForGuestApp.Commands.ReplicateDataHandlerFixure;
namespace HospitioApi.Test.HandleReplicateDataForGuestApp.Commands
{
    public class ReplicateDataHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public ReplicateDataHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();
            var _common = A.Fake<ICommonDataBaseOprationService>();

            A.CallTo(()=> _common.GetOldGuestAppBuilderData(_fix.In.NewBuilderId, A<CancellationToken>.Ignored, _dapper)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.oldReplicateDataModelResponse);

            A.CallTo(() => _common.GetNewGusestAppBuilderData(_fix.In.AppBuilderId, A<CancellationToken>.Ignored, _dapper)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.NewReplicateDataModelResponse);

            A.CallTo(() => _common.DeleteGuestAppBuilderData(_fix.oldReplicateDataModelResponse, db,A<CancellationToken>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CheckDeleteGuestAppBuilderData);

            A.CallTo(() => _common.AddGuestAppBuilderData(_fix.NewReplicateDataModelResponse, db,A<CancellationToken>.Ignored,_fix.In)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CheckAddGuestData);

            var result = await _fix.BuildHandler(_dapper,db,_common).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "ReplicateData SuccessFully..");
        }
        [Fact]
        public async Task Unexpected_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();
            var _common = A.Fake<ICommonDataBaseOprationService>();

            A.CallTo(() => _common.GetOldGuestAppBuilderData(_fix.In.NewBuilderId, A<CancellationToken>.Ignored, _dapper)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.oldReplicateDataModelResponse);

            A.CallTo(() => _common.GetNewGusestAppBuilderData(_fix.In.AppBuilderId, A<CancellationToken>.Ignored, _dapper)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.NewReplicateDataModelResponse);

            A.CallTo(() => _common.DeleteGuestAppBuilderData(_fix.oldReplicateDataModelResponse, db, A<CancellationToken>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CheckDeleteGuestAppBuilderData);

            var actualValue = _fix.CheckAddGuestData;
            _fix.CheckAddGuestData = false;
            A.CallTo(() => _common.AddGuestAppBuilderData(_fix.NewReplicateDataModelResponse, db, A<CancellationToken>.Ignored, _fix.In)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CheckAddGuestData);

            var result = await _fix.BuildHandler(_dapper, db, _common).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Unexpected Error..");
            _fix.CheckAddGuestData = actualValue;
        }
    }
    public class ReplicateDataHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public ReplicateDataIn In { get; set; } = new();
        public List<ReplicateDateModelForOldData> oldReplicateDataModelResponse { get; set; } = new();   
        public List<CustomerGuestAppBuildersOutId> NewReplicateDataModelResponse { get; set; } = new();   
        public bool CheckDeleteGuestAppBuilderData { get; set; } = true;
        public bool CheckAddGuestData { get; set; } = true;
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerRoomName1 = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
            var customerRoomName2 = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
            var customerAppBuilderOld = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName1.Id);
            var customerAppBuilderNew = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName2.Id);

            In.AppBuilderId = customerAppBuilderOld.Id;
            In.NewBuilderId = customerAppBuilderNew.Id;

            ReplicateDateModelForOldData replicateDateModelForOldData = new();
            replicateDateModelForOldData.Message = "Test";
            oldReplicateDataModelResponse.Add(replicateDateModelForOldData);
            CustomerGuestAppBuildersOutId customerGuestAppBuildersOutId = new();
            customerGuestAppBuildersOutId.Message = "NewTest";
            NewReplicateDataModelResponse.Add(customerGuestAppBuildersOutId);

        }
        public ReplicateDataHandler BuildHandler(IDapperRepository _dapper, ApplicationDbContext db, ICommonDataBaseOprationService commonDataBaseOprationService) =>
                new(_dapper, Response, db, commonDataBaseOprationService);
    }
}
