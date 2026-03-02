using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class GroupsFactory
{
    private readonly Faker<Group> _faker;
    public GroupsFactory()
    {
        _faker = new Faker<Group>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.IsActive, true)
            .RuleFor(m => m.Name, f => f.Name.FullName());

    }

    public Group SeedSingle(ApplicationDbContext db,int? departmentId = 0)
    {
        var group = _faker.Generate();
        if(departmentId != 0) 
        {
            group.DepartmentId = departmentId;
        }
        db.Groups.Add(group);
        db.SaveChanges();
        return group;
    }

    public List<Group> SeedMany(ApplicationDbContext db, int departmentId, int numberOfEntitiesToCreate)
    {
        var groups = Generate(departmentId, numberOfEntitiesToCreate);
        db.Groups.AddRange(groups);
        db.SaveChanges();
        return groups;
    }

    private List<Group> Generate(int departmentId, int numberOfEntitiesCreate)
    {
        var faker = _faker.Clone()
                   .RuleFor(m => m.DepartmentId, departmentId);
        return faker.Generate(numberOfEntitiesCreate);
    }
}
