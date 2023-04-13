namespace MvcLibrary.Models;

public class Library
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? AlbumName { get; set; }
    public string? AlbumImage { get; set; }
    public int Originality { get; set; }
    public int Taste { get; set; }
}