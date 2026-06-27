import { Routes } from '@angular/router';
import { Login } from './login/login';
import { Dashboard } from './dashboard/dashboard';
import { MyAssets } from './my-assets/my-assets';
import { ReviewRequests } from './review-requests/review-requests';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'dashboard', component: Dashboard },

  { path: 'my-assets', component: MyAssets },
  { path: 'checkout-assets', component: MyAssets },
  //{ path: 'my-requests', component: MyRequests },

  { path: 'assets', component: MyAssets },
  { path: 'review-requests', component: ReviewRequests },
  //{ path: 'users', component: UserManagement },
  //{ path: 'returns', component: ReviewRequests },
  //{ path: 'reports', component: Dashboard },
];