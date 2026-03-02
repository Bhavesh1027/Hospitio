namespace HospitioApi.Core.HandlePropertyGallery.Commands.EditPropertyGallery
{
    public class EditPropertyGalleryIn
    {
        public int Id { get; set; }
        public int CustomerPropertyInformationId { get; set; }
        public bool IsActive { get; set; } = true;
        public string? PropertyImages { get; set; }
    }

}
