import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Asset, AssetCondition, AssetStatus } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';

@Component({
  selector: 'app-assets',
  imports: [CommonModule],
  templateUrl: './assets.html',
  styleUrl: './assets.css',
})
export class Assets implements OnInit {
  assets: Asset[] = [];

  AssetStatus = AssetStatus;
  AssetCondition = AssetCondition;

  constructor(private api: ITAssetApi) {}

  ngOnInit() {
    this.loadAssets();
  }

  loadAssets() {
    this.api.getAssets().subscribe({
      next: (assets) => {
        this.assets = assets;
      },
      error: (error) => {
        console.error('Failed to load assets:', error);
      },
    });
  }
}