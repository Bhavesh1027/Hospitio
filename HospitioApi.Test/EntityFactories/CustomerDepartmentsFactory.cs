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
    public class CustomerDepartmentsFactory
    {
        public readonly Faker<CustomerDepartment> _faker;
        public CustomerDepartmentsFactory()
        {
            _faker = new Faker<CustomerDepartment>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.Name, f => f.Random.String2(1, 50));
        }
        public CustomerDepartment SeedSingle(ApplicationDbContext db,int customerId,int? departmentManagerId = 0)
        {
            var customerDepartment = _faker.Generate();
            customerDepartment.CustomerId = customerId;
            if(departmentManagerId != 0)
            {
                customerDepartment.DepartmentMangerId = departmentManagerId;
            }
            db.CustomerDepartments.Add(customerDepartment);
            db.SaveChanges();
            return customerDepartment;
        }
    }
}
