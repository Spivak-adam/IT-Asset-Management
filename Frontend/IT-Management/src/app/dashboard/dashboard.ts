import { Component, OnInit, ChangeDetectorRef  } from '@angular/core';
import { Asset } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';


@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  assets: Asset[] = [];

  constructor(
    private api: ITAssetApi,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.getAssets();
  }

  getAssets() {
    this.api.getAssets().subscribe({
      next: (assets) => {
        this.assets = assets;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Failed to load assets:', error);
      }
    });
  }
}
