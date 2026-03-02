using MediatR;
using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleFiles.Commands.CreateFile;
using HospitioApi.Core.HandleFiles.Commands.UploadCommunicationFile;
using HospitioApi.Core.HandleFiles.Queries.GetCommunicationFile;
using HospitioApi.Core.HandleFiles.Queries.GetFile;
using HospitioApi.Helpers;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class WebFileEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/hospitio-guest/file");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

           app.MapPost($"/{Route.Singular}/upload-document", UploadDocumentAsync)
        .AllowAnonymous(),

          app.MapPost($"/{Route.Singular}/image", GetFileAsync)
         .AllowAnonymous(),

         app.MapPost($"/{Route.Singular}/upload-communicationfile", UploadCommunicationFileAsync)
        .AllowAnonymous(),

         app.MapPost($"/{Route.Singular}/communicationfile", GetCommunicationFileAsync)
         //.RequireAuthorization(),
         .AllowAnonymous(),

    };

    #region Delegates

    private async Task<IResult> UploadDocumentAsync(IMediator _mediator, [FromServices] IMediatorHelper mtrHlpr, HttpRequest request, HttpResponse outResponse, ClaimsPrincipal cp, CT ct)
    {

        if (!request.HasFormContentType)
            return Results.NoContent();

        var form = await request.ReadFormAsync(ct);
        var formFile = form.Files["File"];

        if (formFile is null || formFile.Length == 0)
            return Results.NoContent();

        if (!form.ContainsKey("X-Customer-Id"))
        {
            return Results.NoContent();
        }

        if (!form.ContainsKey("X-Doc-Type"))
        {
            return Results.NoContent();
        }
        UploadDocumentType UploadDocument;

        if (Enum.TryParse<UploadDocumentType>(request.Form["X-Doc-Type"].ToString(), out UploadDocument))
        {
            //var handlerResponse = await _mediator.Send(new CreateFileRequest(new CreateFileIn(form.Files[0], request.Form["X-Customer-Id"].ToString(), request.Form["X-Doc-Type"].ToString())), ct);

            //return handlerResponse.Response!;
            return await mtrHlpr.ToResultAsync(new CreateFileRequest(new CreateFileIn(form.Files[0], request.Form["X-Customer-Id"].ToString(), request.Form["X-Doc-Type"].ToString())), ct);

            //outResponse.Headers.Add("file-id", response.Location.ToString());
            //outResponse.Headers.Add("X-Temp-Uri", response.TempSasUri);
            //return handlerResponse.HasResponse
            //    ? Results.File(response.MemoryStream, response.ContentType, response.FileName)
            //    : Results.NotFound();
            //return await mtrHlpr.ToResultAsync(response, ct);
        }
        else
        {
            return Results.BadRequest();
        }

    }
    private async Task<IResult> GetFileAsync(IMediator _mediator, [FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, HttpRequest request, HttpResponse outResponse, CT ct)
    {
        if (!request.HasFormContentType)
            return Results.NoContent();

        var form = await request.ReadFormAsync(ct);

        if (!form.ContainsKey("X-Location"))
        {
            return Results.NoContent();
        }
        var Location = request.Form["X-Location"].ToString();
        //var handlerResponse = await _mediator.Send(new GetFileRequest(@in), ct);
        //var handlerResponse = await _mediator.Send(new GetFileRequest(Location), ct);
        ////outResponse.Headers.Add("file-id", userId);
        //var response = (GetFileOut)(handlerResponse.Response!);
        //return handlerResponse.HasResponse
        //    ? Results.File(response.MemoryStream, response.ContentType, response.FileDownloadName)
        //    : Results.NotFound();
        return await mtrHlpr.ToResultAsync(new GetFileRequest(Location), ct);
    }

    private async Task<IResult> UploadCommunicationFileAsync(IMediator _mediator, [FromServices] IMediatorHelper mtrHlpr, HttpRequest request, HttpResponse outResponse, ClaimsPrincipal cp, CT ct)
    {

        if (!request.HasFormContentType)
            return Results.NoContent();

        var form = await request.ReadFormAsync(ct);
        var formFile = form.Files["File"];
        if (formFile is null || formFile.Length == 0)
            return Results.NoContent();

        if (!form.ContainsKey("X-Doc-Type"))
        {
            return Results.NoContent();
        }
        // string DirectoryPath = form.ContainsKey("BusinessName").ToString() +"/"+ form.ContainsKey("DocumentType").ToString();
        // var value= request.Form["BusinessName"].ToString();
        //form.Keys.Any("BusinessName");
        UploadDocumentType UploadDocument;

        if (Enum.TryParse<UploadDocumentType>(request.Form["X-Doc-Type"].ToString(), out UploadDocument))
        {
            //var handlerResponse = await _mediator.Send(new CreateFileRequest(new CreateFileIn(form.Files[0], null, request.Form["X-Doc-Type"].ToString())), ct);

            //var response = (CreateFileOut)(handlerResponse.Response!);

            //outResponse.Headers.Add("file-id", response.Location.ToString());
            //outResponse.Headers.Add("X-Temp-Uri", response.TempSasUri);
            //return handlerResponse.HasResponse
            //    ? Results.File(response.MemoryStream, response.ContentType, response.FileName)
            //    : Results.NotFound();

            return await mtrHlpr.ToResultAsync(new UploadCommunicationFileRequest(new UploadCommunicationFileIn(form.Files[0], request.Form["X-Doc-Type"].ToString())), ct);


        }
        else
        {
            return Results.BadRequest();
        }



    }

    private async Task<IResult> GetCommunicationFileAsync(IMediator _mediator, [FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, HttpRequest request, HttpResponse outResponse, CT ct)
    {
        if (!request.HasFormContentType)
            return Results.NoContent();

        var form = await request.ReadFormAsync(ct);

        if (!form.ContainsKey("X-Location"))
        {
            return Results.NoContent();
        }
        var Location = request.Form["X-Location"].ToString();
        //var handlerResponse = await _mediator.Send(new GetFileRequest(@in), ct);
        //var handlerResponse = await _mediator.Send(new GetFileRequest(Location), ct);
        ////outResponse.Headers.Add("file-id", userId);
        //var response = (GetFileOut)(handlerResponse.Response!);
        //return handlerResponse.HasResponse
        //    ? Results.File(response.MemoryStream, response.ContentType, response.FileDownloadName)
        //    : Results.NotFound();

        return await mtrHlpr.ToResultAsync(new GetCommunicationFileRequest(Location), ct);

    }
    #endregion

}
