using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;


namespace HospitioApi.Test.EntityFactories;
public class PaymentProcessorFactory
{
    private readonly Faker<PaymentProcessor> _faker;
    public PaymentProcessorFactory()
    {
        _faker = new Faker<PaymentProcessor>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.IsActive, true)
            .RuleFor(m => m.GRName, f => f.Random.Guid().ToString().ToLower());
    }

    public PaymentProcessor SeedSingle(ApplicationDbContext db)
    {
        var paymentProcessor = _faker.Generate();
        db.PaymentProcessors.Add(paymentProcessor);
        db.SaveChanges();
        return paymentProcessor;
    }

    public List<PaymentProcessor> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var paymentProcessors = _faker.Generate(numberOfEntitiesToCreate);
        db.PaymentProcessors.AddRange(paymentProcessors);
        db.SaveChanges();
        return paymentProcessors;
    }
}