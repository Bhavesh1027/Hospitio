using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePropertyGallery.Commands.CreatePropertyGallery;

public class CreatePropertyGalleryOut : BaseResponseOut
{
    public CreatePropertyGalleryOut(string message
        //, CreatedPropertyGalleryOut createdPropertyGalleryOut
        ) : base(message) { }
    //{
    //    CreatedPropertyGalleryOut = createdPropertyGalleryOut;
    //}
    //public CreatedPropertyGalleryOut CreatedPropertyGalleryOut { get; set; }
}
public class CreatedPropertyGalleryOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public bool? IsActive { get; set; } = true;
    public string? PropertyImage { get; set; }
}
