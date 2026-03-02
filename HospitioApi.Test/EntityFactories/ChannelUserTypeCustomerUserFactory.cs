using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class ChannelUserTypeCustomerUserFactory
    {
        public readonly Faker<ChannelUserTypeCustomerUser> _faker;
        public ChannelUserTypeCustomerUserFactory()
        {
            _faker = new Faker<ChannelUserTypeCustomerUser>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
               
        }
        public ChannelUserTypeCustomerUser SeedSingle(ApplicationDbContext _db,int channelId,int userId)
        {
            var channelUserTypeCustomerUser = _faker.Generate();
            channelUserTypeCustomerUser.ChannelId = channelId;
            channelUserTypeCustomerUser.UserId = userId;
            _db.ChannelUserTypeCustomerUser.Add(channelUserTypeCustomerUser);
            _db.SaveChanges();
            return channelUserTypeCustomerUser;
        }
        public List<ChannelUserTypeCustomerUser> SeedMany(ApplicationDbContext _db,int numberOfEntitiesToCreate)
        {
            var channelUserTypeCustomerUsers = _faker.Generate(numberOfEntitiesToCreate);
            _db.ChannelUserTypeCustomerUser.AddRange(channelUserTypeCustomerUsers);
            _db.SaveChanges();
            return channelUserTypeCustomerUsers;
        }
    }
}
