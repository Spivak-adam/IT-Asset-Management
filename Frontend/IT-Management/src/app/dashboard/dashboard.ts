import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ITAssetApi } from '../services/it-asset-api';
import { AuthService } from '../services/auth';
import {
  Asset,
  CheckoutRequest,
  AssetStatus,
  CheckoutRequestStatus
} from '../models/it-asset.models';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  role: string | null = null;
  userId = 0;

  assets: Asset[] = [];
  requests: CheckoutRequest[] = [];

  AssetStatus = AssetStatus;
  CheckoutRequestStatus = CheckoutRequestStatus;

  constructor(
    private api: ITAssetApi,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.role = this.authService.getRole();
    this.userId = this.authService.getUserId();

    this.loadDashboardData();
  }

  loadDashboardData() {
    this.api.getAssets().subscribe({
      next: (assets) => {
        this.assets = assets;
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Failed to load assets', error),
    });

    this.api.getCheckoutRequests().subscribe({
      next: (requests) => {
        this.requests = requests;
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Failed to load requests', error),
    });
  }

  get myAssignedAssets() {
    return this.assets.filter(a => a.assignedToUserId === this.userId);
  }

  get availableAssets() {
    return this.assets.filter(a => a.status === AssetStatus.Available && !a.isArchived);
  }

  get assignedAssets() {
    return this.assets.filter(a => a.status === AssetStatus.Assigned);
  }

  get maintenanceAssets() {
    return this.assets.filter(a => a.status === AssetStatus.Maintenance);
  }

  get retiredAssets() {
    return this.assets.filter(a => a.status === AssetStatus.Retired);
  }

  get myRequests() {
    return this.requests.filter(r => r.requestedByUserId === this.userId);
  }

  get myPendingRequests() {
    return this.myRequests.filter(r => r.status === CheckoutRequestStatus.Pending);
  }

  get pendingRequests() {
    return this.requests.filter(r => r.status === CheckoutRequestStatus.Pending);
  }

  get approvedRequests() {
    return this.requests.filter(r => r.status === CheckoutRequestStatus.Approved);
  }

  get fulfilledRequests() {
    return this.requests.filter(r => r.status === CheckoutRequestStatus.Fulfilled);
  }
}