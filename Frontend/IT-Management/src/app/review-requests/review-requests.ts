import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  CheckoutRequest,
  CheckoutRequestStatus
} from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';

@Component({
  selector: 'app-review-requests',
  imports: [CommonModule],
  templateUrl: './review-requests.html',
  styleUrl: './review-requests.css',
})
export class ReviewRequests implements OnInit {
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
        console.error('Failed to load requests:', error);
      },
    });
  }

  get pendingRequests() {
    return this.requests.filter(
      r => r.status === CheckoutRequestStatus.Pending
    );
  }

  approveRequest(id: number) {
    this.api.approveCheckoutRequest(id).subscribe({
      next: () => this.loadRequests(),
      error: (error) => console.error('Failed to approve request:', error),
    });
  }

  rejectRequest(id: number) {
    this.api.rejectCheckoutRequest(id).subscribe({
      next: () => this.loadRequests(),
      error: (error) => console.error('Failed to reject request:', error),
    });
  }
}