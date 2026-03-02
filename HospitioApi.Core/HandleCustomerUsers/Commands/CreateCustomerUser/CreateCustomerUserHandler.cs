using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleUserAccount.Commands.CreateEditUserAccount;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text.RegularExpressions;

namespace HospitioApi.Core.HandleCustomerUsers.Commands.CreateCustomerUser;

public record CreateCustomerUserRequest(CreateCustomerUserIn In)
    : IRequest<AppHandlerResponse>;
public class CreateCustomerUserHandler : IRequestHandler<CreateCustomerUserRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomerUserHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
       )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerUserRequest request, CancellationToken cancellationToken)
    {
        var CustomerUserIn = request.In!;
        var CustomerDepartment = new CustomerDepartment();
        var CustomerGroup = new CustomerGroup();

        CustomerDepartment = await _db.CustomerDepartments.Where(d => d.Id == CustomerUserIn.DepartmentId).FirstOrDefaultAsync(cancellationToken);
        CustomerGroup = await _db.CustomerGroups.Where(g => g.Id == CustomerUserIn.GroupId).FirstOrDefaultAsync(cancellationToken);

        if (CustomerDepartment == null && CustomerUserIn.CustomerUserLevelId != (int)Shared.Enums.UserLevel.CEO)
        {
            return _response.Error("Department not exist.", AppStatusCodeError.Conflict409);
        }
        if (CustomerGroup == null && (CustomerUserIn.CustomerUserLevelId != (int)Shared.Enums.UserLevel.DeptManager && CustomerUserIn.CustomerUserLevelId != (int)Shared.Enums.UserLevel.CEO))
        {
            return _response.Error("Group not exist .", AppStatusCodeError.Conflict409);
        }

        //validate in user table
        if (CustomerUserIn.CustomerUserLevelId != (int)Shared.Enums.UserLevel.Staff || CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.CEO)
        {
            if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
            {
                var existDeptMgr = await _db.CustomerUsers.Where(u => u.Id != CustomerUserIn.Id && u.CustomerDepartmentId == CustomerUserIn.DepartmentId && u.CustomerLevelId == CustomerUserIn.CustomerUserLevelId && u.IsActive == true).FirstOrDefaultAsync(cancellationToken);
                if (existDeptMgr != null)
                {
                    return _response.Error("Department manager of given department already exist.", AppStatusCodeError.Conflict409);
                }
                if (CustomerDepartment!.DepartmentMangerId != null)
                {
                    return _response.Error("Department Manager of given department already exist.", AppStatusCodeError.Conflict409);
                }
            }
            else if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                var existGroupLeader = await _db.CustomerUsers.Where(u => u.Id != CustomerUserIn.Id && u.CustomerGroupId == CustomerUserIn.GroupId && u.CustomerLevelId == CustomerUserIn.CustomerUserLevelId && u.IsActive == true).FirstOrDefaultAsync(cancellationToken);
                if (existGroupLeader != null)
                {
                    return _response.Error("Group leader of given group already exist.", AppStatusCodeError.Conflict409);
                }
                if (CustomerGroup!.GroupLeaderId != null)
                {
                    return _response.Error("Group leader of given group already exist.", AppStatusCodeError.Conflict409);
                }
            }
            else if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.CEO)
            {
                var existCeo = await _db.CustomerUsers.Where(u => u.Id != CustomerUserIn.Id && u.CustomerLevelId == CustomerUserIn.CustomerUserLevelId && u.CustomerId == CustomerUserIn.CustomerId && u.IsActive == true).FirstOrDefaultAsync(cancellationToken);
                if (existCeo != null)
                {
                    return _response.Error("CEO already exist.", AppStatusCodeError.Conflict409);
                }
            }
        }
        if (await _db.CustomerUsers.AnyAsync(s => s.Email == CustomerUserIn.Email, cancellationToken))
        {
            return _response.Error("Email already exits.", AppStatusCodeError.Conflict409);
        }
        if (await _db.CustomerUsers.AnyAsync(s => s.UserName == CustomerUserIn.UserName, cancellationToken))
        {
            return _response.Error("Username already exits.", AppStatusCodeError.Conflict409);
        }
        if (await _db.CustomerUsers.AnyAsync(s => s.Email == CustomerUserIn.UserName, cancellationToken))
        {
            return _response.Error("Email or username already exits.", AppStatusCodeError.Conflict409);
        }

        CustomerUser custmerUser;
        custmerUser = new CustomerUser()
        {
            FirstName = CustomerUserIn.FirstName,
            LastName = CustomerUserIn.LastName,
            CustomerId = CustomerUserIn.CustomerId,
            Email = CustomerUserIn.Email,
            UserName = CustomerUserIn.UserName,
            ProfilePicture = CustomerUserIn.ProfilePicture,
            PhoneNumber = CustomerUserIn.PhoneNumber,
            PhoneCountry = CustomerUserIn.PhoneCountry,
            SupervisorId = CustomerUserIn.SupervisorId,
            CustomerGroupId = CustomerUserIn.GroupId,
            CustomerDepartmentId = CustomerUserIn.DepartmentId,
            CustomerLevelId = CustomerUserIn.CustomerUserLevelId,
            Title = CustomerUserIn.Title,
            IsActive = CustomerUserIn.IsActive,
        };

        await _db.CustomerUsers.AddAsync(custmerUser, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        custmerUser.Password = CryptoExtension.Encrypt(CustomerUserIn.Password, custmerUser.CustomerId.ToString());
        _db.CustomerUsers.Update(custmerUser);

        await _db.SaveChangesAsync(cancellationToken);

        if (CustomerUserIn.CustomerUserModulePermissions.Any())
        {
            var listOfUserPermission = new List<CustomerUsersPermission>();
            foreach (var permission in CustomerUserIn.CustomerUserModulePermissions)
            {
                CustomerUsersPermission addPermission = new();
                addPermission.CustomerUserId = custmerUser.Id;
                addPermission.CustomerPermissionId = permission.PermissionId;
                addPermission.IsView = permission.IsView;
                addPermission.IsEdit = permission.IsEdit;
                addPermission.IsDownload = permission.IsDownload;
                addPermission.IsReply = permission.IsReply;
                addPermission.IsUpload = permission.IsUpload;

                listOfUserPermission.Add(addPermission);
            }
            await _db.CustomerUsersPermissions.AddRangeAsync(listOfUserPermission, cancellationToken);
        }

        if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
        {
            CustomerDepartment.DepartmentMangerId = custmerUser.Id;
        }
        else if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
        {
            CustomerGroup.GroupLeaderId = custmerUser.Id;
        }
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateCustomerUserOut("User added successfully"));
    }
}
