namespace IT_Asset.DTOs;

public class ApproveCheckoutRequestDto
{
    public int ReviewedByUserId { get; set; }
    public int AssignedAssetId { get; set; }
}