import { Routes } from '@angular/router';
import { Login } from './login/login';
import { Dashboard } from './dashboard/dashboard';
import { MyAssets } from './my-assets/my-assets';
import { ReviewRequests } from './review-requests/review-requests';
import { Assets } from './assets/assets'
import { MyRequests } from './my-requests/my-requests'
import { Checkout } from './checkout/checkout';
import { ReturnAsset } from './return-asset/return-asset';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'dashboard', component: Dashboard },

  { path: 'my-assets', component: MyAssets },
  { path: 'checkout', component: Checkout },
  { path: 'my-requests', component: MyRequests },

  { path: 'assets', component: MyAssets },
  { path: 'review-requests', component: ReviewRequests },
  { path: 'all-assets', component: Assets },
  //{ path: 'users', component: UserManagement },
  { path: 'returns', component: ReturnAsset },
  //{ path: 'reports', component: Dashboard },
];