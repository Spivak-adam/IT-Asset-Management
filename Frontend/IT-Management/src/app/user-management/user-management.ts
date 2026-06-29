import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ITAssetApi } from '../services/it-asset-api';
import { User, UserRole } from '../models/it-asset.models';

@Component({
  selector: 'app-user-management',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-management.html',
  styleUrl: './user-management.css',
})
export class UserManagement implements OnInit {
  users: User[] = [];
  UserRole = UserRole;

  constructor(
    private api: ITAssetApi,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.api.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Failed to load users:', error);
      },
    });
  }

  updateUserRole(userId: number, role: UserRole) {
    this.api.updateUserRole(userId, role).subscribe({
      next: () => this.loadUsers(),
      error: (error) => console.error('Failed to update user role:', error),
    });
  }

  toggleUserActive(user: User) {
    this.api.updateUserActive(user.id, !user.isActive).subscribe({
      next: () => this.loadUsers(),
      error: (error) => console.error('Failed to update user active status:', error),
    });
  }
}