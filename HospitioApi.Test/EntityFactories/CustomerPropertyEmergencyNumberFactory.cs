using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class CustomerPropertyEmergencyNumberFactory
    {
        private readonly Faker<CustomerPropertyEmergencyNumber> _faker;
        public CustomerPropertyEmergencyNumberFactory()
        {
            _faker = new Faker<CustomerPropertyEmergencyNumber>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true);
        }

        public CustomerPropertyEmergencyNumber SeedSingle(ApplicationDbContext db, int propertyInformationId)
        {
            var propertyEmergencyNumber = _faker.Generate();
            propertyEmergencyNumber.CustomerPropertyInformationId = propertyInformationId;
            db.CustomerPropertyEmergencyNumbers.Add(propertyEmergencyNumber);
            db.TestCaseSaveChanges();
            return propertyEmergencyNumber;
        }

        public List<CustomerPropertyEmergencyNumber> SeedMany(ApplicationDbContext db, int propertyInformationId, int numberOfEntitiesToCreate)
        {
            var propertyEmergencyNumbers = Generate(propertyInformationId, numberOfEntitiesToCreate);
            db.CustomerPropertyEmergencyNumbers.AddRange(propertyEmergencyNumbers);
            db.SaveChanges();
            return propertyEmergencyNumbers;
        }

        private List<CustomerPropertyEmergencyNumber> Generate(int propertyInformationId, int numberOfEntitiesCreate)
        {
            var faker = _faker.Clone()
                       .RuleFor(m => m.CustomerPropertyInformationId, propertyInformationId);
            return faker.Generate(numberOfEntitiesCreate);
        }
    }
}
