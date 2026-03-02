using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerUsers.Commands.CreateCustomerUser;
using HospitioApi.Core.HandleCustomerUsers.Commands.EditCustomerUser;
using HospitioApi.Core.HandleCustomerUsers.Commands.UpdateCustomerUserStatus;
using HospitioApi.Core.HandleCustomerUsers.Commands.UpdateMuteStatus;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerInfoByWidgetChatId;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerSupervisorUsers;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserByCustomerId;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserById;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetDepartmentCustomerUsers;
using HospitioApi.Core.HandleUserAccount.Commands.UpdateUserProfile;
using HospitioApi.Core.HandleUserAccount.Queries.GetUserProfile;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerUserEndPoints : IEndpointsModule
{
	public AppRoute Route { get; init; } = new(
	  plural__: "api/hospitio-customer/Users",
	  singular: "api/hospitio-customer/User");

	public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
	{
		   app.MapGet($"/{Route.Plural}",GetCustomerUsersAsync)
			 .RequireAuthorization(),
		   app.MapPost($"/{Route.Singular}", CreateCustomerUserAsync)
			 .RequireAuthorization(),
		   app.MapGet($"/{Route.Plural}/getSupervisorUsers", GetCustomerSupervisorUsersAsync)
			.AllowAnonymous(),
		   app.MapGet($"/{Route.Plural}" + "/all", GetCustomerDepartmentsUsersAsync)
			.AllowAnonymous(),
		   app.MapGet($"/{Route.Singular}"  + "/{Id}", GetCustomerUserByIdAsync)
			.AllowAnonymous(),
		   app.MapPost($"/{Route.Singular}"  + "/edit", EditCustomerUserAsync)
			.AllowAnonymous(),
		   app.MapGet($"/{Route.Singular}"  + "/Profile", GetUserProfileAsync)
			.RequireAuthorization(),
		   app.MapPost($"/{Route.Singular}"  + "/ProfileUpdate", UpdateUserProfileAsync)
			.RequireAuthorization(),
		   app.MapPost($"/{Route.Singular}"  + "/status-update", UpdateCustomerUserAsync)
			.AllowAnonymous(),
		   app.MapGet($"/{Route.Singular}"  + "/getCustomerByWidgetChatId", GetCustomerByWidgetChatId)
			.AllowAnonymous(),
		   app.MapPost($"/{Route.Singular}"  + "/mute-update", UpdateMuteStatusAsync)
			.RequireAuthorization(),
	};
	#region Delegates
	private async Task<IResult> GetCustomerUsersAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
	{
		return await mtrHlpr.ToResultAsync(new GetCustomerStaffsByCustomerIdRequest(cp.CustomerId()!, Convert.ToInt32(cp.UserId()!)), ct);
	}
	private async Task<IResult> CreateCustomerUserAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerUserIn @in, ClaimsPrincipal cp, CT ct)
	{
		@in.CustomerId = Convert.ToInt32(cp.CustomerId());
		return await mtrHlpr.ToResultAsync(new CreateCustomerUserRequest(@in), ct);
	}

	private async Task<IResult> GetCustomerSupervisorUsersAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, int? DepartmentId, int? GroupId, int? CustomerUserLevel, int? CustomerUserId, ClaimsPrincipal cp)
	{
		GetCustomerSupervisorUsersIn @In = new()
		{
			DepartmentId = DepartmentId,
			GroupId = GroupId,
			CustomerUserLevelId = CustomerUserLevel,
			CustomerUserId = CustomerUserId == null ? 0 : CustomerUserId.Value,
			CustomerId = Convert.ToInt32(cp.CustomerId())
		};
		return await mtrHlpr.ToResultAsync(new GetCustomerSupervisorUsersRequest(@In), ct);
	}
	private async Task<IResult> GetCustomerDepartmentsUsersAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, CT ct, ClaimsPrincipal cp)
	{
		GetDepartmentCustomerUsersIn @In = new()
		{
			//SearchColumn = SearchColumn,
			SearchValue = SearchValue,
			PageNo = PageNo,
			PageSize = PageSize,
			SortColumn = SortColumn,
			SortOrder = SortOrder,
			CustomerId = Convert.ToInt32(cp.CustomerId())
		};
		return await mtrHlpr.ToResultAsync(new GetDepartmentCustomerUsersRequest(@In), ct);
	}
	private async Task<IResult> GetCustomerUserByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, CT ct)
	{
		GetCustomerUserByIdIn @In = new()
		{
			Id = Id
		};
		return await mtrHlpr.ToResultAsync(new GetCustomerUserByIdRequest(@In), ct);
	}
	private async Task<IResult> EditCustomerUserAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] EditCustomerUserIn @in, CT ct, ClaimsPrincipal cp)
	{
		@in.CustomerId = Convert.ToInt32(cp.CustomerId());
		return await mtrHlpr.ToResultAsync(new EditCustomerUserRequest(@in), ct);
	}
	private async Task<IResult> GetUserProfileAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, ClaimsPrincipal cp)
	{
		GetUserProfileIn @In = new()
		{
			UserId = Convert.ToInt32(cp.UserId()),
			UserType = cp.UserType()
		};
		return await mtrHlpr.ToResultAsync(new GetUserProfileRequest(@In), ct);
	}
	private async Task<IResult> UpdateUserProfileAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateUserProfileIn @in, CT ct)
	{

		@in.UserType = Convert.ToInt32(cp.UserType());
		@in.CustomerId = Convert.ToInt32(cp.CustomerId());
		return await mtrHlpr.ToResultAsync(new UpdateUserProfileRequest(@in), ct);
	}

	//=> await mtrHlpr.ToResultAsync(new EditCustomerUserRequest(@in), ct);

	private async Task<IResult> UpdateCustomerUserAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomerUserStatusIn @in, CT ct)
	=> await mtrHlpr.ToResultAsync(new UpdateCustomerUserStatusRequest(@in), ct);

	private async Task<IResult> GetCustomerByWidgetChatId([FromServices] IMediatorHelper mtrHlpr, string? widgetId, CT ct)
	{
		GetCustomerInfoByWidgetChatIdIn @In = new()
		{
			WidgetChatId = widgetId
		};
		return await mtrHlpr.ToResultAsync(new GetCustomerInfoByWidgetChatIdRequest(@In), ct);
	}

	private async Task<IResult> UpdateMuteStatusAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateMuteStatusIn @in, CT ct, ClaimsPrincipal cp)
	{
		@in.CustomerUserId = Convert.ToInt32(cp.UserId());
		return await mtrHlpr.ToResultAsync(new UpdateCustomerMuteStatusRequest(@in), ct);
	}
	#endregion
}
