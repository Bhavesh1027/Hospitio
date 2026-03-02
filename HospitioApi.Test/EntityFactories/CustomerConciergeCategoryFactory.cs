using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerConciergeCategoryFactory
{
    private readonly Faker<CustomerGuestAppConciergeCategory> _faker;
    public CustomerConciergeCategoryFactory()
    {
        _faker = new Faker<CustomerGuestAppConciergeCategory>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }
    public CustomerGuestAppConciergeCategory SeedSingle(ApplicationDbContext db, int? CustomerId, int? appbuilderId)
    {
        var conciergeCategory = _faker.Generate();
        conciergeCategory.CustomerId = CustomerId;
        conciergeCategory.CustomerGuestAppBuilderId = appbuilderId;
        db.CustomerGuestAppConciergeCategories.Add(conciergeCategory);
        db.SaveChanges();
        return conciergeCategory;
    }
    public List<CustomerGuestAppConciergeCategory> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuestAppConcierges = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppConciergeCategories.AddRange(customerGuestAppConcierges);
        db.SaveChanges();
        return customerGuestAppConcierges;
    }
}
