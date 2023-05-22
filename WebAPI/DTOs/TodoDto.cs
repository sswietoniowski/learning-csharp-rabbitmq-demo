namespace Web.UI.DTOs;

public record TodoDto(String Id, string Title, bool IsCompleted = false);