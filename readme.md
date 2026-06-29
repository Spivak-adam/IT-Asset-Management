# IT Asset Management System

A full-stack IT Asset Management application built with **Angular**, **ASP.NET Core**, **Entity Framework Core**, and **SQL Server**.

This project supports three main roles:

- **Employee**
- **Asset Manager**
- **Admin**

The system allows employees to request assets by category, managers to approve and assign specific assets, and admins to manage users and asset inventory.

---

## Project Overview

This application was built as a role-based internal IT asset management portal.

The core workflow is:

```text
Employee requests asset by category
        ↓
Manager reviews request
        ↓
Manager assigns available asset
        ↓
Asset becomes assigned to employee
        ↓
Employee requests return
        ↓
Manager approves return
        ↓
Asset becomes available again
```

---

## Tech Stack

### Frontend

- Angular
- TypeScript
- HTML
- CSS
- Angular Router
- Angular Forms
- Angular HttpClient

### Backend

- ASP.NET Core
- C#
- Entity Framework Core
- SQL Server
- BCrypt password hashing
- REST API architecture

### Database

- SQL Server
- Entity Framework migrations
- Seed data

---

## Project Structure

Example structure:

```text
IT-Asset-Management/
│
├── Frontend/
│   └── IT-Management/
│       ├── package.json
│       ├── angular.json
│       ├── src/
│       │   ├── app/
│       │   │   ├── dashboard/
│       │   │   ├── login/
│       │   │   ├── my-assets/
│       │   │   ├── checkout/
│       │   │   ├── my-requests/
│       │   │   ├── review-requests/
│       │   │   ├── return-asset/
│       │   │   ├── assets/
│       │   │   ├── user-management/
│       │   │   ├── services/
│       │   │   └── models/
│       │   └── assets/
│
└── IT-Asset-Mang-BE/
    ├── Controllers/
    ├── DTOs/
    ├── Models/
    ├── Services/
    ├── Data/
    ├── Enums/
    ├── Migrations/
    ├── Program.cs
    ├── appsettings.json
    └── IT-Asset-Mang-BE.csproj
```

---

## Features Completed

### Authentication

- User login
- Password hashing with BCrypt
- Session values stored in local storage
- Role-based navigation
- Inactive users blocked from logging in
- Logout functionality
- Unauthorized page for inactive/unauthorized users

---

## Role-Based Navigation

The sidebar changes based on the logged-in user's role.

### Employee

- Dashboard
- My Assets
- Checkout Assets
- My Requests

### Asset Manager

- Dashboard
- Review Requests
- Asset Inventory
- Returns

### Admin

- Dashboard
- Asset Management
- Review Requests
- User Management
- Returns

---

## Employee Features

Employees can:

- View their assigned assets
- View asset details
- View asset history
- Request checkout by asset category
- View their checkout and return requests
- Request asset returns

The checkout request flow was updated so employees request an **asset category**, not a specific asset.

Example:

```text
Employee requests: Laptop
Manager assigns: TL-LAP-001 Dell Latitude
```

---

## Asset Manager Features

Asset managers can:

- Review pending checkout requests
- View requested category
- Assign a specific available asset from that category
- Approve checkout requests
- Reject checkout requests
- Review return requests
- Approve asset returns
- View asset inventory

---

## Admin Features

Admins can:

- View all assets
- Archive assets
- Restore archived assets
- Manually assign assets to users
- Manually return assets
- View users
- Change user roles
- Activate users
- Deactivate users
- Access manager functionality

---

## Backend Architecture

The backend follows this pattern:

```text
Controller
    ↓
Service
    ↓
DTO
    ↓
Entity Framework
    ↓
SQL Server
```

Main backend folders:

```text
Controllers/
DTOs/
Models/
Services/
Data/
Enums/
Migrations/
```

---

## Main Models

### User

Stores users and roles.

Important fields:

- Id
- Email
- PasswordHash
- FirstName
- LastName
- Role
- IsActive
- CreatedAt

### Asset

Stores IT assets.

Important fields:

- Id
- AssetTag
- Name
- Category
- SerialNumber
- Status
- Condition
- AssignedToUserId
- IsArchived
- CreatedAt
- UpdatedAt

### CheckoutRequest

Tracks checkout and return workflow.

Important fields:

- RequestedByUserId
- AssetCategory
- Status
- ReviewedByUserId
- AssignedAssetId
- ApprovedAt
- RejectedAt
- FulfilledAt
- ReturnedAt
- CreatedAt
- UpdatedAt

### AssetHistory

Tracks audit history for assets.

Important fields:

- AssetId
- UserId
- Action
- OldValue
- NewValue
- CreatedAt

---

## Enums

### UserRole

Example:

```csharp
public enum UserRole
{
    Employee = 1,
    AssetManager = 2,
    Admin = 3
}
```

### AssetStatus

Example:

```csharp
public enum AssetStatus
{
    Available = 1,
    Assigned = 2,
    Maintenance = 3,
    Retired = 4
}
```

### CheckoutRequestStatus

Example:

```csharp
public enum CheckoutRequestStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Fulfilled = 4,
    Cancelled = 5,
    Returned = 6,
    ReturnRequested = 7
    
}
```

---

## API Features

Implemented API areas:

### Auth

```http
POST /api/ITAsset/auth/login
```

### Assets

```http
GET /api/ITAsset/assets
GET /api/ITAsset/my-assets/{userId}
GET /api/ITAsset/assets/available/{category}
PATCH /api/ITAsset/assets/{id}/archive
PATCH /api/ITAsset/assets/{id}/restore
PATCH /api/ITAsset/assets/{id}/assign
PATCH /api/ITAsset/assets/{id}/return
PATCH /api/ITAsset/assets/{assetId}/request-return/{userId}
```

### Checkout Requests

```http
GET /api/ITAsset/checkout-requests
GET /api/ITAsset/checkout-requests/my/{userId}
POST /api/ITAsset/checkout-requests
PATCH /api/ITAsset/checkout-requests/{id}/approve
PATCH /api/ITAsset/checkout-requests/{id}/reject
PATCH /api/ITAsset/checkout-requests/{id}/return
```

### Users

```http
GET /api/ITAsset/users
PATCH /api/ITAsset/users/{id}/role
PATCH /api/ITAsset/users/{id}/active
```

### Asset History

```http
GET /api/ITAsset/assets/{assetId}/history
```

---

## Setup Instructions

### Prerequisites

Install the following:

- Node.js
- npm
- Angular CLI
- .NET SDK
- SQL Server
- Entity Framework CLI

Install Angular CLI if needed:

```bash
npm install -g @angular/cli
```

Install EF Core CLI if needed:

```bash
dotnet tool install --global dotnet-ef
```

---

## Backend Setup

Open a terminal in the backend folder:

```bash
cd IT-Asset-Mang-BE
```

Restore backend dependencies:

```bash
dotnet restore
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the backend:

```bash
dotnet run
```

The backend should run on something like:

```text
http://localhost:5058
```

or:

```text
https://localhost:7089
```

Check the terminal output to confirm the exact port.

---

## Frontend Setup

Open a second terminal in the Angular frontend folder:

```bash
cd Frontend/IT-Management
```

Install frontend dependencies:

```bash
npm install
```

Run the Angular app:

```bash
ng serve
```

Open the frontend in the browser:

```text
http://localhost:4200
```

---

## Database Reset During Development

To drop and recreate the database:

```bash
dotnet ef database drop --force
dotnet ef database update
dotnet run
```

Use this when migrations or seed data change during development.

---

## Seed Data

The project includes seed data for:

- Admin user
- Asset manager user
- Employee user
- Sample laptops
- Sample monitors
- Sample phones
- Sample security keys
- Asset history records

Example login accounts:

```text
admin@test.com
manager@test.com
employee@test.com
```

Example password:

```text
Password123!
```

---

## Frontend Pages

### Login

Login page with session storage and inactive-user handling.

### Dashboard

Dynamic dashboard based on user role.

### My Assets

Employee page showing assigned assets and asset history.

### Checkout Assets

Employee page for requesting assets by category.

### My Requests

Employee page for viewing checkout and return request status.

### Review Requests

Manager/Admin page for reviewing checkout requests and assigning assets.

### Returns

Manager/Admin page for approving return requests.

### Asset Management

Admin page for managing assets.

Includes:

- Assign asset
- Return asset
- Archive asset
- Restore asset

### User Management

Admin page for managing users.

Includes:

- View users
- Change role
- Activate/deactivate account

---

## Styling

The UI was polished with a consistent professional style:

- Dark blue sidebar
- White content cards
- Rounded corners around 8px-10px
- Soft shadows
- Clean tables
- Status badges
- Role-based navigation
- Responsive horizontal table scrolling

---

## Known Remaining Work

The core application is functionally complete. Remaining polish items:

1. Replace user IDs in tables with full user names.
2. Add asset history buttons/views for manager and admin asset pages.
3. Show a visible error message on the login page when invalid credentials are entered.
4. Refactor backend code
   - Reorganize `ItAssetController.cs` endpoints by feature area
   - Move repeated DTO mapping into helper methods
   - Reduce duplicated response logic

---

## Future Improvements

Potential future enhancements:

- JWT authentication instead of temporary GUID token
- Route guards for role-based page protection
- Search and filter on tables
- Pagination
- Toast notifications
- Email notifications for approvals and returns
- Barcode or QR scanning for assets
- Reports dashboard
- Better audit logging
- Display asset tags/names instead of IDs
- Deployment to Azure or another cloud provider

---

## Project Status

Current project status:

```text
Approximately 98-99% complete
```

Core workflows are complete:

- Login
- Role-based navigation
- Employee checkout request
- Manager approval and assignment
- Return request and approval
- Admin user management
- Admin asset management
- Asset archive/restore
- Asset history tracking

Remaining items are mostly UI polish and data display improvements.
