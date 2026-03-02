using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;


namespace HospitioApi.Core.HandleUserAccount.Commands.EditUserAccount;
public record EditUserAccountRequest(EditUserAccountIn In)
    : IRequest<AppHandlerResponse>;

public class EditUserAccountHandler : IRequestHandler<EditUserAccountRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;


    public EditUserAccountHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
       )
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(EditUserAccountRequest request, CancellationToken cancellationToken)
    {
        var UserIn = request.In!;
        var upDepartment = new Department();
        var upGroup = new Data.Models.Group();
        var reDepartment = new Department();
        var reGroup = new Data.Models.Group();

        upDepartment = await _db.Departments.Where(d => d.Id == UserIn.DepartmentId).FirstOrDefaultAsync(cancellationToken);
        upGroup = await _db.Groups.Where(g => g.Id == UserIn.GroupId).FirstOrDefaultAsync(cancellationToken);


        if (upDepartment == null && UserIn.UserLevelId != (int)Shared.Enums.UserLevel.CEO)
        {
            return _response.Error("Department not exist.", AppStatusCodeError.Conflict409);
        }
        if (upGroup == null && (UserIn.UserLevelId != (int)Shared.Enums.UserLevel.DeptManager && UserIn.UserLevelId != (int)Shared.Enums.UserLevel.CEO))
        {
            return _response.Error("Group not exist .", AppStatusCodeError.Conflict409);
        }


        //validate in user table
        if (UserIn.UserLevelId != (int)Shared.Enums.UserLevel.Staff || UserIn.UserLevelId == (int)Shared.Enums.UserLevel.CEO)
        {
            if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
            {
                var existDeptMgr = await _db.Users.Where(u => u.Id != UserIn.Id && u.DepartmentId == UserIn.DepartmentId && u.UserLevelId == UserIn.UserLevelId && u.IsActive == true).SingleOrDefaultAsync(cancellationToken);
                if (existDeptMgr != null)
                {
                    return _response.Error("Department manager of given department already exist.", AppStatusCodeError.Conflict409);
                }
            }
            else if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                var existGroupLeader = await _db.Users.Where(u => u.Id != UserIn.Id && u.GroupId == UserIn.GroupId && u.UserLevelId == UserIn.UserLevelId && u.IsActive == true).SingleOrDefaultAsync(cancellationToken);
                if (existGroupLeader != null)
                {
                    return _response.Error("Group leader of given group already exist.", AppStatusCodeError.Conflict409);
                }
            }
            else if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.CEO)
            {
                var existCeo = await _db.Users.Where(u => u.Id != UserIn.Id && u.UserLevelId == UserIn.UserLevelId && u.IsActive == true).FirstOrDefaultAsync(cancellationToken);
                if (existCeo != null)
                {
                    return _response.Error("CEO already exist.", AppStatusCodeError.Conflict409);
                }
            }
        }



        //// Update Dept/Group table
        //if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
        //{
        //    //upDepartment = await _db.Departments.Where(d => d.Id == UserIn.DepartmentId && d.DepartmentMangerId == null).SingleOrDefaultAsync(cancellationToken);
        //    if (upDepartment!.DepartmentMangerId != null)
        //    {
        //        return _response.Error("Department Manager of given department already exist.", AppStatusCodeError.Conflict409);
        //    }
        //}
        //else if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
        //{
        //    if (upGroup!.GroupLeaderId != null)
        //    {
        //        return _response.Error("Group Leader of given group already exist.", AppStatusCodeError.Conflict409);
        //    }
        //}

        var editUser = await _db.Users.Include(p => p.UsersPermissions).Where(u => u.Id == UserIn.Id).FirstOrDefaultAsync(cancellationToken);

        if (editUser == null)
        {
            return _response.Error("Current User does not exist.", AppStatusCodeError.UnprocessableEntity422);
        }
        else
        {
            if (editUser.UserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
            {
                var FindDepartment = await _db.Departments.Where(d => d.DepartmentMangerId == editUser.Id).ToListAsync(cancellationToken);

                if (FindDepartment != null || FindDepartment.Count != 0)
                {
                    foreach(var departmentData in FindDepartment)
                    {
                        departmentData.DepartmentMangerId = null;
                    }
                }
            }
            if (editUser.UserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                var FindGroup = await _db.Groups.Where(g => g.GroupLeaderId == editUser.Id).ToListAsync(cancellationToken);

                if (FindGroup != null || FindGroup.Count != 0)
                {
                    foreach (var departmentData in FindGroup)
                    {
                        departmentData.GroupLeaderId = null;
                    }
                }
            }

            if (!string.IsNullOrEmpty(UserIn.Password))
                editUser.Password = CryptoExtension.Encrypt(UserIn.Password, editUser.Id.ToString());

            editUser.FirstName = UserIn.FirstName;
            editUser.LastName = UserIn.LastName;
            editUser.Email = UserIn.Email;
            editUser.UserName = UserIn.UserName;
            editUser.ProfilePicture = UserIn.ProfilePicture;
            editUser.PhoneNumber = UserIn.PhoneNumber;
            editUser.PhoneCountry = UserIn.PhoneCountry;
            editUser.SupervisorId = UserIn.SupervisorId;
            editUser.GroupId = UserIn.GroupId;
            editUser.DepartmentId = UserIn.DepartmentId;
            editUser.UserLevelId = UserIn.UserLevelId;
            editUser.Title = UserIn.Title;
            editUser.IsActive = UserIn.IsActive;

            //update user module permissions

            if (UserIn.UserModulePermissions is not null)
            {
                //if (editUser.UsersPermissions != null)
                //{
                //    while (editUser.UsersPermissions.Count > 0)
                //    {
                //        var sk = editUser.UsersPermissions.Last();
                //        editUser.UsersPermissions.Remove(sk);
                //        _db.UsersPermissions.Remove(sk);
                //    }
                //}
                if (UserIn.UserModulePermissions.Any())
                {
                    //editUser.UsersPermissions = new List<UsersPermission>();

                    foreach (var permission in UserIn.UserModulePermissions)
                    {

                        var addPermission = await _db.UsersPermissions.Where(u => u.Id == permission.Id).FirstOrDefaultAsync(cancellationToken);

                        if (addPermission == null)
                        {
                            UsersPermission addPermissionNew = new();
                            addPermissionNew.UserId = UserIn.Id;
                            addPermissionNew.PermissionId = permission.PermissionId;
                            addPermissionNew.IsView = permission.IsView;
                            addPermissionNew.IsEdit = permission.IsEdit;
                            addPermissionNew.IsSend = permission.IsSend;
                            addPermissionNew.IsReply = permission.IsReply;
                            addPermissionNew.IsUpload = permission.IsUpload;
                            await _db.UsersPermissions.AddAsync(addPermissionNew, cancellationToken);
                        }
                        else
                        {
                            addPermission.UserId = permission.UserId;
                            addPermission.PermissionId = permission.PermissionId;
                            addPermission.IsView = permission.IsView;
                            addPermission.IsEdit = permission.IsEdit;
                            addPermission.IsSend = permission.IsSend;
                            addPermission.IsReply = permission.IsReply;
                            addPermission.IsUpload = permission.IsUpload;
                        }
                        //await _db.Users.AddAsync(addPermission, cancellationToken);
                        //editUser.UsersPermissions.Add(addPermission);
                    }
                }
            }
            if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
            {
                reGroup = await _db.Groups.Where(g => g.GroupLeaderId == UserIn.Id).FirstOrDefaultAsync(cancellationToken);
                if (reGroup != null)
                {
                    reGroup.GroupLeaderId = null;
                }
                upDepartment.DepartmentMangerId = UserIn.Id;
            }
            else if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                reDepartment = await _db.Departments.Where(d => d.DepartmentMangerId == UserIn.Id).FirstOrDefaultAsync(cancellationToken);
                if(reDepartment != null)
                {
                    reDepartment.DepartmentMangerId = null;
                }
                upGroup.GroupLeaderId = UserIn.Id;
            }else if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.Staff)
            {
                reGroup = await _db.Groups.Where(g => g.GroupLeaderId == UserIn.Id).FirstOrDefaultAsync(cancellationToken);
                reDepartment = await _db.Departments.Where(d => d.DepartmentMangerId == UserIn.Id).FirstOrDefaultAsync(cancellationToken);
                if (reGroup != null)
                {
                    reGroup.GroupLeaderId = null;
                }
                if(reDepartment != null)
                {
                    reDepartment.DepartmentMangerId = null;
                }
            }
            //await _db.Users.AddAsync(editUser, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return _response.Success(new EditUserAccountOut("User edited successfully"));
        }
    }
}
