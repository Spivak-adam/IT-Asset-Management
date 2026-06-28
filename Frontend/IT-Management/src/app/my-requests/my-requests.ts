import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutRequest, CheckoutRequestStatus } from '../models/it-asset.models';
import { ITAssetApi } from '../services/it-asset-api';
import { AuthService } from '../services/auth';

@Component({
  selector: 'app-my-requests',
  imports: [CommonModule],
  templateUrl: './my-requests.html',
  styleUrl: './my-requests.css',
})
export class MyRequests implements OnInit {
  requests: CheckoutRequest[] = [];
  CheckoutRequestStatus = CheckoutRequestStatus;

  constructor(
    private api: ITAssetApi,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadMyRequests();
  }

  loadMyRequests() {
    const userId = this.authService.getUserId();

    this.api.getMyCheckoutRequests(userId).subscribe({
      next: (requests) => {
        this.requests = requests;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Failed to load my requests:', error);
      },
    });
  }
}