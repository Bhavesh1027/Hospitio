using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories
{
    public class CustomerGuestAppEnhanceYourStayItemsImagesFactory
    {
        public readonly Faker<CustomerGuestAppEnhanceYourStayItemsImage> _faker;
        public CustomerGuestAppEnhanceYourStayItemsImagesFactory()
        {
            _faker = new Faker<CustomerGuestAppEnhanceYourStayItemsImage>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.ItemsImages, f => f.Image.PicsumUrl())
                .RuleFor(m => m.JsonData, f => f.Random.Words())
                .RuleFor(m => m.IsPublish, f => f.Random.Bool());
        }
        public CustomerGuestAppEnhanceYourStayItemsImage SeedSingle(ApplicationDbContext db,int CustomerGuestAppEnhanceYourStayItemId = 0)
        {
            var customerGuestAppEnhanceYourStayItemsImage = _faker.Generate();
            customerGuestAppEnhanceYourStayItemsImage.CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId;
            db.CustomerGuestAppEnhanceYourStayItemsImages.Add(customerGuestAppEnhanceYourStayItemsImage);
            db.SaveChanges();
            return customerGuestAppEnhanceYourStayItemsImage;
        }
    }
}
