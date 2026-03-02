

using FluentValidation;


namespace HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
public class GetDepartmentsUsersValidator : AbstractValidator<GetDepartmentsUsersRequest>
{
    public class GetDepartmentsUsersInValidator : AbstractValidator<GetDepartmentsUsersIn>
    {
        public GetDepartmentsUsersInValidator()
        {


        }
    }
}