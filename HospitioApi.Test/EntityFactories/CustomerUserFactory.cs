using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;


namespace HospitioApi.Test.EntityFactories;
public class CustomerUserFactory
{
    private readonly Faker<CustomerUser> _faker;
    public CustomerUserFactory()
    {
        _faker = new Faker<CustomerUser>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.IsActive, true)
            .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.FirstName, f => f.Random.Guid().ToString().ToLower())
            .RuleFor(m => m.LastName, f => f.Random.Guid().ToString().ToLower())
            .RuleFor(m => m.UserName, f => f.Internet.UserName());
    }
    public CustomerUser SeedSingle(ApplicationDbContext db)
    {
        var customerUser = _faker.Generate();
        customerUser.UserName = "Test";
        db.CustomerUsers.Add(customerUser);
        db.SaveChanges();
        return customerUser;
    }
    public CustomerUser SeedSingle(ApplicationDbContext db,int? customerId, int? CustomerUserLevelId = 0, int? departmentId = 0,int? groupId = 0)
    {
        var customerUser = _faker.Generate();
        customerUser.UserName = "Test";
        if (customerId != 0)
        {
            customerUser.CustomerId = customerId;
        }
        if(CustomerUserLevelId != 0)
        {
            customerUser.CustomerLevelId = CustomerUserLevelId;
        }
        if(departmentId != 0)
        {
            customerUser.CustomerDepartmentId = departmentId;
        }
        if(groupId != 0)
        {
            customerUser.CustomerGroupId = groupId;
        }
        db.CustomerUsers.Add(customerUser);
        db.SaveChanges();
        return customerUser;
    }
    public List<CustomerUser> SeedMany(ApplicationDbContext db,int numberOfEntitiesToCreate, int? customerId = null)
    {
        var customerUsers = _faker.Generate(numberOfEntitiesToCreate);
        foreach(var customerUser in customerUsers)
        {
            customerUser.CustomerId = customerId;
        }
        db.CustomerUsers.AddRange(customerUsers);
        db.SaveChanges();
        return customerUsers;
    }
    public CustomerUser SeedSingle(ApplicationDbContext db, CustomerLevel? customerLevel = null)
    {
        var customerUser = _faker.Generate();
        customerUser.UserName = "Test";
 
        if (customerLevel != null)
        {
            customerUser.CustomerLevel = customerLevel;
        }
        db.CustomerUsers.Add(customerUser);
        db.SaveChanges();
        return customerUser;
    }
}
