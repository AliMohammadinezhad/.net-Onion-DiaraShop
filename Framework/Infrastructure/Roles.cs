namespace Framework.Infrastructure;

public static class Roles
{
    public const string Admin = "1";
    public const string User = "2";
    public const string ContentCreator = "3";
    public const string Colleague = "4";
    public static string GetRoleBy(long id)
    {
        return id switch
        {
            1 => "مدیر سیستم",
            3 => "محتوا گذار",
            _ => ""
        };
    }
}