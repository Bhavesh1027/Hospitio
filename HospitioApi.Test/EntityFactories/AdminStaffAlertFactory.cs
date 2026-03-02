using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class AdminStaffAlertFactory
    {
        public readonly Faker<AdminStaffAlert> _faker;

        public AdminStaffAlertFactory()
        {
            _faker = new Faker<AdminStaffAlert>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.Name, f => f.Random.String2(1, 50))
                .RuleFor(m => m.Platfrom, f => f.Random.String2(1, 10))
                .RuleFor(m => m.PhoneCountry, f => f.Random.String2(1, 3))
                .RuleFor(m => m.PhoneNumber, f => f.Random.String2(1, 20))
                .RuleFor(m => m.WaitTimeInMintes, f => f.Random.Int(1, 60))
                .RuleFor(m => m.Msg, f => f.Random.Words())
                .RuleFor(m => m.UserId, f => f.Random.Int(1, 1000));
        }
        public AdminStaffAlert SeedSingle(ApplicationDbContext db,int? userId = 0,string? name = null)
        {
            var adminStaffAlert = _faker.Generate();
            if(userId != 0)
            {
                adminStaffAlert.UserId = userId;
            }
            if(name != null)
            {
                adminStaffAlert.Name = name;
            }
            db.AdminStaffAlerts.Add(adminStaffAlert);
            db.SaveChanges();
            return adminStaffAlert;
        }
        public List<AdminStaffAlert> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate, int? userId = 0)
        {
            var adminStaffAlerts = _faker.Generate(numberOfEntitiesToCreate);
            foreach(var item in adminStaffAlerts)
            {
                item.UserId = userId;
            }
            db.AdminStaffAlerts.AddRange(adminStaffAlerts);
            db.SaveChanges();
            return adminStaffAlerts;
        }
        public bool Remove(ApplicationDbContext db,AdminStaffAlert adminStaffAlert)
        {
           
                db.AdminStaffAlerts.Remove(adminStaffAlert);
                db.SaveChanges();
            
            return true;
        }

    }
}
