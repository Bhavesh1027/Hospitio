using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerReceptionCategoryFactory
{
    private readonly Faker<CustomerGuestAppReceptionCategory> _faker;
    public CustomerReceptionCategoryFactory()
    {
        _faker = new Faker<CustomerGuestAppReceptionCategory>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }
    public CustomerGuestAppReceptionCategory SeedSingle(ApplicationDbContext db, int? CustomerId, int? appbuilderId)
    {
        var receptionCategory = _faker.Generate();
        receptionCategory.CustomerId = CustomerId;
        receptionCategory.CustomerGuestAppBuilderId = appbuilderId;
        db.CustomerGuestAppReceptionCategories.Add(receptionCategory);
        db.SaveChanges();
        return receptionCategory;
    }
    public List<CustomerGuestAppReceptionCategory> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuestAppReceptionCategories = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppReceptionCategories.AddRange(customerGuestAppReceptionCategories);
        db.SaveChanges();
        return customerGuestAppReceptionCategories;
    }
}
