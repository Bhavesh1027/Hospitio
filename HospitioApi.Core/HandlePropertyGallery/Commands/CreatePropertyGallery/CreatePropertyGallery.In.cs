namespace HospitioApi.Core.HandlePropertyGallery.Commands.CreatePropertyGallery
{   
    public class CreatePropertyGalleryIn
    {
        public List<CreatePropertyGalleryImagesIn> CreatePropertyGalleryImagesIns { get; set; } = new List<CreatePropertyGalleryImagesIn>();
    }
    public class CreatePropertyGalleryImagesIn
    {
        public int Id { get; set; }
        public int CustomerPropertyInformationId { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? PropertyImage { get; set; }
        public bool IsDeleted { get; set; }
    }

}
