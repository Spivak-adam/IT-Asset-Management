public class Asset
{
    public int Id { get; set; }

    public string AssetTag { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string SerialNumber { get; set; } = string.Empty;

    public AssetStatus Status { get; set; }

    public AssetCondition Condition { get; set; }

    public int? AssignedToUserId { get; set; }

    public User? AssignedToUser { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}