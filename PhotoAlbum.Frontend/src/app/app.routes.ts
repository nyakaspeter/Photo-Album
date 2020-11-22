import { AuthenticationGuard } from './core/auth/authentication.guard';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AlbumsComponent } from './albums/albums.component';
import { AlbumsSharedComponent } from './albums-shared/albums-shared.component';
import { AlbumsPublicComponent } from './albums-public/albums-public.component';
import { GroupsComponent } from './groups/groups.component';

export const APP_ROUTES = [
  {
    path: 'albums/my',
    canActivate: [AuthenticationGuard],
    component: AlbumsComponent,
  },
  {
    path: 'albums/shared',
    canActivate: [AuthenticationGuard],
    component: AlbumsSharedComponent,
  },
  {
    path: 'albums/:path',
    component: AlbumsPublicComponent,
  },
  {
    path: 'groups',
    canActivate: [AuthenticationGuard],
    component: GroupsComponent,
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', redirectTo: 'albums/my' },
];
