using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerUsers.Commands.EditCustomerUser;
public record EditCustomerUserRequest(EditCustomerUserIn In)
    : IRequest<AppHandlerResponse>;
public class EditCustomerUserHandler : IRequestHandler<EditCustomerUserRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public EditCustomerUserHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
       )
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(EditCustomerUserRequest request, CancellationToken cancellationToken)
    {
        var CustomerUserIn = request.In!;
        var CustomerDepartment = new CustomerDepartment();
        var CustomerGroup = new Data.Models.CustomerGroup();
        var reDepartment = new CustomerDepartment();
        var reGroup = new Data.Models.CustomerGroup();

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
                var existDeptMgr = await _db.CustomerUsers.Where(u => u.Id != CustomerUserIn.Id && u.CustomerDepartmentId == CustomerUserIn.DepartmentId && u.CustomerLevelId == CustomerUserIn.CustomerUserLevelId && u.IsActive == true).SingleOrDefaultAsync(cancellationToken);
                if (existDeptMgr != null)
                {
                    return _response.Error("Department manager of given department already exist.", AppStatusCodeError.Conflict409);
                }
            }
            else if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                var existGroupLeader = await _db.CustomerUsers.Where(u => u.Id != CustomerUserIn.Id && u.CustomerGroupId == CustomerUserIn.GroupId && u.CustomerLevelId == CustomerUserIn.CustomerUserLevelId && u.IsActive == true).SingleOrDefaultAsync(cancellationToken);
                if (existGroupLeader != null)
                {
                    return _response.Error("Group leader of given group already exist.", AppStatusCodeError.Conflict409);
                }
            }
            else if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.CEO)
            {
                var existCeo = await _db.CustomerUsers.Where(u => u.Id != CustomerUserIn.Id && u.CustomerLevelId == CustomerUserIn.CustomerUserLevelId  && u.CustomerId == CustomerUserIn.CustomerId && u.IsActive == true).FirstOrDefaultAsync(cancellationToken);
                if (existCeo != null)
                {
                    return _response.Error("CEO already exist.", AppStatusCodeError.Conflict409);
                }
            }
        }

        var editCustomerUser = await _db.CustomerUsers.Include(p => p.CustomerUsersPermissions).Where(u => u.Id == CustomerUserIn.Id).FirstOrDefaultAsync(cancellationToken);

        if (editCustomerUser == null)
        {
            return _response.Error("Current User does not exist.", AppStatusCodeError.UnprocessableEntity422);
        }
        else
        {
            if (editCustomerUser.CustomerLevelId == (int)Shared.Enums.UserLevel.DeptManager)
            {
                var FindDepartment = await _db.CustomerDepartments.Where(d => d.DepartmentMangerId == editCustomerUser.Id).ToListAsync(cancellationToken);

                if (FindDepartment != null || FindDepartment.Count != 0)
                {
                    foreach (var departmentData in FindDepartment)
                    {
                        departmentData.DepartmentMangerId = null;
                    }
                }
            }
            if (editCustomerUser.CustomerLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                var FindGroup = await _db.CustomerGroups.Where(g => g.GroupLeaderId == editCustomerUser.Id).ToListAsync(cancellationToken);

                if (FindGroup != null || FindGroup.Count != 0)
                {
                    foreach (var departmentData in FindGroup)
                    {
                        departmentData.GroupLeaderId = null;
                    }
                }
            }


            if (!string.IsNullOrEmpty(CustomerUserIn.Password))
                editCustomerUser.Password = CryptoExtension.Encrypt(CustomerUserIn.Password, editCustomerUser.CustomerId.ToString());

            editCustomerUser.FirstName = CustomerUserIn.FirstName;
            editCustomerUser.LastName = CustomerUserIn.LastName;
            editCustomerUser.Email = CustomerUserIn.Email;
            editCustomerUser.UserName = CustomerUserIn.UserName;
            editCustomerUser.ProfilePicture = CustomerUserIn.ProfilePicture;
            editCustomerUser.PhoneNumber = CustomerUserIn.PhoneNumber;
            editCustomerUser.PhoneCountry = CustomerUserIn.PhoneCountry;
            editCustomerUser.SupervisorId = CustomerUserIn.SupervisorId;
            editCustomerUser.CustomerGroupId = CustomerUserIn.GroupId;
            editCustomerUser.CustomerDepartmentId = CustomerUserIn.DepartmentId;
            editCustomerUser.CustomerLevelId = CustomerUserIn.CustomerUserLevelId;
            editCustomerUser.Title = CustomerUserIn.Title;
            editCustomerUser.IsActive = CustomerUserIn.IsActive;

            //update user module permissions

            if (CustomerUserIn.CustomerUserModulePermissions is not null)
            {
                if (CustomerUserIn.CustomerUserModulePermissions.Any())
                {

                    foreach (var permission in CustomerUserIn.CustomerUserModulePermissions)
                    {

                        var addPermission = await _db.CustomerUsersPermissions.Where(u => u.Id == permission.Id).FirstOrDefaultAsync(cancellationToken);

                        if (addPermission == null)
                        {
                            CustomerUsersPermission addPermissionNew = new();
                            addPermissionNew.CustomerUserId = CustomerUserIn.Id;
                            addPermissionNew.CustomerPermissionId = permission.PermissionId;
                            addPermissionNew.IsView = permission.IsView;
                            addPermissionNew.IsEdit = permission.IsEdit;
                            addPermissionNew.IsDownload = permission.IsDownload;
                            addPermissionNew.IsReply = permission.IsReply;
                            addPermissionNew.IsUpload = permission.IsUpload;
                            await _db.CustomerUsersPermissions.AddAsync(addPermissionNew, cancellationToken);
                        }
                        else
                        {
                            addPermission.CustomerUserId = permission.CustomerUserId;
                            addPermission.CustomerPermissionId = permission.PermissionId;
                            addPermission.IsView = permission.IsView;
                            addPermission.IsEdit = permission.IsEdit;
                            addPermission.IsDownload = permission.IsDownload;
                            addPermission.IsReply = permission.IsReply;
                            addPermission.IsUpload = permission.IsUpload;
                        }
                    }
                }
            }
            if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
            {
                reGroup = await _db.CustomerGroups.Where(g => g.GroupLeaderId == CustomerUserIn.Id).FirstOrDefaultAsync(cancellationToken);
                if (reGroup != null)
                {
                    reGroup.GroupLeaderId = null;
                }
                CustomerDepartment.DepartmentMangerId = CustomerUserIn.Id;
            }
            else if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                reDepartment = await _db.CustomerDepartments.Where(d => d.DepartmentMangerId == CustomerUserIn.Id).FirstOrDefaultAsync(cancellationToken);
                if (reDepartment != null)
                {
                    reDepartment.DepartmentMangerId = null;
                }
                CustomerGroup.GroupLeaderId = CustomerUserIn.Id;
            }
            else if (CustomerUserIn.CustomerUserLevelId == (int)Shared.Enums.UserLevel.Staff)
            {
                reGroup = await _db.CustomerGroups.Where(g => g.GroupLeaderId == CustomerUserIn.Id).FirstOrDefaultAsync(cancellationToken);
                reDepartment = await _db.CustomerDepartments.Where(d => d.DepartmentMangerId == CustomerUserIn.Id).FirstOrDefaultAsync(cancellationToken);
                if (reGroup != null)
                {
                    reGroup.GroupLeaderId = null;
                }
                if (reDepartment != null)
                {
                    reDepartment.DepartmentMangerId = null;
                }
            }
        }
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new EditCustomerUserOut("User edited successfully"));
    }
}
