using HospitioApi.Shared;


namespace HospitioApi.Core.HandleLeads.Commands.DeleteLead;
public class DeleteLeadOut : BaseResponseOut
{
    public DeleteLeadOut(string message, RemoveLeadOut removeLeadOut) : base(message)
    {
        RemoveLeadOut = removeLeadOut;
    }
    public RemoveLeadOut RemoveLeadOut { get; set; }
}

public class RemoveLeadOut
{
    public int DeletedLeadId { get; set; }
}
