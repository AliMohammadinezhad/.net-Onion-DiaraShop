using Framework.Infrastructure;
using ShopManagement.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EfCore.Repository;

public class SlideRepository : RepositoryBase<long, Slide>, ISlideRepository
{
    private readonly ApplicationDbContext _context;
    public SlideRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public EditSlide GetDetails(long id)
    {
        return _context.Slides.Select(x => new EditSlide
        {
            BtnText = x.BtnText,
            Id = x.Id,
            Heading = x.Heading,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Text = x.Text,
            Title = x.Title,
            Link = x.Link
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<SlideViewModel> GetList()
    {
        return _context.Slides.Select(x => new SlideViewModel
        {
            Title = x.Title,
            Heading = x.Heading,
            Picture = x.Picture,
            Id = x.Id,
            IsRemoved = x.IsRemoved,
            CreationDate = x.CreationDate.ToString(),
        }).OrderByDescending(x => x.Id).ToList();
    }
}