using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Test.EntityFactories
{
    public class PaymentProcessorsDefinationsFactory
    {
        private readonly Faker<PaymentProcessorsDefinations> _faker;
        public PaymentProcessorsDefinationsFactory()
        {
            _faker = new Faker<PaymentProcessorsDefinations>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true)
                .RuleFor(m => m.GRFields, f => f.Random.Bool() ? f.Lorem.Sentence() : null)
                .RuleFor(m => m.GRSupportedCountries, f => f.Random.Bool() ? f.Address.Country() : null);
        }
        public PaymentProcessorsDefinations SeedSingle(ApplicationDbContext db,int paymentProcessorId)
        {
            var paymentProcessorDefinition = _faker.Generate();
            paymentProcessorDefinition.PaymentProcessorId = paymentProcessorId;
            db.PaymentProcessorsDefinations.Add(paymentProcessorDefinition);
            db.SaveChanges();
            return paymentProcessorDefinition;
        }

    }
}
