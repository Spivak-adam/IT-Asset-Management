import { Component, Input, ChangeDetectorRef} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Asset } from '../models/it-asset.models';
import { ITAssetApi, CreateCheckoutRequestDto } from '../services/it-asset-api';
import { AuthService } from '../services/auth';

@Component({
  selector: 'app-checkout',
  imports: [FormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout {
  @Input() selectedAsset: Asset | null = null;

  requestReason = '';
  successMessage = '';
  errorMessage = '';

  constructor(
    private api: ITAssetApi,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  Checkout() {
    if (!this.selectedAsset) return;

    const request: CreateCheckoutRequestDto = {
      requestedByUserId: this.authService.getUserId(),
      requestedAssetId: this.selectedAsset.id,
      assetCategory: this.selectedAsset.category,
      reason: this.requestReason,
    };

    this.api.createCheckoutRequest(request).subscribe({
      next: () => {
        this.successMessage = 'Checkout request submitted successfully.';
        this.errorMessage = '';
        this.requestReason = '';
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Failed to create checkout request:', error);
        this.errorMessage = 'Failed to submit checkout request.';
        this.successMessage = '';
        this.cdr.detectChanges();
      },
    });
  }
}
