using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestFactory
{
    private readonly Faker<CustomerGuest> _faker;
    public CustomerGuestFactory()
    {
        _faker = new Faker<CustomerGuest>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.DateOfBirth, f => f.Date.Past())
            .RuleFor(m => m.Firstname, f => f.Person.FirstName)
            .RuleFor(m => m.Lastname, f => f.Person.LastName)
            .RuleFor(m => m.Email, f => f.Person.Email)
            .RuleFor(m => m.Picture, f => f.Internet.Avatar())
            .RuleFor(m => m.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(m => m.Country, f => f.Address.CountryCode());
    }

    public CustomerGuest SeedSingle(ApplicationDbContext db, int? customerReservationId,string roomname = null)
    {
        var customerGuest = _faker.Generate();
        customerGuest.CustomerReservationId = customerReservationId;
        if(roomname != null)
        {
            customerGuest.RoomNumber = roomname;
        }
        db.CustomerGuests.Add(customerGuest);
        db.SaveChanges();
        return customerGuest;
    }

    public List<CustomerGuest> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuests = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuests.AddRange(customerGuests);
        db.SaveChanges();
        return customerGuests;
    }

    public CustomerGuest Remove(ApplicationDbContext db, CustomerGuest customerGuest)
    {
        db.CustomerGuests.Remove(customerGuest);
        return customerGuest;
    }
    public CustomerGuest Update(ApplicationDbContext db, CustomerGuest customerGuest)
    {
        db.CustomerGuests.Update(customerGuest);
        return customerGuest;
    }
}
