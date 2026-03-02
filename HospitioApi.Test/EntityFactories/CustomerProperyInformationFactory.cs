using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerProperyInformationFactory
{
    private readonly Faker<CustomerPropertyInformation> _faker;
    public CustomerProperyInformationFactory()
    {
        _faker = new Faker<CustomerPropertyInformation>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.WifiUsername, f => f.Lorem.Word())
            .RuleFor(m => m.WifiPassword, f => f.Lorem.Word())
            .RuleFor(m => m.Overview, f => f.Lorem.Sentence())
            .RuleFor(m => m.CheckInPolicy, f => f.Lorem.Sentence())
            .RuleFor(m => m.TermsAndConditions, f => f.Lorem.Sentence())
            .RuleFor(m => m.Street, f => f.Address.StreetAddress())
            .RuleFor(m => m.StreetNumber, f => f.Lorem.Word())
            .RuleFor(m => m.City, f => f.Address.City())
            .RuleFor(m => m.Postalcode, f => f.Address.ZipCode())
            .RuleFor(m => m.Country, f => f.Address.Country())
            .RuleFor(m => m.IsActive, true);
    }

    public CustomerPropertyInformation SeedSingle(ApplicationDbContext db, int? customerId=null, int? guestAppBuilderId = null)
    {
        var customerProperty = _faker.Generate();
        customerProperty.CustomerId = customerId;
        customerProperty.CustomerGuestAppBuilderId = guestAppBuilderId;
        db.CustomerPropertyInformations.Add(customerProperty);
        db.SaveChanges();
        return customerProperty;
    }

    public List<CustomerPropertyInformation> SeedMany(ApplicationDbContext db, int customerId,int guestAppBuilderId, int numberOfEntitiesToCreate)
    {
        var propertyInfos = Generate(customerId, guestAppBuilderId, numberOfEntitiesToCreate);
        db.CustomerPropertyInformations.AddRange(propertyInfos);
        db.SaveChanges();
        return propertyInfos;
    }

    private List<CustomerPropertyInformation> Generate(int customerId, int guestAppBuilderId, int numberOfEntitiesCreate)
    {
        var faker = _faker.Clone()
                   .RuleFor(m => m.CustomerId, customerId)
                   .RuleFor(m => m.CustomerGuestAppBuilderId, guestAppBuilderId);
        return faker.Generate(numberOfEntitiesCreate);
    }

    public CustomerPropertyInformation Update(ApplicationDbContext db, CustomerPropertyInformation propertyInfo)
    {
        db.CustomerPropertyInformations.Update(propertyInfo);
        db.SaveChanges();
        return propertyInfo;
    }

}
