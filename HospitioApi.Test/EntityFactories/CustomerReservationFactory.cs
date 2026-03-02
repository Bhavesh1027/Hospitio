using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerReservationFactory
{
    private readonly Faker<CustomerReservation> _faker;
    public CustomerReservationFactory()
    {
        _faker = new Faker<CustomerReservation>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.Uuid, f => f.Random.Uuid().ToString())
            .RuleFor(m => m.ReservationNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(m => m.Source, f => f.Random.Word())
            .RuleFor(m => m.NoOfGuestAdults, f => f.Random.Int(1, 5))
            .RuleFor(m => m.NoOfGuestChildrens, f => f.Random.Int(0, 3))
            .RuleFor(m => m.CheckinDate, f => f.Date.Future())
            .RuleFor(m => m.CheckoutDate, (f, m) => m.CheckinDate.HasValue ? m.CheckinDate.Value.AddDays(f.Random.Int(1, 7)) : (DateTime?)null);
    }

    public CustomerReservation SeedSingle(ApplicationDbContext db, int CustomerId)
    {
        var customerReservation = _faker.Generate();
        customerReservation.CustomerId = CustomerId;
        db.CustomerReservations.Add(customerReservation);
        db.SaveChanges();
        return customerReservation;
    }
    public List<CustomerReservation> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var reservations = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerReservations.AddRange(reservations);
        db.SaveChanges();
        return reservations;
    }
    public CustomerReservation Update(ApplicationDbContext db, CustomerReservation customerReservation)
    {
        db.CustomerReservations.Update(customerReservation);
        return customerReservation;
    }
}
