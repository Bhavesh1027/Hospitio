using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using System.Text.Json;

namespace HospitioApi.Test.EntityFactories;

public class ScreenDisplayOrderAndStatusesFactory
{
    private readonly Faker<ScreenDisplayOrderAndStatus> _faker;
    public ScreenDisplayOrderAndStatusesFactory()
    {
        _faker = new Faker<ScreenDisplayOrderAndStatus>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public ScreenDisplayOrderAndStatus SeedSingle(ApplicationDbContext db, int id, int ScreenName)
    {
        var screenDisplayOrderAndStatus = _faker.Generate();
        screenDisplayOrderAndStatus.JsonData = "[{\"name\":\"test name\",\"displayOrder\":1,\"isDisable\":true,\"image\":\"test image\",\"items\":1,\"categories\":1,\"customerAppBuliderId\":1}]";
        screenDisplayOrderAndStatus.RefrenceId = id;
        screenDisplayOrderAndStatus.ScreenName = ScreenName;
        db.ScreenDisplayOrderAndStatuses.Add(screenDisplayOrderAndStatus);
        db.SaveChanges();
        return screenDisplayOrderAndStatus;
    }
}
