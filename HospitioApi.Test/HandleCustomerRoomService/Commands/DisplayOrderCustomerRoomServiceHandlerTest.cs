using FakeItEasy;
using HospitioApi.Core.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomService;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Collections.Immutable;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomServiceHandlerFixure;
namespace HospitioApi.Test.HandleCustomerRoomService.Commands
{
    public class DisplayOrderCustomerRoomServiceHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public DisplayOrderCustomerRoomServiceHandlerTest(ThisTestFixure fix)
        {
            _fix  = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Room-Service display order updated.");
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

            var actualData = _fix.In.DisplayOrderCustomerRoomService[0].Id;
            _fix.In.DisplayOrderCustomerRoomService[0].Id = 0;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Cusomers Room Service with Id {_fix.In.DisplayOrderCustomerRoomService[0].Id} could not be found.");
            _fix.In.DisplayOrderCustomerRoomService[0].Id = actualData;
        }
    }
    public class DisplayOrderCustomerRoomServiceHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DisplayOrderCustomerRoomServiceIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var roomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
            var appBuilderId = CustomerGuestAppBuilderFactory.SeedSingle(db, roomName.Id);
            var customerRoomServiceCategory = CustomerRoomServiceCategoryFactory.SeedSingle(db,customer.Id,appBuilderId.Id);

            DisplayOrderCustomerRoomService displayOrderCustomerRoomService = new();
            displayOrderCustomerRoomService.Id = customerRoomServiceCategory.Id;
            displayOrderCustomerRoomService.DisplayOrder = customerRoomServiceCategory.DisplayOrder;
            List<DisplayOrderCustomerRoomService> displayOrderCustomerRoomServices = new();
            displayOrderCustomerRoomServices.Add(displayOrderCustomerRoomService);
            In.DisplayOrderCustomerRoomService = displayOrderCustomerRoomServices;
        }
        public DisplayOrderCustomerRoomServiceHandler BuildHandler(ApplicationDbContext db) =>
           new(db, Response);
    }
    }
