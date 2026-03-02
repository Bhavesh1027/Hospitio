using static HospitioApi.Shared.GetPagedExtension;

namespace HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyGallery
{
    public class GetPropertyGalleryIn : BaseSearchFilterOptions
    {
        public int CustomerPropertyInformationId { get; set; }
    }
}
