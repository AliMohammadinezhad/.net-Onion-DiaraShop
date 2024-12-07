namespace Framework.Infrastructure;


public interface IPermissionExposer
{
    Dictionary<string, List<PermissionDTO>> Expose();
}