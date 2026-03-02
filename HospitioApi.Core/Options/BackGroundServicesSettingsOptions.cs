using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Options;

public class BackGroundServicesSettingsOptions
{
    public const string BackGroundServicesSettings = "BackGroundServicesSettings";
    public bool CustomerQueueEnabled { get; set; }
    public bool CustomerQueueConsumerEnabled { get; set; }
    public bool GuestMessageConsumerEnabled { get; set; }
    public string? CustomerQueueTiming { get; set; } = string.Empty;
    public string? CustomerQueueConsumeTiming { get; set; } = string.Empty;
    public string? GuestMessageConsumeTiming { get; set; } = string.Empty;
    public string? ConnectionDefinationsBackgroundServiceTiming { get; set; } = string.Empty;
    public string? PaymentServiceDefinitionByIdBackgroundServiceTiming { get; set; } = string.Empty;
    public string? TemplateStatusCheckServiceTiming { get; set; } = string.Empty;
    public string? ReceiveWPMessageConsumeTiming { get; set; } = string.Empty;
    public string? SendAlertMessageBackGroundServiceTiming { get; set; } = string.Empty;
    public bool TaxiTransferStatusCheckServiceEnabled { get;set; }
    public string? TaxiTransferStatusCheckServiceTiming { get; set; } = string.Empty;
}
