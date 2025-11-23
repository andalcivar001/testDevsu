import { Routes } from '@angular/router';
import { Clientes } from './clientes/clientes';
import { ClientesForm } from './clientes/clientes-form/clientes-form';
import { Cuentas } from './cuentas/cuentas';
import { CuentasForm } from './cuentas/cuentas-form/cuentas-form';
import { Movimientos } from './movimientos/movimientos';
import { MovimientoForm } from './movimientos/movimiento-form/movimiento-form';

export const MANTENIMIENTOS_ROUTES: Routes = [
  {
    path: '',
    children: [
      // grid
      { path: 'clientes', component: Clientes },
      { path: 'cuentas', component: Cuentas },
      { path: 'movimientos', component: Movimientos },
      // formularios
      { path: 'cliente', component: ClientesForm },
      { path: 'cuenta', component: CuentasForm },
      { path: 'movimiento', component: MovimientoForm },
    ],
  },
];
