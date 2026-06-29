import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  constructor(
    public authService: AuthService,
    private router: Router,
  ) {}

  get menuItems() {
    const role = this.authService.getRole();

    if (role === 'Admin') {
      return [
        { label: 'Dashboard', route: '/dashboard' },
        { label: 'Asset Management', route: '/all-assets' },
        { label: 'Review Requests', route: '/review-requests' },
        { label: 'User Management', route: '/users' },
        { label: 'Returns', route: '/returns' },
      ];
    }

    if (role === 'AssetManager') {
      return [
        { label: 'Dashboard', route: '/dashboard' },
        { label: 'Review Requests', route: '/review-requests' },
        { label: 'Asset Inventory', route: '/all-assets' },
        { label: 'Returns', route: '/returns' },
      ];
    }

    if (role === 'Employee') {
      return [
        {
          label: 'Dashboard',
          route: '/dashboard',
        },
        {
          label: 'My Assets',
          route: '/my-assets',
        },
        {
          label: 'Checkout Assets',
          route: '/checkout',
        },
        {
          label: 'My Requests',
          route: '/my-requests',
        },
      ];
    }

    return [];
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
