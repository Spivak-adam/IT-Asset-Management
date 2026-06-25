import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Asset, AssetHistory, CheckoutRequest } from '../models/it-asset.models';

export interface CreateCheckoutRequestDto {
  requestedByUserId: number;
  assetCategory: string;
  reason: string;
}

@Injectable({
  providedIn: 'root',
})
export class ITAssetApi {
  private apiUrl = 'http://localhost:5058/api/ITAsset';

  constructor(private http: HttpClient) {}

  getAssets() {
    return this.http.get<Asset[]>(`${this.apiUrl}/assets`);
  }

   getAssetHistory(assetId: number) {
    return this.http.get<AssetHistory[]>(`${this.apiUrl}/assets/${assetId}/history`);
  }

  createCheckoutRequest(request: CreateCheckoutRequestDto) {
    return this.http.post<CheckoutRequest>(
      `${this.apiUrl}/checkout-requests`,
      request
    );
  }

  
}