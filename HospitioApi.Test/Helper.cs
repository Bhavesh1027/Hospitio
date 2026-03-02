using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace HospitioApi.Test;
public static class Helper
{
    /* All methods will be use for mocking the inbulit function of Identity */

    public static Mock<RoleManager<TIdentityRole>> GetRoleManagerMock<TIdentityRole>() where TIdentityRole : IdentityRole<string>
    {
        return new Mock<RoleManager<TIdentityRole>>(
                new Mock<IRoleStore<TIdentityRole>>().Object,
                new IRoleValidator<TIdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<TIdentityRole>>>().Object);
    }
    public static Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser<string>
    {
        return new Mock<UserManager<TIDentityUser>>(
                new Mock<IUserStore<TIDentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<TIDentityUser>>().Object,
                new IUserValidator<TIDentityUser>[0],
                new IPasswordValidator<TIDentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
    }
    public static Mock<SignInManager<TIDentityUser>> GetSignInManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser<string>
    {
        return new Mock<SignInManager<TIDentityUser>>(
            new Mock<IUserStore<TIDentityUser>>(),
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<TIDentityUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<TIDentityUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<TIDentityUser>>().Object);
    }
}
