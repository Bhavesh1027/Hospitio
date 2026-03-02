using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class TicketFactory
{
    private readonly Faker<Ticket> _faker;
    public TicketFactory()
    {
        _faker = new Faker<Ticket>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public Ticket SeedSingle(ApplicationDbContext db)
    {
        var ticket = _faker.Generate();
        db.Tickets.Add(ticket);
        db.SaveChanges();
        return ticket;
    }

    public List<Ticket> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var tickets = _faker.Generate(numberOfEntitiesToCreate);
        db.Tickets.AddRange(tickets);
        db.SaveChanges();
        return tickets;
    }
}
