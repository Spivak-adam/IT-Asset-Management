import { Component, OnInit } from '@angular/core';
import { Asset } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  constructor(private api: ITAssetApi) {}
  assets: Asset[] = [];

  ngOnInit() {
    this.getAssets();
  }

  getAssets() {
  this.api.getAssets().subscribe({
    next: (assets) => {
      this.assets = assets;
      console.log('Assets loaded:', assets);
    },
    error: (error) => {
      console.error('Failed to load assets:', error);
    }
  });
}
}
