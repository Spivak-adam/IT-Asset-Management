namespace IT_Asset.Models;

public class AssetHistory
{
    public int Id { get; set; }

    public int AssetId { get; set; }

    public Asset Asset { get; set; }

    public int? UserId { get; set; }

    public User? User { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public DateTime CreatedAt { get; set; }
}