using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class CustomerUsersPermissionFactory
    {
        public readonly Faker<CustomerUsersPermission> _faker;

        public CustomerUsersPermissionFactory()
        {
            _faker = new Faker<CustomerUsersPermission>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsView, f => f.Random.Bool())
                .RuleFor(m => m.IsEdit, f => f.Random.Bool())
                .RuleFor(m => m.IsUpload, f => f.Random.Bool())
                .RuleFor(m => m.IsReply, f => f.Random.Bool())
                .RuleFor(m => m.IsDownload, f => f.Random.Bool());
        }
        public CustomerUsersPermission SeedSingle(ApplicationDbContext db,int? customerUserId,int? customerPermissionId = 0)
        {
            var CustomerUsersPermission = _faker.Generate();
            db.CustomerUsersPermissions.Add(CustomerUsersPermission);
            db.SaveChanges();
            return CustomerUsersPermission;
        }
        public List<CustomerUsersPermission> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate, int? userId = 0)
        {
            var CustomerUsersPermissions = _faker.Generate(numberOfEntitiesToCreate);
            db.CustomerUsersPermissions.AddRange(CustomerUsersPermissions);
            db.SaveChanges();
            return CustomerUsersPermissions;
        }
    }
}
