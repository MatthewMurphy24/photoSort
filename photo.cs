public class Photo
{
    public string FileName { get; set; } = "";
    public string UniqueId { get; set; } = Guid.NewGuid().ToString();
    public string? Location { get; set; }
    public DateTime? DateTimeOriginal { get; set; }
    public string? Model { get; set; }
    public bool IsSelfie { get; set; } = false;
    public bool IsScreenshot { get; set; } = false;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}