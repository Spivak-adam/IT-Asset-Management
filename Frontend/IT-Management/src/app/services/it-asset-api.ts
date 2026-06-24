import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Asset, CheckoutRequest } from '../models/it-asset.models';

@Injectable({
  providedIn: 'root',
})
export class ITAssetApi {
  private apiUrl = 'http://localhost:5058/api/ITAsset';

  constructor(private http: HttpClient) {}

  getAssets() {
    return this.http.get<Asset[]>(`${this.apiUrl}/assets`);
  }

  getCheckoutRequests() {
    return this.http.get<CheckoutRequest[]>(`${this.apiUrl}/checkout-requests`);
  }

  
}