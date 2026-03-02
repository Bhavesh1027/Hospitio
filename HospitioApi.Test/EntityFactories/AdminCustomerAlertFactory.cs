using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class AdminCustomerAlertFactory
    {
        public readonly Faker<AdminCustomerAlert> _faker;
        public AdminCustomerAlertFactory()
        {
            _faker = new Faker<AdminCustomerAlert>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.Msg, f => f.Random.Words())
                .RuleFor(m => m.MsgWaitTimeInMinutes, f => f.Random.Int(1, 60));
        }
        public AdminCustomerAlert SeedSingle(ApplicationDbContext db,string Msg = null)
        {
            var adminCustomerAlert = _faker.Generate();
            if(Msg != null)
            {
                adminCustomerAlert.Msg = Msg;
            }
            db.AdminCustomerAlerts.Add(adminCustomerAlert);
            db.SaveChanges();
            return adminCustomerAlert;
        }
        public List<AdminCustomerAlert> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
        {
            var adminCustomerAlerts = _faker.Generate(numberOfEntitiesToCreate);
            db.AdminCustomerAlerts.AddRange(adminCustomerAlerts);
            db.SaveChanges();
            return adminCustomerAlerts;
        }

        public bool Remove(ApplicationDbContext db,AdminCustomerAlert adminCustomerAlert)
        {
            db.AdminCustomerAlerts.Remove(adminCustomerAlert);
            db.SaveChanges();
            return true;
        }
    }
}
