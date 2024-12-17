using Microsoft.EntityFrameworkCore;
using Query.Contracts.Slide;
using ShopManagement.Infrastructure.EfCore;

namespace Query.Query;

public class SlideQuery : ISlideQuery
{
    private readonly ApplicationDbContext _shopContext;

    public SlideQuery(ApplicationDbContext shopContext)
    {
        _shopContext = shopContext;
    }

    public List<SlideQueryModel> GetSlides()
    {
        return _shopContext.Slides
            .Where(x => x.IsRemoved == false)
            .Select(x => new SlideQueryModel()
            {
                BtnText = x.BtnText,
                Heading = x.Heading,
                Link = x.Link,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Text = x.Text,
                Title = x.Title
            }).AsNoTracking().ToList();
    }
}