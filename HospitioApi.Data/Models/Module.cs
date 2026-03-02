using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models
{
    public partial class Module : Auditable
    {
        public Module()
        {
            ModuleServices = new HashSet<ModuleService>();
        }

        [MaxLength(30)]
        public string? Name { get; set; }
        /// <summary>
        ///  1: price required ,0: price not reuqire, 2: Price Period Dropdown (Month/Year/Day)
        /// </summary>
        [DefaultValue(0)]
        public byte? ModuleType { get; set; }

        public virtual ICollection<ModuleService> ModuleServices { get; set; }


    }
}
