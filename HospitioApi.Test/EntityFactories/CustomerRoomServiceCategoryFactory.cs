using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerRoomServiceCategoryFactory
{
    private readonly Faker<CustomerGuestAppRoomServiceCategory> _faker;
    public CustomerRoomServiceCategoryFactory()
    {
        _faker = new Faker<CustomerGuestAppRoomServiceCategory>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }
    public CustomerGuestAppRoomServiceCategory SeedSingle(ApplicationDbContext db, int? CustomerId, int? appbuilderId)
    {
        var roomServiceCategory = _faker.Generate();
        roomServiceCategory.CustomerId = CustomerId;
        roomServiceCategory.CustomerGuestAppBuilderId = appbuilderId;
        db.CustomerGuestAppRoomServiceCategories.Add(roomServiceCategory);
        db.SaveChanges();
        return roomServiceCategory;
    }
    public List<CustomerGuestAppRoomServiceCategory> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var roomServiceCategories = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppRoomServiceCategories.AddRange(roomServiceCategories);
        db.SaveChanges();
        return roomServiceCategories;
    }
}
