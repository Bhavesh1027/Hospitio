using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class TicketCategoryFactory
{
    private readonly Faker<TicketCategory> _faker;
    public TicketCategoryFactory()
    {
        _faker = new Faker<TicketCategory>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CategoryName, "Test")
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public TicketCategory SeedSingle(ApplicationDbContext db)
    {
        var ticketCategory = _faker.Generate();
        db.TicketCategorys.Add(ticketCategory);
        db.SaveChanges();
        return ticketCategory;
    }

    public List<TicketCategory> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var ticketCategories = _faker.Generate(numberOfEntitiesToCreate);
        db.TicketCategorys.AddRange(ticketCategories);
        db.SaveChanges();
        return ticketCategories;
    }
}
