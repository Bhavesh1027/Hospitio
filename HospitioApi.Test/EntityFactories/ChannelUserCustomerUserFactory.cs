using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class ChannelUserCustomerUserFactory
    {
        public readonly Faker<ChannelUserCustomerUser> _faker;
        public ChannelUserCustomerUserFactory()
        {
            _faker = new Faker<ChannelUserCustomerUser>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.Uuid, f => f.Random.Uuid().ToString());
        }
        public ChannelUserCustomerUser SeedSingle(ApplicationDbContext db,int userId)
        {
            var ChannelUserCustomerUser = _faker.Generate();
            ChannelUserCustomerUser.channelUserID = userId;
            db.Channels.Add(ChannelUserCustomerUser);
            db.SaveChanges();
            return ChannelUserCustomerUser;
        }
    }
}
