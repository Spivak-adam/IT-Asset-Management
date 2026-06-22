using IT_Asset.Enums;

namespace IT_Asset.Models;

public class CheckoutRequest
{
    public int Id { get; set; }

    public int RequestedByUserId { get; set; }

    public User RequestedByUser { get; set; }

    public string AssetCategory { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public CheckoutRequestStatus Status { get; set; }

    public int? ReviewedByUserId { get; set; }

    public User? ReviewedByUser { get; set; }

    public int? AssignedAssetId { get; set; }

    public Asset? AssignedAsset { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public DateTime? FulfilledAt { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}