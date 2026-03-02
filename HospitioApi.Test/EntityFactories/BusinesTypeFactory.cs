using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class BusinessTypeFactory
{
    private readonly Faker<BusinessType> _faker;
    public BusinessTypeFactory()
    {
        _faker = new Faker<BusinessType>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public BusinessType SeedSingle(ApplicationDbContext db)
    {
        var businessType = _faker.Generate();
        db.BusinessTypes.Add(businessType);
        db.SaveChanges();
        return businessType;
    }

    public List<BusinessType> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var businessTypes = _faker.Generate(numberOfEntitiesToCreate);
        db.BusinessTypes.AddRange(businessTypes);
        db.SaveChanges();
        return businessTypes;
    }

    public BusinessType Remove(ApplicationDbContext db, BusinessType businessType)
    {
        db.BusinessTypes.Remove(businessType);
        return businessType;
    }
    public BusinessType Update(ApplicationDbContext db, BusinessType businessType)
    {
        db.BusinessTypes.Update(businessType);
        return businessType;
    }
}
