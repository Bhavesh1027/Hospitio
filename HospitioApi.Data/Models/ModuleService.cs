using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models
{
    public partial class ModuleService : Auditable
    {
        public ModuleService()
        {
            ProductModuleServices = new HashSet<ProductModuleService>();
        }
        public int ModuleId { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }

        public virtual Module? Module { get; set; }
        public virtual ICollection<ProductModuleService> ProductModuleServices { get; set; }


    }
}
