﻿using BlogManagement.Application.Contract.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Framework.Application;

namespace BlogManagement.Application;

public class ArticleCategoryApplication : IArticleCategoryApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IArticleCategoryRepository _articleCategoryRepository;

    public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository, IFileUploader fileUploader)
    {
        _articleCategoryRepository = articleCategoryRepository;
        _fileUploader = fileUploader;
    }

    public OperationResult Create(CreateArticleCategory command)
    {
        var operation = new OperationResult();
        if (_articleCategoryRepository.Exists(x => x.Name == command.Name))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Slug.Slugify();
        var filePath = _fileUploader.Upload(command.Picture, slug);
        var articleCategory = new ArticleCategory(command.Name, command.PictureAlt, command.PictureTitle, filePath, command.Description, command.ShowOrder,
            command.Slug, command.Keywords, command.MetaDescription, command.CanonicalAddress);

        _articleCategoryRepository.Create(articleCategory);
        _articleCategoryRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditArticleCategory command)
    {
        var operation = new OperationResult();
        var articleCategory = _articleCategoryRepository.Get(command.Id);

        if (articleCategory == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        if (_articleCategoryRepository.Exists(x => x.Name == command.Name && x.Id == command.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);


        var slug = command.Slug.Slugify();
        var filePath = _fileUploader.Upload(command.Picture, slug);
        articleCategory.Edit(command.Name, filePath, command.Description, command.ShowOrder, command.Slug,
            command.Keywords, command.MetaDescription, command.CanonicalAddress, command.PictureAlt, command.PictureTitle);

        _articleCategoryRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditArticleCategory GetDetails(long id)
    {
        return _articleCategoryRepository.GetDetails(id);
    }

    public List<ArticleCategoryViewModel> GetArticleCategories()
    {
        return _articleCategoryRepository.GetArticleCategories();
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
    {
        return _articleCategoryRepository.Search(searchModel);
    }
}