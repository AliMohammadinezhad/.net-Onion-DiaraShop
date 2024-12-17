using BlogManagement.Infrastructure.EfCore;
using CommentManagement.Infrastructure.EfCore;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Query.Contracts.Article;
using Query.Contracts.Comment;

namespace Query.Query;

public class ArticleQuery : IArticleQuery
{
    private readonly BlogContext _context;
    private readonly CommentContext _commentContext;
    public ArticleQuery(BlogContext context, CommentContext commentContext)
    {
        _context = context;
        _commentContext = commentContext;
    }

    public ArticleQueryModel GetArticleDetails(string slug)
    {
        var article = _context.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Id = x.Id,
                Slug = x.Slug,
                Picture = x.Picture,
                CategorySlug = x.Category.Slug,
                CanonicalAddress = x.CanonicalAddress,
                CategoryName = x.Category.Name,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
                Title = x.Title,
            }).FirstOrDefault(x => x.Slug == slug);

        if (!string.IsNullOrWhiteSpace(article?.Keywords))
            article.KewordList = article.Keywords.Split("،").ToList();





        // Always populate the comments property
        var comments = _commentContext.Comments
            .Where(x => x.IsConfirmed == true && x.IsCancelled == false)
            .Where(x => x.OwnerRecordId == article.Id)
            .Where(x => x.Type == CommentType.Article)
            .Select(x => new CommentQueryModel
            {
                Id = x.Id,
                Message = x.Message,
                Name = x.Name,
                ParentId = x.ParentId,
                CreationDate = x.CreationDate.ToFarsi(),
                Children = new List<CommentQueryModel>(),
            })
            .OrderByDescending(x => x.Id)
            .ToList();

        // get count of total comments (both parent and child)
        article.TotalCommentCount = comments.Count;

        // Build parent-child relationships and sort children in ascending order
        var commentsDictionary = comments.ToDictionary(c => c.Id, c => c);

        foreach (var comment in comments)
        {
            if (comment.ParentId is > 0 && commentsDictionary.ContainsKey(comment.ParentId.Value))
            {
                var parent = commentsDictionary[comment.ParentId.Value];
                parent.Children.Add(comment);
            }
        }

        // Sort children in ascending order
        foreach (var comment in comments)
        {
            comment.Children = comment.Children.OrderBy(c => c.Id).ToList();
        }

        // Pass only top-level comments to the article
        article.Comments = comments.Where(c => c.ParentId is null or 0).ToList();
        return article;
    }

    public List<ArticleQueryModel> LatestArticles()
    {
        return _context.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
                Title = x.Title
            }).ToList();
    }
}