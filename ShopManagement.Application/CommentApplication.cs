﻿using Framework.Application;
using ShopManagement.Contracts.Comment;
using ShopManagement.Domain.CommentAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application;

public class CommentApplication : ICommentApplication
{
    private readonly ICommentRepository _commentRepository;

    public CommentApplication(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public OperationResult Add(AddComment command)
    {
        var operation = new OperationResult();
        var comment = new Comment(command.Name, command.Email, command.Message, command.ProductId);
        
        _commentRepository.Create(comment);
        _commentRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Confirm(long id)
    {
        var operation = new OperationResult();
        var comment = _commentRepository.Get(id);

        if (comment == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);


        comment?.Confirm();
        _commentRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Cancel(long id)
    {
        var operation = new OperationResult();
        var comment = _commentRepository.Get(id);

        if (comment == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);


        comment?.Cancel();
        _commentRepository.SaveChanges();
        return operation.Succeeded();
    }

    public List<CommentViewModel> Search(CommentSearchModel searchModel)
    {
        return _commentRepository.Search(searchModel);
    }
}