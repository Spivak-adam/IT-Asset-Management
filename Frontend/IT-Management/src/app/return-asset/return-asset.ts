import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutRequest, CheckoutRequestStatus } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';

@Component({
  selector: 'app-return-asset',
  imports: [CommonModule],
  templateUrl: './return-asset.html',
  styleUrl: './return-asset.css',
})
export class ReturnAsset implements OnInit {
  requests: CheckoutRequest[] = [];
  CheckoutRequestStatus = CheckoutRequestStatus;

  constructor(
    private api: ITAssetApi,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadRequests();
  }

  loadRequests() {
    this.api.getCheckoutRequests().subscribe({
      next: (requests) => {
        this.requests = requests;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Failed to load return requests:', error);
      },
    });
  }

  get returnableRequests() {
    return this.requests.filter(
      request => request.status === CheckoutRequestStatus.ReturnRequested
    );
  }

  approveReturn(requestId: number) {
  this.api.returnAsset(requestId).subscribe({
    next: () => this.loadRequests(),
    error: error => console.error('Failed to approve return:', error)
  });
}
}