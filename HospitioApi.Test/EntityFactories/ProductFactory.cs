using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class ProductFactory
{
    private readonly Faker<Product> _faker;
    public ProductFactory()
    {
        _faker = new Faker<Product>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.Name, "Test");
    }

    public Product SeedSingle(ApplicationDbContext db, User? user = null)
    {
        var product = _faker.Generate();

        if (user != null)
        {
            product.CreatedBy = user.Id;
        }

        db.Products.Add(product);
        db.TestCaseSaveChanges();
        return product;
    }

    public List<Product> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var products = _faker.Generate(numberOfEntitiesToCreate);
        db.Products.AddRange(products);
        db.SaveChanges();
        return products;
    }

    public Product Update(ApplicationDbContext db, Product product)
    {
        db.Products.Update(product);
        db.SaveChanges();
        return product;
    }
}
