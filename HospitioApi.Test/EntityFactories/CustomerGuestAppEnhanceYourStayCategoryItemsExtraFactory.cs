using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory
{
    private readonly Faker<CustomerGuestAppEnhanceYourStayCategoryItemsExtra> _faker;
    public CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory()
    {
        _faker = new Faker<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.QueType, f => f.Random.Byte())
            .RuleFor(m => m.Questions, f => f.Random.Words())
            .RuleFor(m => m.OptionValues, f => GenerateOptionValues(f))
            .RuleFor(m => m.JsonData, f => f.Random.Words())
            .RuleFor(m => m.IsPublish, f => f.Random.Bool());
    }

    public CustomerGuestAppEnhanceYourStayCategoryItemsExtra SeedSingle(ApplicationDbContext db, int? EnhanceYourStayItemId)
    {
        var enhanceYourStayCategoryItemsExtra = _faker.Generate();
        enhanceYourStayCategoryItemsExtra.CustomerGuestAppEnhanceYourStayItemId = EnhanceYourStayItemId;
        db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Add(enhanceYourStayCategoryItemsExtra);
        db.SaveChanges();
        return enhanceYourStayCategoryItemsExtra;
    }

    public List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var enhanceYourStayCategoryItemsExtras = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.AddRange(enhanceYourStayCategoryItemsExtras);
        db.SaveChanges();
        return enhanceYourStayCategoryItemsExtras;
    }
    static string? GenerateOptionValues(Faker f)
    {
        // Generate a list of option values
        var optionValues = f.Random.WordsArray(2)
            .Select(option => new { option, hasQuantityBar = false, required = true })
            .ToList();

        // Serialize the list to JSON
        return System.Text.Json.JsonSerializer.Serialize(optionValues);
    }
}
