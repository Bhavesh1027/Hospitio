using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Test.EntityFactories
{
    public class LeadsFactory
    {
        private readonly Faker<Lead> _faker;
        public LeadsFactory()
        {
            _faker = new Faker<Lead>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true)
                .RuleFor(m => m.FirstName, f => f.Name.FirstName())
                .RuleFor(m => m.LastName, f => f.Name.LastName())
                .RuleFor(m => m.Company, f => f.Company.CompanyName())
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.Comment, f => f.Lorem.Sentence())
                .RuleFor(m => m.PhoneCountry, f => f.Phone.PhoneNumberFormat())
                .RuleFor(m => m.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(m => m.ContactFor, f => f.Random.Int(1, 1000));

        }

        public Lead SeedSingle(ApplicationDbContext db, int? contactFor)
        {
            var lead = _faker.Generate();
            lead.ContactFor = contactFor;
            db.Leads.Add(lead);
            db.SaveChanges();
            return lead;
        }

        public List<Lead> SeedMany(ApplicationDbContext db, int userId,int numberOfEntitiesToCreate)
        {
            var leads = Generate(userId, numberOfEntitiesToCreate);
            db.Leads.AddRange(leads);
            db.SaveChanges();
            return leads;
        }

        private List<Lead> Generate(int userId,int numberOfEntitiesCreate)
        {
            var faker = _faker.Clone()
                       .RuleFor(m => m.ContactFor, userId);
            return faker.Generate(numberOfEntitiesCreate);
        }
    }
}
