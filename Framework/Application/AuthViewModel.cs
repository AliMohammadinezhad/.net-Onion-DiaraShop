namespace Framework.Application;

public class AuthViewModel
{
    public long AccountId { get; set; }
    public string Role { get; set; }
    public long RoleId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Mobile { get; set; }
    public List<int> Permissions { get; set; }

    public AuthViewModel()
    {
    }
    public AuthViewModel(long accountId, long roleId, string fullName, string username, string mobile, List<int> permissions)
    {
        Mobile = mobile;
        AccountId = accountId;
        RoleId = roleId;
        FullName = fullName;
        Username = username;
        Permissions = permissions;
    }
}