namespace Web.UI.DTOs;

public class TodoDto
{
    public String Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public bool IsCompleted { get; set; } = false;
}