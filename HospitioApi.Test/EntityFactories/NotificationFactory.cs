using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class NotificationFactory
    {
        private readonly Faker<Notification> _faker;
        public NotificationFactory()
        {
            _faker = new Faker<Notification>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true)
                .RuleFor(m => m.Country, f => f.Address.Country())
                .RuleFor(m => m.City, f => f.Address.City())
                .RuleFor(m => m.Postalcode, f => f.Address.ZipCode())
                .RuleFor(m => m.BusinessTypeId, f => f.Random.Int(1, 999))
                .RuleFor(m => m.ProductId, f => f.Random.Int(1, 999))
                .RuleFor(m => m.Title, f => f.Lorem.Word())
                .RuleFor(m => m.Message, f => f.Lorem.Sentence());

        }

        public Notification SeedSingle(ApplicationDbContext db, int businessTypeId, int productId)
        {
            var notification = _faker.Generate();
            notification.BusinessTypeId = businessTypeId;
            notification.ProductId = productId;
            db.Notifications.Add(notification);
            db.SaveChanges();
            return notification;
        }

        public List<Notification> SeedMany(ApplicationDbContext db, int businessTypeId, int productId, int numberOfEntitiesToCreate)
        {
            var notifications = Generate(businessTypeId, productId, numberOfEntitiesToCreate);
            db.Notifications.AddRange(notifications);
            db.SaveChanges();
            return notifications;
        }

        private List<Notification> Generate(int businessTypeId, int productId, int numberOfEntitiesCreate)
        {
            var faker = _faker.Clone()
                       .RuleFor(m => m.BusinessTypeId, businessTypeId)
                       .RuleFor(m => m.ProductId, productId);
            return faker.Generate(numberOfEntitiesCreate);
        }
    }
}
