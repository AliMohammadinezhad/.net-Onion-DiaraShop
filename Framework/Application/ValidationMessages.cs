namespace Framework.Application;

public static class ValidationMessages
{
    public const string IsRequired = "این مقدار نمی تواند خالی باشد.";
    public const string IsPositive = "این مقدار نمی تواند منفی باشد.";
    public const string IsMaxFileSize = "فایل حجیم تر از حد مجاز است.";
    public const string InvalidFileFormat = "فرمت فایل مجاز نیست.";
    public const string PasswordsNotMatch = "رمز عبور وارد شده مطابقت ندارد.";
    public const string InvalidMobileFormat = "شماره موبایل وارد شده درست نیست.";
}