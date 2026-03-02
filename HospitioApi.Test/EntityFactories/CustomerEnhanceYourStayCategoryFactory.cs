using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerEnhanceYourStayCategoryFactory
{
    private readonly Faker<CustomerGuestAppEnhanceYourStayCategory> _faker;
    public CustomerEnhanceYourStayCategoryFactory()
    {
        _faker = new Faker<CustomerGuestAppEnhanceYourStayCategory>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.CategoryName, f => f.Lorem.Word());
    }

    public CustomerGuestAppEnhanceYourStayCategory SeedSingle(ApplicationDbContext db, int? CustomerGuestAppBuilderId, int? CustomerId)
    {
        var customerStayCategory = _faker.Generate();
        customerStayCategory.CustomerGuestAppBuilderId = CustomerGuestAppBuilderId;
        customerStayCategory.CustomerId = CustomerId;
        db.CustomerGuestAppEnhanceYourStayCategories.Add(customerStayCategory);
        db.SaveChanges();
        return customerStayCategory;
    }

    public List<CustomerGuestAppEnhanceYourStayCategory> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerStayCategories = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppEnhanceYourStayCategories.AddRange(customerStayCategories);
        db.SaveChanges();
        return customerStayCategories;
    }
}
