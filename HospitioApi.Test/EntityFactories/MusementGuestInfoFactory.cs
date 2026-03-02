using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class MusementGuestInfoFactory
    {
        private readonly Faker<MusementGuestInfo> _faker;
        public MusementGuestInfoFactory()
        {
            _faker = new Faker<MusementGuestInfo>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                //.RuleFor(m => m.CustomerGuestId, f => f.Random.Int(1, 999))
                .RuleFor(m => m.FirstName, f => f.Name.FirstName())
                .RuleFor(m => m.LastName, f => f.Name.LastName())
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.PhoneNumber, f => f.Phone.PhoneNumber())
                //.RuleFor(m => m.MusementCustomerId, f => f.Random.Long(1, 1000))
                .RuleFor(m => m.CartUUID, f => f.Random.Guid().ToString());

        }
        public MusementGuestInfo SeedSingle(ApplicationDbContext db,int guestId = 0,int customerId = 0)
        {
           var musementGuestInfo =  _faker.Generate();
            if(guestId != 0)
            {
                musementGuestInfo.CustomerGuestId = guestId;
                musementGuestInfo.MusementCustomerId = customerId;
            }
            db.MusementGuestInfos.Add(musementGuestInfo);
            db.SaveChanges();
            return musementGuestInfo;
        }
    }
}
