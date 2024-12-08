using Framework.Infrastructure;
using static BlogManagement.Infrastructure.Configuration.Permissions.BlogPermissions;
namespace BlogManagement.Infrastructure.Configuration.Permissions;

public class BlogPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
            "Articles", [
                new PermissionDTO(ListArticles, "لیست مقاله ها"),
                new PermissionDTO(SearchArticles, "جست و جوی مقاله ها"),
                new PermissionDTO(CreateArticle, "ایجاد مقاله"),
                new PermissionDTO(EditArticles, "ویرایش مقاله ها")
            ]
            },
            {
                "Article Categories", [
                    new PermissionDTO(ListArticleCategories, "لیست گروه مقاله ها"),
                    new PermissionDTO(SearchArticleCategories, "جست و جوی گروه مقاله ها"),
                    new PermissionDTO(CreateArticleCategory, "ایجاد گروه مقاله"),
                    new PermissionDTO(EditArticleCategories, "ویرایش گروه مقاله ها")
                ]
            }
        };
    }
}