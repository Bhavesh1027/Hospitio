using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class MusementOrderInfoFactory
    {
        private readonly Faker<MusementOrderInfo> _faker;
        public MusementOrderInfoFactory()
        {
            _faker = new Faker<MusementOrderInfo>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                //.RuleFor(m => m.MusementGuestInfoId, f => f.Random.Int(1, 999))
                .RuleFor(m => m.OrderUUID, f => f.Random.Guid().ToString())
                .RuleFor(m => m.Identifier, f => f.Random.AlphaNumeric(8).ToUpper())
                .RuleFor(m => m.Currency, f => f.Finance.Currency().Code)
                .RuleFor(m => m.TotalPrice, f => f.Random.Decimal(50, 200))
                .RuleFor(m => m.DiscountAmount, f => f.Random.Decimal(1, 10))
                .RuleFor(m => m.PaymentJson, f => f.Lorem.Sentence())
                .RuleFor(m => m.CartUUID, f => f.Random.Guid().ToString());
        }
        public MusementOrderInfo SeedSingle(ApplicationDbContext db,int musementGuestInfoId = 0)
        {
            var musementOrderInfo = _faker.Generate();
            if(musementGuestInfoId != 0)
            {
                musementOrderInfo.MusementGuestInfoId = musementGuestInfoId;
            }
            db.MusementOrderInfos.Add(musementOrderInfo);
            db.SaveChanges();
            return musementOrderInfo;
        }
    }
}
