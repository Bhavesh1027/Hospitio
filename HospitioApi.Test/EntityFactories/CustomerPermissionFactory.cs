using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class CustomerPermissionFactory
    {
        public readonly Faker<CustomerPermission> _faker;

        public CustomerPermissionFactory()
        {
            _faker = new Faker<CustomerPermission>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.Name, f => f.Random.String2(1, 50))
                .RuleFor(m => m.Name, f => f.Person.FirstName)
                .RuleFor(m => m.NormalizedName, f => f.Lorem.Word())
                .RuleFor(m => m.IsView, f => f.Random.Bool())
                .RuleFor(m => m.IsEdit, f => f.Random.Bool())
                .RuleFor(m => m.IsUpload, f => f.Random.Bool())
                .RuleFor(m => m.IsReply, f => f.Random.Bool())
                .RuleFor(m => m.IsDownload, f => f.Random.Bool());
        }
        public CustomerPermission SeedSingle(ApplicationDbContext db)
        {
            var CustomerPermission = _faker.Generate();
            db.CustomerPermissions.Add(CustomerPermission);
            db.SaveChanges();
            return CustomerPermission;
        }
        public List<CustomerPermission> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
        {
            var customerPermissions = _faker.Generate(numberOfEntitiesToCreate);
            db.CustomerPermissions.AddRange(customerPermissions);
            db.SaveChanges();
            return customerPermissions;
        }
    }
}
