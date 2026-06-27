namespace IT_Asset.DTOs;

public class CreateCheckoutRequestDto
{
    public int RequestedByUserId { get; set; }

    public int RequestedAssetId { get; set; }

    public string AssetCategory { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;
}