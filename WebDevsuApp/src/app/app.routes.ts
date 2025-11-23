import { Routes } from '@angular/router';
import { Layout } from './shared/layout/layout';

export const routes: Routes = [
  {
    path: '',
    component: Layout,
    children: [
      {
        path: 'mantenimientos',
        loadChildren: () =>
          import('./components/mantenimientos/mantenimientos.route').then(
            (m) => m.MANTENIMIENTOS_ROUTES
          ),
      },
      {
        path: 'reportes',
        loadChildren: () =>
          import('./components/reportes/reportes.route').then(
            (m) => m.REPORTE_ROUTES
          ),
      },
      //   { path: '', redirectTo: 'home', pathMatch: 'full' },
    ],
  },
];
