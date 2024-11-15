﻿using Framework.Application;

namespace ShopManagement.Contracts.Comment;

public interface ICommentApplication
{
    OperationResult Add(AddComment command);
    OperationResult Confirm(long id);
    OperationResult Cancel(long id);
    List<CommentViewModel> Search(CommentSearchModel  searchModel);
}