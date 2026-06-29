import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Asset, AssetStatus, AssetCondition, User } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';

@Component({
  selector: 'app-assets',
  imports: [CommonModule, FormsModule],
  templateUrl: './assets.html',
  styleUrl: './assets.css',
})
export class Assets implements OnInit {
  assets: Asset[] = [];
  users: User[] = [];

  selectedUserIds: { [assetId: number]: number } = {};

  AssetStatus = AssetStatus;
  AssetCondition = AssetCondition;

  constructor(
    private api: ITAssetApi,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadAssets();
    this.loadUsers();
  }

  loadAssets() {
    this.api.getAssets().subscribe({
      next: assets => {
        this.assets = assets;
        this.cdr.detectChanges();
      },
      error: error => console.error('Failed to load assets:', error),
    });
  }

  loadUsers() {
    this.api.getUsers().subscribe({
      next: users => {
        this.users = users;
        this.cdr.detectChanges();
      },
      error: error => console.error('Failed to load users:', error),
    });
  }

  archiveAsset(id: number) {
    this.api.archiveAsset(id).subscribe({
      next: () => this.loadAssets(),
      error: error => console.error('Failed to archive asset:', error),
    });
  }

  assignAsset(assetId: number) {
    const userId = this.selectedUserIds[assetId];

    if (!userId) return;

    this.api.assignAsset(assetId, userId).subscribe({
      next: () => this.loadAssets(),
      error: error => console.error('Failed to assign asset:', error),
    });
  }

  returnAsset(assetId: number) {
    this.api.returnAssetFromAdmin(assetId).subscribe({
      next: () => this.loadAssets(),
      error: error => console.error('Failed to return asset:', error),
    });
  }
}