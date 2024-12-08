using Framework.Infrastructure;
using static DiscountManagement.infrastructure.Configuration.Permissions.CommentPermissions;
namespace CommentManagement.Infrastructure.Configuration.Permissions;

public class CommentPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
            "Comments", [
                new PermissionDTO(ListComments, "لیست کامنت ها"),
                new PermissionDTO(SearchComments, "جست و جوی کامنت ها"),
                new PermissionDTO(CreateComment, "ایجاد کامنت"),
                new PermissionDTO(EditComments, "ویرایش کامنت ها"),
                new PermissionDTO(ConfirmComment, "تایید کردن کامنت"),
                new PermissionDTO(CancelComment, "کنسل کردن کامنت"),
            ]
        }
        };
    }
}