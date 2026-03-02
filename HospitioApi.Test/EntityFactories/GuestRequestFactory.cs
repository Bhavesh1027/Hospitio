using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;
using HospitioApi.Core.HandleGuestRequest.Commands.CreateGuestRequest;

namespace HospitioApi.Test.EntityFactories;

public class GuestRequestFactory
{
    private readonly Faker<GuestRequest> _faker;

    public GuestRequestFactory()
    {
        _faker = new Faker<GuestRequest>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(M => M.UpdateAt, f => f.Date.Recent());
    }

    public GuestRequest SeedSingle(ApplicationDbContext db, int? CustomerId)
    {
        var guestRequest = _faker.Generate();
        guestRequest.CustomerId = CustomerId;
        db.GuestRequests.Add(guestRequest);
        db.SaveChanges();
        return guestRequest;
    }
    public List<GuestRequest> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var guestRequests = _faker.Generate(numberOfEntitiesToCreate);
        db.GuestRequests.AddRange(guestRequests);
        db.SaveChanges();
        return guestRequests;
    }
    public GuestRequest Update(ApplicationDbContext db, GuestRequest guestRequest)
    {
        db.GuestRequests.Update(guestRequest);
        return guestRequest;
    }
}
