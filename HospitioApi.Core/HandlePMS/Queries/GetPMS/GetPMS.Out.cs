using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePMS.Queries.GetPMS;

public class GetPMSOut : BaseResponseOut
{
    public GetPMSOut(string message, List<PMSOut> pmsOut) : base(message)
    {
        PMSOut = pmsOut;
    }
    public List<PMSOut> PMSOut { get; set; } = new();
}
public class PMSOut
{
    public int Id { get; set; }
    public string PMS { get; set; } = string.Empty;

}