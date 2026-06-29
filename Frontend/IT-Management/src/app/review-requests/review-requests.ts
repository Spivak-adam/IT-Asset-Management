import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { CheckoutRequest, CheckoutRequestStatus } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';
import { AuthService } from '../services/auth';
import { Asset } from '../models/it-asset.models';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-review-requests',
  imports: [CommonModule, FormsModule],
  templateUrl: './review-requests.html',
  styleUrl: './review-requests.css',
})
export class ReviewRequests implements OnInit {
  requests: CheckoutRequest[] = [];
  CheckoutRequestStatus = CheckoutRequestStatus;

  constructor(
    private api: ITAssetApi,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
  ) {}

  assetsByRequest: { [requestId: number]: Asset[] } = {};

  selectedAssetIds: { [requestId: number]: number } = {};

  ngOnInit() {
    this.loadRequests();
  }

  loadAvailableAssets(request: CheckoutRequest) {
    this.api.getAvailableAssetsByCategory(request.assetCategory).subscribe({
      next: (assets) => {
        this.assetsByRequest[request.id] = assets;

        if (assets.length > 0) {
          this.selectedAssetIds[request.id] = assets[0].id;
        }
        this.cdr.detectChanges();
      },
    });
  }

  loadRequests() {
    this.api.getCheckoutRequests().subscribe({
      next: (requests) => {
        this.requests = requests;

        this.pendingRequests.forEach((request) => {
          this.loadAvailableAssets(request);
        });

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  get pendingRequests() {
    return this.requests.filter((r) => r.status === CheckoutRequestStatus.Pending);
  }

  approveRequest(requestId: number, assetId: number) {
    const reviewedByUserId = this.authService.getUserId();

    this.api.approveCheckoutRequest(requestId, reviewedByUserId, assetId).subscribe({
      next: () => this.loadRequests(),
      error: (error) => console.error(error),
    });
  }

  rejectRequest(id: number) {
    const userId = this.authService.getUserId();
    this.api.rejectCheckoutRequest(id, userId).subscribe({
      next: () => this.loadRequests(),
      error: (error) => console.error('Failed to reject request:', error),
    });
  }
}
