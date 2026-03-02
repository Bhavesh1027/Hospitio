using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestsCheckInFormBuildersFactory
{
    private readonly Faker<CustomerGuestsCheckInFormBuilder> _faker;
    public CustomerGuestsCheckInFormBuildersFactory()
    {
        _faker = new Faker<CustomerGuestsCheckInFormBuilder>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.Color, f => f.Random.Replace("######"))
            .RuleFor(m => m.Name, f => f.Company.CompanyName())
            .RuleFor(m => m.Stars, f => f.Random.Byte(1, 5))
            .RuleFor(m => m.Logo, f => f.Image.PicsumUrl())
            .RuleFor(m => m.AppImage, f => f.Image.PicsumUrl())
            .RuleFor(m => m.SplashScreen, f => f.Image.PicsumUrl())
            .RuleFor(m => m.IsOnlineCheckInFormEnable, f => f.Random.Bool())
            .RuleFor(m => m.IsRedirectToGuestAppEnable, f => f.Random.Bool())
            .RuleFor(m => m.SubmissionMail, f => f.Internet.Email())
            .RuleFor(m => m.TermsLink, f => f.Internet.Url())
            .RuleFor(m => m.GuestWelcomeMessage, f => f.Lorem.Sentence())
            .RuleFor(m => m.JsonData, f => f.Random.Words());
    }

    public CustomerGuestsCheckInFormBuilder SeedSingle(ApplicationDbContext db, int? CustomerId = 0)
    {
        var customerGuestsCheckIn = _faker.Generate();
        if (CustomerId != 0)
        {
            customerGuestsCheckIn.CustomerId = CustomerId;
        }
        db.CustomerGuestsCheckInFormBuilders.Add(customerGuestsCheckIn);
        db.SaveChanges();
        return customerGuestsCheckIn;
    }

    public List<CustomerGuestsCheckInFormBuilder> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuestsCheckInForms = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestsCheckInFormBuilders.AddRange(customerGuestsCheckInForms);
        db.SaveChanges();
        return customerGuestsCheckInForms;
    }
}
