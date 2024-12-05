using System.ComponentModel.DataAnnotations;
using AccountManagement.Application.Contract.Role;
using Framework.Application;
using Microsoft.AspNetCore.Http;

namespace AccountManagement.Application.Contract.Account;

public class RegisterAccount
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string FullName { get;  init; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Username { get;  init; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Password { get;  init; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [RegularExpression(@"^(098|0098|98|\+98|0){1}9(0[0-5]|[1 3]\d|2[0-3]|9[0-9]|41)\d{7}$", ErrorMessage = ValidationMessages.InvalidMobileFormat)]
    public string Mobile { get;  init; }

    [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
    public long RoleId { get;  init; }
    public IFormFile? ProfilePhoto { get;  init; }
    public List<RoleViewModel> Roles { get; set; }


    public override bool Equals(object? obj)
    {
        var account = obj as RegisterAccount;
        if (account == null)
            return false;
        
        return Mobile.ToEnglishNumber().Equals(account.Mobile.ToEnglishNumber());
    }
}