using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerRoomNamesRepository
{
    private readonly Faker<CustomerRoomName> _faker;
    public CustomerRoomNamesRepository()
    {
        _faker = new Faker<CustomerRoomName>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.Guid, f => f.Random.Guid())
            .RuleFor(m => m.GuidType, f => f.Random.Byte(1, 2))
            .RuleFor(m => m.Name, f => f.Name.FullName())
            .RuleFor(m => m.CreatedFrom, f => f.Random.Byte(1, 2));
    }

    public List<CustomerRoomName> SeedMany(ApplicationDbContext db, int customerId, int numberOfEntitiesToCreate)
    {
        var customerRoomNames = Generate(customerId, numberOfEntitiesToCreate);
        db.CustomerRoomNames.AddRange(customerRoomNames);
        db.SaveChanges();
        return customerRoomNames;
    }

    private List<CustomerRoomName> Generate(int customerId, int numberOfEntitiesCreate)
    {
        var faker = _faker.Clone()
                   .RuleFor(m => m.CustomerId, customerId);
        return faker.Generate(numberOfEntitiesCreate);
    }

    public CustomerRoomName SeedSingle(ApplicationDbContext db, int customerId)
    {
        var customerRoomNames = _faker.Generate();
        customerRoomNames.CustomerId = customerId;
        db.CustomerRoomNames.Add(customerRoomNames);
        db.SaveChanges();
        return customerRoomNames;
    }
}
