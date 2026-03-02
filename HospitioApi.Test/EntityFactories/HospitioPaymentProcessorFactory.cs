using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class HospitioPaymentProcessorFactory
    {
        private readonly Faker<HospitioPaymentProcessor> _faker;
        public HospitioPaymentProcessorFactory()
        {
            _faker = new Faker<HospitioPaymentProcessor>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true);
        //        .RuleFor(m => m.ClientId, f => f.Random.Guid().ToString().ToLower())
        //        .RuleFor(m => m.ClientSecret, f => f.Random.Guid().ToString().ToLower())
        //        .RuleFor(m => m.Currency, f => f.Random.Guid().ToString().ToLower());
        }

        public HospitioPaymentProcessor SeedSingle(ApplicationDbContext db, int paymentProcessorId)
        {
            var hospitioPaymentProcessor = _faker.Generate();
            hospitioPaymentProcessor.PaymentProcessorId = paymentProcessorId;
            db.HospitioPaymentProcessors.Add(hospitioPaymentProcessor);
            db.SaveChanges();
            return hospitioPaymentProcessor;
        }

        public List<HospitioPaymentProcessor> SeedMany(ApplicationDbContext db, int paymentProcessorId, int numberOfEntitiesToCreate)
        {
            var hospitioPaymentProcessor = Generate(paymentProcessorId, numberOfEntitiesToCreate);
            db.HospitioPaymentProcessors.AddRange(hospitioPaymentProcessor);
            db.SaveChanges();
            return hospitioPaymentProcessor;
        }

        private List<HospitioPaymentProcessor> Generate(int paymentProcessorId, int numberOfEntitiesCreate)
        {
            var faker = _faker.Clone()
                       .RuleFor(m => m.PaymentProcessorId, paymentProcessorId);
            return faker.Generate(numberOfEntitiesCreate);
        }
    }
}
