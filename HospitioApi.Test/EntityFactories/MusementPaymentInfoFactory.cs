using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class MusementPaymentInfoFactory
    {
        private readonly Faker<MusementPaymentInfo> _faker;
        public MusementPaymentInfoFactory()
        {
            _faker = new Faker<MusementPaymentInfo>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.OrderInfoId, f => f.Random.Int(1, 999))
                .RuleFor(m => m.PaymentMetod, f => f.PickRandom((byte?)1, (byte?)2))
                .RuleFor(m => m.PaymentStatus, f => f.PickRandom((byte?)1, (byte?)2, (byte?)3))
                .RuleFor(m => m.PaymentTransactionId, f => f.Random.Guid().ToString())
                .RuleFor(m => m.PlatForm, f => f.PickRandom((byte?)1, (byte?)2))
                .RuleFor(m => m.CustomerGuestId, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CustomerId, f => f.Random.Int(1, 999))
                .RuleFor(m => m.OrderUUID, f => f.Random.Guid().ToString())
                .RuleFor(m => m.Amount, f => f.Random.Decimal(50, 200))
                .RuleFor(m => m.Currency, f => f.Finance.Currency().Code)
                .RuleFor(m => m.Description, f => f.Lorem.Sentence());
        }
        public MusementPaymentInfo SeedSingle(ApplicationDbContext db)
        {
            var musementPaymentInfo = _faker.Generate();
            db.MusementPaymentInfos.Add(musementPaymentInfo);
            db.SaveChanges();
            return musementPaymentInfo;
        }
    }
}
