using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuestPMS.Commands.CreateCustomerGuestPMS;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;


public class CustomerGuestRegisterPMSEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       singular: "api/hospitio-customer/guestregisterpms");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
   {
        app.MapPost($"/{Route.Singular}/create", CreateAsync)
        .RequireAuthorization()
    };

    #region Delegates    
    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, HttpRequest request, CT ct)
    {
        if (!request.HasFormContentType)
            return Results.NoContent();

        var form = await request.ReadFormAsync(ct);
        var formFile = form.Files["DocumentAttachment"];

        //if (formFile is null || formFile.Length == 0)
        //    return Results.NoContent();
        //if (!form.ContainsKey("X-Customer-Id"))
        //{
        //    return Results.NoContent();
        //}

        //if (!form.ContainsKey("X-Doc-Type"))
        //{
        //    return Results.NoContent();
        //}
        UploadDocumentType UploadDocument;

        if (Enum.TryParse<UploadDocumentType>(UploadDocumentType.documentattachment.ToString(), out UploadDocument))
        {
            var createCustomerGuestPMSIn = new CreateCustomerGuestPMSIn();
            createCustomerGuestPMSIn.DocumentAttachment = form.Files[0];
            createCustomerGuestPMSIn.ContainerName = cp.CustomerId();
            createCustomerGuestPMSIn.DocumentType = UploadDocumentType.documentattachment.ToString();
            createCustomerGuestPMSIn.Title = request.Form["Title"].ToString();
            createCustomerGuestPMSIn.FirstName = request.Form["FirstName"].ToString();
            createCustomerGuestPMSIn.LastName = request.Form["LastName"].ToString();
            createCustomerGuestPMSIn.Email = request.Form["Email"].ToString();
            createCustomerGuestPMSIn.MobileNumber = request.Form["MobileNumber"].ToString();
            createCustomerGuestPMSIn.Street = request.Form["Street"].ToString();
            createCustomerGuestPMSIn.PostalCode = request.Form["PostalCode"].ToString();
            createCustomerGuestPMSIn.City = request.Form["City"].ToString();
            createCustomerGuestPMSIn.Country = request.Form["Country"].ToString();
            createCustomerGuestPMSIn.ReservationNumber = request.Form["ReservationNumber"].ToString();
            createCustomerGuestPMSIn.ArrivalDate = Convert.ToDateTime(request.Form["ArrivalDate"]);
            createCustomerGuestPMSIn.DepartureDate = Convert.ToDateTime(request.Form["DepartureDate"]);
            createCustomerGuestPMSIn.VATNumber = request.Form["VATNumber"].ToString();
            createCustomerGuestPMSIn.DocumentName = request.Form["DocumentName"].ToString();
            return await mtrHlpr.ToResultAsync(new CreateCustomerGuestPMSRequest(createCustomerGuestPMSIn, cp.CustomerId()!), ct);
        }

        else
        {
            return Results.BadRequest();
        }
    }
    #endregion
}
