using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class CustomerPropertyGalleryFactory
    {
        private readonly Faker<CustomerPropertyGallery> _faker;
        public CustomerPropertyGalleryFactory()
        {
            _faker = new Faker<CustomerPropertyGallery>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m=>m.PropertyImage,f=>f.Random.String())
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true);
        }

        public CustomerPropertyGallery SeedSingle(ApplicationDbContext db, int propertyInformationId,bool? IsPropertyImageNull = null)
        {
            var propertyGalley = _faker.Generate();
            if (IsPropertyImageNull != null && IsPropertyImageNull == true)
            {
                propertyGalley.PropertyImage = null;
            }
            propertyGalley.CustomerPropertyInformationId = propertyInformationId;
            db.CustomerPropertyGalleries.Add(propertyGalley);
            db.TestCaseSaveChanges();
            return propertyGalley;
        }

        public List<CustomerPropertyGallery> SeedMany(ApplicationDbContext db, int propertyInformationId, int numberOfEntitiesToCreate)
        {
            var propertyGalley = Generate(propertyInformationId, numberOfEntitiesToCreate);
            db.CustomerPropertyGalleries.AddRange(propertyGalley);
            db.SaveChanges();
            return propertyGalley;
        }

        private List<CustomerPropertyGallery> Generate(int propertyInformationId, int numberOfEntitiesCreate)
        {
            var faker = _faker.Clone()
                       .RuleFor(m => m.CustomerPropertyInformationId, propertyInformationId);
            return faker.Generate(numberOfEntitiesCreate);
        }

    }
}
