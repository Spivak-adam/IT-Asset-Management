export enum UserRole {
  Employee = 1,
  AssetManager = 2,
  Admin = 3,
}

export enum AssetStatus {
  Available = 1,
  Assigned = 2,
  Maintenance = 3,
  Retired = 4,
  Pending = 5,
}

export enum AssetCondition {
  New = 1,
  Good = 2,
  Fair = 3,
  Damaged = 4,
  Lost = 5,
}

export enum CheckoutRequestStatus {
  Pending = 1,
  Approved = 2,
  Rejected = 3,
  Fulfilled = 4,
  Cancelled = 5,
  Returned = 6,
}

export interface User {
  id: number;
  email: string;
  FirstName: string;
  LastName: string;
  passwordHash?: string;
  role: UserRole;
  isActive: boolean;
  createdAt: string;
}

export interface Asset {
  id: number;
  assetTag: string;
  name: string;
  category: string;
  serialNumber: string;
  status: AssetStatus;
  condition: AssetCondition;
  assignedToUserId?: number | null;
  assignedToUser?: User | null;
  createdAt: string;
  updatedAt: string;
  isArchived: boolean;
}

export interface CheckoutRequest {
  id: number;
  requestedByUserId: number;
  requestedAssetId?: number | null;
  assetCategory: string;
  reason: string;
  status: CheckoutRequestStatus;
  reviewedByUserId?: number | null;
  assignedAssetId?: number | null;
  approvedAt?: string | null;
  rejectedAt?: string | null;
  fulfilledAt?: string | null;
  returnedAt?: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface AssetHistory {
  id: number;
  assetId: number;
  userId?: number | null;
  action: string;
  oldValue?: string | null;
  newValue?: string | null;
  createdAt: string;
}