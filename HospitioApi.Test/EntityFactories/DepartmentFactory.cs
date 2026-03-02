using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class DepartmentFactory
{
    private readonly Faker<Department> _faker;
    public DepartmentFactory()
    {
        _faker = new Faker<Department>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.Name , f => f.Lorem.Word())
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }
    public Department SeedSingle(ApplicationDbContext db)
    {
        var department = _faker.Generate();
        db.Departments.Add(department);
        db.SaveChanges();
        return department;
    }

    public List<Department> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var department = _faker.Generate(numberOfEntitiesToCreate);
        db.Departments.AddRange(department);
        db.SaveChanges();
        return department;
    }
    public Department Update(ApplicationDbContext db, Department department)
    {
        db.Departments.Update(department);
        return department;
    }
}
