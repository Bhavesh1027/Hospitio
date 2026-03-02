using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Options
{
    public class FrontEndLinksSettingsOptions
    {
        public const string FrontEndLinksSettings = "FrontEndLinksSettings"; /** String must match property in appsettings.json file */
        public string GuestPortal { get; set; } = string.Empty;

    }
}
