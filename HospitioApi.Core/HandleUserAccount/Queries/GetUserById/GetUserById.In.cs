using static HospitioApi.Shared.GetPagedExtension;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUserById
{
    public class GetUserByIdIn : BaseSearchFilterOptions
    {
        public int Id { get; set; }
    }
}
