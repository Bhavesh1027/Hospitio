using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestAppEnhanceYourStayItemFactory
{
    private readonly Faker<CustomerGuestAppEnhanceYourStayItem> _faker;
    public CustomerGuestAppEnhanceYourStayItemFactory()
    {
        _faker = new Faker<CustomerGuestAppEnhanceYourStayItem>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerGuestAppEnhanceYourStayItem SeedSingle(ApplicationDbContext db, int? CustomerGuestAppBuilderId, int? CustomerId, int? CustomerGuestAppBuilderCategoryId)
    {
        var customerGuest = _faker.Generate();
        customerGuest.CustomerGuestAppBuilderId = CustomerGuestAppBuilderId;
        customerGuest.CustomerId = CustomerId;
        customerGuest.CustomerGuestAppBuilderCategoryId = CustomerGuestAppBuilderCategoryId;
        db.CustomerGuestAppEnhanceYourStayItems.Add(customerGuest);
        db.SaveChanges();
        return customerGuest;
    }

    public List<CustomerGuestAppEnhanceYourStayItem> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuests = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppEnhanceYourStayItems.AddRange(customerGuests);
        db.SaveChanges();
        return customerGuests;
    }
}
