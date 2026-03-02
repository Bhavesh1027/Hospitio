using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class VonageCredentialFactory
    {
        private readonly Faker<VonageCredentials> _faker;
        public VonageCredentialFactory() 
        {
            _faker = new Faker<VonageCredentials>()
            .RuleFor(m => m.SubAccountName, f => f.Person.UserName)
            .RuleFor(m => m.APIKey, f => f.Random.AlphaNumeric(20))
            .RuleFor(m => m.APISecret, f => f.Random.AlphaNumeric(30))
            .RuleFor(m => m.AppId, f => f.Random.AlphaNumeric(10))
            .RuleFor(m => m.AppPrivatKey, f => f.Random.AlphaNumeric(40))
            .RuleFor(m => m.AppPublicKey, f => f.Random.AlphaNumeric(40))
            .RuleFor(m => m.WABAId, f => f.Random.AlphaNumeric(15));
        }
        public VonageCredentials SeedSingle(ApplicationDbContext db, int CustomerId,bool? IsNull = false)
        {
            var vonageCredentials = _faker.Generate();
            if((bool)IsNull)
            {
                vonageCredentials.WABAId = null;
            }
            vonageCredentials.CustomerId = CustomerId;
            db.VonageCredentials.Add(vonageCredentials); 
            db.SaveChanges();
            return vonageCredentials;
        }
    }
}
