import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Asset, AssetHistory } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';
import { AuthService } from '../services/auth';
import { AssetStatus, AssetCondition } from '../models/it-asset.models';
import { Checkout } from '../checkout/checkout';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-my-assets',
  imports: [CommonModule, FormsModule],
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
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.loadMyAssets();
  }

  loadMyAssets() {
    const userId = this.authService.getUserId();

    this.api.getMyAssets(userId).subscribe({
      next: (assets) => {
        this.assets = assets;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Failed to load my assets:', error);
      },
    });
  }

  requestReturn(assetId: number) {
    const userId = this.authService.getUserId();

    this.api.requestReturnByAsset(assetId, userId).subscribe({
      next: () => {
        this.closeDrawer();
        this.loadMyAssets();
      },
      error: (error) => console.error('Failed to request return:', error),
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
