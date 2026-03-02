using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Core.HandleAccount.Commands.Login;

public class LoginIn
{
    public LoginIn()
    {
    }
        public LoginIn(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; } = string.Empty;
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}