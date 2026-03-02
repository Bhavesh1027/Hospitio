using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class MusementItemInfoFactory
    {
        private readonly Faker<MusementItemInfo> _faker;
        public MusementItemInfoFactory()
        {
            _faker = new Faker<MusementItemInfo>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.ProductActivityId, f => f.Random.Guid().ToString())
                .RuleFor(m => m.Quantity, f => f.Random.Number(1, 10))
                .RuleFor(m => m.ProductOriginalRetailPrice, f => f.Random.Decimal(10, 100))
                .RuleFor(m => m.ItemUUID, f => f.Random.Guid().ToString())
                .RuleFor(m => m.ItemMusementProductId, f => f.Random.Guid().ToString())
                .RuleFor(m => m.CartItemUUID, f => f.Random.Guid().ToString())
                .RuleFor(m => m.TransactionCode, f => f.Random.AlphaNumeric(8).ToUpper())
                .RuleFor(m => m.Title, f => f.Commerce.ProductName())
                .RuleFor(m => m.PriceFeature, f => f.Commerce.ProductAdjective())
                .RuleFor(m => m.TicketHolder, f => f.Person.FullName)
                .RuleFor(m => m.Currency, f => f.Finance.Currency().Code)
                .RuleFor(m => m.ProductDiscountAmount, f => f.Random.Decimal(1, 10))
                .RuleFor(m => m.ProductServiceFee, f => f.Random.Decimal(1, 5))
                .RuleFor(m => m.TotalPrice, f => f.Random.Decimal(50, 200));
        }
        public MusementItemInfo SeedSingle(ApplicationDbContext db)
        {
            var musementItemInfo = _faker.Generate();
            db.MusementItemInfos.Add(musementItemInfo);
            db.SaveChanges();
            return musementItemInfo;
        }
    }
}
