namespace Query.Contracts.Product;

public class CommentQueryModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Message { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsCancelled { get; set; }
}