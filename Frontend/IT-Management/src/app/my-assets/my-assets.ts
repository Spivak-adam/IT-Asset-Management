import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Asset, AssetHistory } from '../models/it-asset.models';
import { ITAssetApi, CreateCheckoutRequestDto } from '../services/it-asset-api';
import { AuthService } from '../services/auth';

@Component({
  selector: 'app-my-assets',
  imports: [CommonModule, FormsModule],
  templateUrl: './my-assets.html',
  styleUrl: './my-assets.css',
})
export class MyAssets implements OnInit {
  assets: Asset[] = [];
  selectedAsset: Asset | null = null;
  assetHistory: AssetHistory[] = [];

  drawerOpen = false;
  requestReason = '';
  successMessage = '';
  errorMessage = '';

  constructor(private api: ITAssetApi, private authService: AuthService) {}

  ngOnInit() {
    this.getAssets();
  }

  getAssets() {
    this.api.getAssets().subscribe({
      next: (assets) => {
        this.assets = assets;
      },
      error: (error) => {
        console.error('Failed to load assets:', error);
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

  requestCheckout() {
    if (!this.selectedAsset) return;

    const request: CreateCheckoutRequestDto = {
      requestedByUserId: this.authService.getUserId(),
      assetCategory: this.selectedAsset.category,
      reason: this.requestReason,
    };

    this.api.createCheckoutRequest(request).subscribe({
      next: () => {
        this.successMessage = 'Checkout request submitted successfully.';
        this.errorMessage = '';
      },
      error: (error) => {
        console.error('Failed to create checkout request:', error);
        this.errorMessage = 'Failed to submit checkout request.';
        this.successMessage = '';
      },
    });
  }
}
