using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandlePropertyGallery.Commands.CreatePropertyGallery;
public record CreatePropertyGalleryRequest(CreatePropertyGalleryIn In)
: IRequest<AppHandlerResponse>;

public class EditPropertyGalleryHandler : IRequestHandler<CreatePropertyGalleryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public EditPropertyGalleryHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreatePropertyGalleryRequest request, CancellationToken cancellationToken)
    {
        if (request.In == null)
        {
            return _response.Error($"Request cannot be null.", AppStatusCodeError.Forbidden403);
        }
        //var propertyGallary = await _db.CustomerPropertyGalleries.Where(e => e.CustomerPropertyInformationId == request.In.CustomerPropertyInformationId).ToListAsync(cancellationToken);
        List<CustomerPropertyGallery> galleryInfo = new();
        //if (propertyGallary != null)
        //{
        //    while (propertyGallary.Count > 0)
        //    {
        //        var gallery = propertyGallary.Last();
        //        propertyGallary.Remove(gallery);
        //        _db.CustomerPropertyGalleries.Remove(gallery);
        //    }
        //}
        foreach (var imgUrl in request.In.CreatePropertyGalleryImagesIns)
        {
            if(imgUrl.Id > 0)
            {
                var propertyGallary = await _db.CustomerPropertyGalleries.Where(e => e.Id == imgUrl.Id).FirstOrDefaultAsync(cancellationToken);
                if (propertyGallary != null)
                {
                    var json = request.In.CreatePropertyGalleryImagesIns.Where(e => e.Id == imgUrl.Id).FirstOrDefault();
                    propertyGallary.IsPublish = propertyGallary.IsPublish;
                    propertyGallary.JsonData = JsonConvert.SerializeObject(json);
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                CustomerPropertyGallery obj = new();
                obj.CustomerPropertyInformationId = imgUrl.CustomerPropertyInformationId;
                obj.PropertyImage = imgUrl.PropertyImage;
                obj.IsActive = imgUrl.IsActive;
                obj.IsPublish = false;
                obj.JsonData = null;
                galleryInfo.Add(obj);
            }
            
        }

        await _db.CustomerPropertyGalleries.AddRangeAsync(galleryInfo, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        //CustomerPropertyGallery galleryData = null;
        //if (request.In.Id > 0)
        //{
        //    galleryData = await _db.CustomerPropertyGalleries.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        //    if (galleryData != null)
        //    {
        //        galleryData.IsPublish = galleryData.IsPublish;
        //        galleryData.JsonData = JsonConvert.SerializeObject(request.In);
        //        await _db.SaveChangesAsync(cancellationToken);
        //    }
        //} else
        //{
        //    galleryData = new CustomerPropertyGallery()
        //    {
        //        CustomerPropertyInformationId = request.In.CustomerPropertyInformationId,
        //        PropertyImage = request.In.PropertyImage,
        //        IsActive = request.In.IsActive,
        //        IsPublish = false,
        //        JsonData = null
        //    };
        //    await _db.SaveChangesAsync(cancellationToken);
        //}

        //CreatedPropertyGalleryOut createdPropertyGalleryOut = new CreatedPropertyGalleryOut()
        //{
        //    Id = galleryData.Id,
        //    CustomerPropertyInformationId = request.In.CustomerPropertyInformationId,
        //    PropertyImage = request.In.PropertyImage,
        //    IsActive = request.In.IsActive
        //};


        return _response.Success(new CreatePropertyGalleryOut("Gallery created successfully."));
    }
}