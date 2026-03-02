using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;


namespace HospitioApi.Core.HandleUserAccount.Commands.CreateEditUserAccount;

public record CreateUserAccountRequest(CreateUserAccountIn In)
    : IRequest<AppHandlerResponse>;

public class CreateUserAccountHandler : IRequestHandler<CreateUserAccountRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;


    public CreateUserAccountHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
       )
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateUserAccountRequest request, CancellationToken cancellationToken)
    {
        var UserIn = request.In!;
        var upDepartment = new Department();
        var upGroup = new Group();

        upDepartment = await _db.Departments.Where(d => d.Id == UserIn.DepartmentId).FirstOrDefaultAsync(cancellationToken);
        upGroup = await _db.Groups.Where(g => g.Id == UserIn.GroupId).FirstOrDefaultAsync(cancellationToken);
        var users = await _db.Users.Where(s => s.Email == UserIn.Email || s.UserName == UserIn.Email).FirstOrDefaultAsync();

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
                var existDeptMgr = await _db.Users.Where(u => u.Id != UserIn.Id && u.DepartmentId == UserIn.DepartmentId && u.UserLevelId == UserIn.UserLevelId && u.IsActive == true).FirstOrDefaultAsync(cancellationToken);
                if (existDeptMgr != null)
                {
                    return _response.Error("Department manager of given department already exist.", AppStatusCodeError.Conflict409);
                }
                if (upDepartment!.DepartmentMangerId != null)
                {
                    return _response.Error("Department Manager of given department already exist.", AppStatusCodeError.Conflict409);
                }
            }
            else if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
            {
                var existGroupLeader = await _db.Users.Where(u => u.Id != UserIn.Id && u.GroupId == UserIn.GroupId && u.UserLevelId == UserIn.UserLevelId && u.IsActive == true).FirstOrDefaultAsync(cancellationToken);
                if (existGroupLeader != null)
                {
                    return _response.Error("Group leader of given group already exist.", AppStatusCodeError.Conflict409);
                }
                if (upGroup!.GroupLeaderId != null)
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
        if (await _db.Users.AnyAsync(s => s.Email == UserIn.Email, cancellationToken))
        {
            return _response.Error("Email already exits.", AppStatusCodeError.Conflict409);
        }
        if (await _db.Users.AnyAsync(s => s.UserName == UserIn.UserName, cancellationToken))
        {
            return _response.Error("Username already exits.", AppStatusCodeError.Conflict409);
        }
        if (await _db.Users.AnyAsync(s => s.Email == UserIn.UserName, cancellationToken))
        {
            return _response.Error("Email or username already exits.", AppStatusCodeError.Conflict409);
        }
        User user;
        user = new User
        {
            FirstName = UserIn.FirstName,
            LastName = UserIn.LastName,
            Email = UserIn.Email,
            UserName = UserIn.UserName,
            ProfilePicture = UserIn.ProfilePicture,
            PhoneNumber = UserIn.PhoneNumber,
            PhoneCountry = UserIn.PhoneCountry,
            SupervisorId = UserIn.SupervisorId,
            GroupId = UserIn.GroupId,
            DepartmentId = UserIn.DepartmentId,
            UserLevelId = UserIn.UserLevelId,
            Title = UserIn.Title,
            IsActive = UserIn.IsActive,

        };

        await _db.Users.AddAsync(user, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        user.Password = CryptoExtension.Encrypt(UserIn.Password, user.Id.ToString());
        _db.Users.Update(user);

        await _db.SaveChangesAsync(cancellationToken);

        if (UserIn.UserModulePermissions.Any())
        {
            var listOfUserPermission = new List<UsersPermission>();
            foreach (var permission in UserIn.UserModulePermissions)
            {
                UsersPermission addPermission = new();
                addPermission.UserId = user.Id;
                addPermission.PermissionId = permission.PermissionId;
                addPermission.IsView = permission.IsView;
                addPermission.IsEdit = permission.IsEdit;
                addPermission.IsSend = permission.IsSend;
                addPermission.IsReply = permission.IsReply;
                addPermission.IsUpload = permission.IsUpload;

                listOfUserPermission.Add(addPermission);
            }
            await _db.UsersPermissions.AddRangeAsync(listOfUserPermission, cancellationToken);
        }

        if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.DeptManager)
        {
            upDepartment.DepartmentMangerId = user.Id;
        }
        else if (UserIn.UserLevelId == (int)Shared.Enums.UserLevel.GroupLeader)
        {
            upGroup.GroupLeaderId = user.Id;
        }
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new CreateUserAccountOut("User added successfully"));
    }
}


