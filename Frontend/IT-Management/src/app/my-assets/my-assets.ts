import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Asset, AssetHistory } from '../models/it-asset.models';
import { ITAssetApi, CreateCheckoutRequestDto } from '../services/it-asset-api';
import { AuthService } from '../services/auth';
import { AssetStatus, AssetCondition } from '../models/it-asset.models';
import { Checkout } from '../checkout/checkout';

@Component({
  selector: 'app-my-assets',
  imports: [CommonModule, FormsModule, Checkout],
  templateUrl: './my-assets.html',
  styleUrl: './my-assets.css',
})
export class MyAssets implements OnInit {
  AssetStatus = AssetStatus;
  AssetCondition = AssetCondition;

  assets: Asset[] = [];
  selectedAsset: Asset | null = null;
  assetHistory: AssetHistory[] = [];

  drawerOpen = false;
  requestReason = '';
  successMessage = '';
  errorMessage = '';

  historyPopupOpen = false;

  constructor(
    private api: ITAssetApi,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  viewMode: 'my-assets' | 'check-assets' = 'my-assets';

ngOnInit() {
  this.loadMyAssets();
}

loadMyAssets() {
  const userId = this.authService.getUserId();

  this.api.getAssets().subscribe({
    next: (assets) => {
      this.assets = assets.filter(asset => asset.assignedToUserId === userId);
      this.viewMode = 'my-assets';
      this.cdr.detectChanges();
    },
    error: (error) => {
      console.error('Failed to load my assets:', error);
    },
  });
}

loadCheckAssets() {
  this.api.getAssets().subscribe({
    next: (assets) => {
      this.assets = assets.filter(asset =>
        asset.status === AssetStatus.Available &&
        asset.isArchived === false
      );

      this.viewMode = 'check-assets';
      this.cdr.detectChanges();
    },
    error: (error) => {
      console.error('Failed to load requestable assets:', error);
    },
  });
}

  openStatusDrawer(asset: Asset) {
    this.selectedAsset = asset;
    this.drawerOpen = true;
    this.requestReason = '';
    this.successMessage = '';
    this.errorMessage = '';
    this.getAssetHistory(asset.id);
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedAsset = null;
    this.assetHistory = [];
    this.cdr.detectChanges();
  }

  openHistoryPopup() {
    this.historyPopupOpen = true;
  }

  closeHistoryPopup() {
    this.historyPopupOpen = false;
  }

  getAssetHistory(assetId: number) {
    this.api.getAssetHistory(assetId).subscribe({
      next: (history) => {
        this.assetHistory = history;
      },
      error: (error) => {
        console.error('Failed to load asset history:', error);
        this.assetHistory = [];
      },
    });
  }

  
}
