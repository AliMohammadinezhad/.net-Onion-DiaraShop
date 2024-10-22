using Framework.Application;
using ShopManagement.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application;

public class SlideApplication : ISlideApplication
{
    private readonly ISlideRepository _repository;

    public SlideApplication(ISlideRepository repository)
    {
        _repository = repository;
    }

    public OperationResult Create(CreateSlide command)
    {
        var operation = new OperationResult();
        var slide = new Slide(command.Picture, command.PictureAlt,command.PictureTitle, command.Heading,
            command.Title, command.Text, command.BtnText);
        if (_repository.Exists(x => x.Picture == command.Picture))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        _repository.Create(slide);
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditSlide command)
    {
        var operation = new OperationResult();
        var slide = _repository.Get(command.Id);
        if (slide == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        slide.Edit(command.Picture, command.PictureAlt, command.Title, command.Heading, command.Title, command.Text, command.BtnText);
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Remove(long id)
    {
        var operation = new OperationResult();
        var slide = _repository.Get(id);
        if (slide == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        slide.Remove();
        _repository.SaveChanges();
        return operation.Succeeded();

    }

    public OperationResult Restore(long id)
    {
        var operation = new OperationResult();
        var slide = _repository.Get(id);
        if (slide == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        slide.Restore();
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public EditSlide GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<SlideViewModel> GetList()
    {
        return _repository.GetList();
    }
}