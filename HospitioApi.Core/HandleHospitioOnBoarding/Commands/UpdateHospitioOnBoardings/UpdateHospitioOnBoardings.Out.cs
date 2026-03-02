using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioOnBoarding.Commands.UpdateHospitioOnBoardings;

public class UpdateHospitioOnBoardingsOut : BaseResponseOut
{
    public UpdateHospitioOnBoardingsOut(string message, UpdatedHospitioOnBoardingsOut updatedHospitioOnBoardingsOut) : base(message)
    {
        UpdatedHospitioOnBoardingsOut = updatedHospitioOnBoardingsOut;
    }
    public UpdatedHospitioOnBoardingsOut UpdatedHospitioOnBoardingsOut { get; set; }
}
public class UpdatedHospitioOnBoardingsOut
{
    public int Id { get; set; }
}
