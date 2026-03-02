using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class CustomerLevelFactory
    {
        private readonly Faker<CustomerLevel> _faker;
        public CustomerLevelFactory() 
        {
            _faker = new Faker<CustomerLevel>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 5))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.LevelName, f => f.Lorem.Word())
            .RuleFor(m => m.NormalizedLevelName, f => f.Lorem.Word())
            .RuleFor(m => m.IsCustomerUserType, f => f.Random.Bool());
        }

        public List<CustomerLevel> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
        {
            var customerLevel = _faker.Generate(numberOfEntitiesToCreate);
            db.CustomerLevels.AddRange(customerLevel);
            db.SaveChanges();
            return customerLevel;
        }

        public CustomerLevel SeedSingle(ApplicationDbContext db,int? id = null)
        {
            var customerLevel = _faker.Generate();

            if (id != null)
            {
                customerLevel.Id = id ?? 0;
            }
            
            db.CustomerLevels.Add(customerLevel);
            db.SaveChanges();
            return customerLevel;
        }
    }
}
