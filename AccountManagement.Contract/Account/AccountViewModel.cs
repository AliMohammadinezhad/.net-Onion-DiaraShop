namespace AccountManagement.Application.Contract.Account;

public class AccountViewModel
{
    public long Id { get; init; }
    public string FullName { get; init; }
    public string Username { get; init; }
    public string Mobile { get; init; }
    public string Role { get; init; }
    public long RoleId { get; init; }
    public string ProfilePhoto { get; init; }
    public string CreationDate { get; init; }
    public DateTime CreationDateCalculation { get; set; }
}