

using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyGallery;

public class GetPropertyGalleryOut : BaseResponseOut
{
    public GetPropertyGalleryOut(string message, List<PropertyGalleryOut> propertyGallery) : base(message)
    {
        PropertyGallery = propertyGallery;
    }
    public List<PropertyGalleryOut> PropertyGallery { get; set; } = new();

    public class PropertyGalleryOut
    {
        public int Id { get; set; }

        public int CustomerPropertyInformationId { get; set; }

        public string PropertyImage { get; set; } = String.Empty;

        public bool IsDeleted { get; set; }


    }
}