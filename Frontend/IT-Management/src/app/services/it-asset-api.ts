import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Asset, AssetHistory, CheckoutRequest, User, UserRole } from '../models/it-asset.models';

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

  getMyAssets(userId: number) {
    return this.http.get<Asset[]>(`${this.apiUrl}/my-assets/${userId}`);
  }

  getAssetHistory(assetId: number) {
    return this.http.get<AssetHistory[]>(`${this.apiUrl}/assets/${assetId}/history`);
  }

  getAvailableAssetsByCategory(category: string) {
    return this.http.get<Asset[]>(`${this.apiUrl}/assets/available/${category}`);
  }

  createCheckoutRequest(request: CreateCheckoutRequestDto) {
    return this.http.post<CheckoutRequest>(`${this.apiUrl}/checkout-requests`, request);
  }

  getCheckoutRequests() {
    return this.http.get<CheckoutRequest[]>(`${this.apiUrl}/checkout-requests`);
  }

  approveCheckoutRequest(id: number, reviewedByUserId: number, assignedAssetId: number) {
    return this.http.patch<CheckoutRequest>(`${this.apiUrl}/checkout-requests/${id}/approve`, {
      reviewedByUserId: reviewedByUserId,
      assignedAssetId,
    });
  }

  rejectCheckoutRequest(id: number, reviewedByUserId: number) {
    return this.http.patch<CheckoutRequest>(
      `${this.apiUrl}/checkout-requests/${id}/reject/${reviewedByUserId}`,
      {},
    );
  }

  getMyCheckoutRequests(userId: number) {
    return this.http.get<CheckoutRequest[]>(`${this.apiUrl}/checkout-requests/my/${userId}`);
  }

  requestReturn(requestId: number) {
    return this.http.patch<CheckoutRequest>(
      `${this.apiUrl}/checkout-requests/${requestId}/request-return`,
      {},
    );
  }

  requestReturnByAsset(assetId: number, userId: number) {
    return this.http.patch<CheckoutRequest>(
      `${this.apiUrl}/assets/${assetId}/request-return/${userId}`,
      {},
    );
  }

  returnAsset(requestId: number) {
    return this.http.patch<CheckoutRequest>(
      `${this.apiUrl}/checkout-requests/${requestId}/return`,
      {},
    );
  }

  archiveAsset(id: number) {
    return this.http.patch<Asset>(`${this.apiUrl}/assets/${id}/archive`, {});
  }

  restoreAsset(id: number) {
    return this.http.patch<Asset>(`${this.apiUrl}/assets/${id}/restore`, {});
  }

  assignAsset(id: number, userId: number) {
    return this.http.patch<Asset>(`${this.apiUrl}/assets/${id}/assign`, { userId });
  }

  returnAssetFromAdmin(id: number) {
    return this.http.patch<Asset>(`${this.apiUrl}/assets/${id}/return`, {});
  }

  getUsers() {
    return this.http.get<User[]>(`${this.apiUrl}/users`);
  }

  updateUserRole(id: number, role: UserRole) {
    return this.http.patch<User>(`${this.apiUrl}/users/${id}/role`, { role });
  }

  updateUserActive(id: number, isActive: boolean) {
    return this.http.patch<User>(`${this.apiUrl}/users/${id}/active`, { isActive });
  }
}
