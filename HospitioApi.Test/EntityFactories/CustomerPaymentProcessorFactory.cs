using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;


namespace HospitioApi.Test.EntityFactories;
public class CustomerPaymentProcessorFactory
{
    private readonly Faker<CustomerPaymentProcessor> _faker;
    public CustomerPaymentProcessorFactory()
    {
        _faker = new Faker<CustomerPaymentProcessor>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.IsActive, true);
            //.RuleFor(m => m.ClientId, f => f.Random.Guid().ToString().ToLower())
            //.RuleFor(m => m.ClientSecret, f => f.Random.Guid().ToString().ToLower())
            //.RuleFor(m => m.Currency, f => f.Random.Guid().ToString().ToLower());
    }

    public CustomerPaymentProcessor SeedSingle(ApplicationDbContext db,int customerId,int paymentProcessorId)
    {
        var customerPaymentProcessor = _faker.Generate();
        customerPaymentProcessor.Id = customerId;
        customerPaymentProcessor.PaymentProcessorId = paymentProcessorId;
        db.CustomerPaymentProcessors.Add(customerPaymentProcessor);
        db.SaveChanges();
        return customerPaymentProcessor;
    }
}
