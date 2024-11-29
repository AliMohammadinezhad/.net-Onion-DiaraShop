namespace Query.Contracts.Comment;

public class CommentQueryModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Message { get; set; }
    public string CreationDate { get; set; }
    public long? ParentId { get; set; }
    public List<CommentQueryModel> Children { get; set; }
}