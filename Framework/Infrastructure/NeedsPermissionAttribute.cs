namespace Framework.Infrastructure;


public class NeedsPermissionAttribute : Attribute
{
    public int Permissions { get; set; }

    public NeedsPermissionAttribute(int permissions)
    {
        Permissions = permissions;
    }
}