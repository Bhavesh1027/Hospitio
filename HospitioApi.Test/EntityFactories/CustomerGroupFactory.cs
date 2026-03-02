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
    public class CustomerGroupFactory
    {
        public readonly Faker<CustomerGroup> _faker;
        public CustomerGroupFactory()
        {
            _faker = new Faker<CustomerGroup>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.Name, f => f.Lorem.Word());
        }
        public CustomerGroup SeedSingle(ApplicationDbContext db,int? departmentId= 0,int? customerId = 0)
        {
            var customerGroup = _faker.Generate();
            customerGroup.CustomerId = customerId != 0 ? customerId : 0;
            customerGroup.DepartmentId = departmentId != 0 ? departmentId : 0;
            db.CustomerGroups.Add(customerGroup);
            db.SaveChanges();
            return customerGroup;
        }
        public List<CustomerGroup> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
        {
            var custmerGroups = _faker.Generate(numberOfEntitiesToCreate);
            db.CustomerGroups.AddRange(custmerGroups);
            db.SaveChanges();
            return custmerGroups;
        }
    }
}
