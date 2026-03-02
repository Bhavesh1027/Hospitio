using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using HospitioApi.Data;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;

namespace HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequestStatus
{
    public record UpdateGuestRequestStatus(UpdateGuestRequestStatusIn In) : IRequest<AppHandlerResponse>;
    public class UpdateGuestRequestStatusHandler : IRequestHandler<UpdateGuestRequestStatus, AppHandlerResponse>
    {
		private readonly ApplicationDbContext _db;
		private readonly IHandlerResponseFactory _response;
		public UpdateGuestRequestStatusHandler(ApplicationDbContext db, IHandlerResponseFactory response)
		{
			_db = db;
			_response = response;
		}
		public async Task<AppHandlerResponse> Handle(UpdateGuestRequestStatus request, CancellationToken cancellationToken)
		{
			if ((request.In.GuestRequestId == null || request.In.GuestRequestId == 0) && (request.In.EnhanceStayGuestRequestId == null || request.In.EnhanceStayGuestRequestId == 0))
            {
				return _response.Error($"Request can not be null.", AppStatusCodeError.Forbidden403);
			}

			var guestRequestId = request.In.GuestRequestId;
			var enhanceStayGuestRequestId = request.In.EnhanceStayGuestRequestId;

			if (guestRequestId != null && guestRequestId > 0)
			{
				var guestRequest = await _db.GuestRequests.Where(e => e.Id == request.In.GuestRequestId).FirstOrDefaultAsync(cancellationToken);
				if (guestRequest == null)
				{
					return _response.Error($"Guest request with Id {request.In.GuestRequestId} could not be found.", AppStatusCodeError.Gone410);
				}
				guestRequest.Status = (byte)request.In.Status;
				guestRequest.Id = request.In.GuestRequestId ?? 0;

			}
			else if (enhanceStayGuestRequestId != null && enhanceStayGuestRequestId > 0)
			{
				var enhanceStayGuestRequest = await _db.EnhanceStayItemsGuestRequests.Where(c => c.Id == request.In.EnhanceStayGuestRequestId).FirstOrDefaultAsync(cancellationToken);
				if (enhanceStayGuestRequest == null)
				{
					return _response.Error($"Enhance Stay Guest Request with Id {request.In.EnhanceStayGuestRequestId} could not be found.", AppStatusCodeError.Gone410);
				}
				//enhanceStayGuestRequest.Status = request.In.Status.ToString();
				enhanceStayGuestRequest.Status = (byte)request.In.Status;
				enhanceStayGuestRequest.Id = request.In.EnhanceStayGuestRequestId ?? 0;
			}

			await _db.SaveChangesAsync(cancellationToken);

			return _response.Success(new UpdateGuestRequestStatusOut("Update guest enhance request successful."));
		}
	}
}
