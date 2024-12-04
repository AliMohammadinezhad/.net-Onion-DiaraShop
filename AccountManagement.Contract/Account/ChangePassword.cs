using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Framework.Application;

namespace AccountManagement.Application.Contract.Account;

public class ChangePassword
{
    public long Id { get; init; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Password { get; init; }
    [Compare(nameof(Password), ErrorMessage = ValidationMessages.PasswordsNotMatch)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string NewPassword { get; init; }


    public override bool Equals(object? obj)
    {
        var passwordObj = obj as ChangePassword;

        return passwordObj != null && passwordObj.Password.Equals(passwordObj.NewPassword);
    }
}
