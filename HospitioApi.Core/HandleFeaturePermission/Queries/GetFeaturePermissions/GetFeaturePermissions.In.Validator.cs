using FluentValidation;

namespace HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
public class GetFeaturePermissionsValidator : AbstractValidator<GetFeaturePermissionsRequest>
{
    public GetFeaturePermissionsValidator()
    {
    }

    public class GetFeaturePermissionsInValidator : AbstractValidator<GetFeaturePermissionsIn>
    {
        public GetFeaturePermissionsInValidator()
        {
        }
    }
}