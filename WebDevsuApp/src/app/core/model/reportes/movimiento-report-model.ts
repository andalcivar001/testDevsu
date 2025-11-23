export interface IMovimientoReporte {
  fecha?: Date;
  cliente?: string;
  numeroCuenta?: string;
  tipoCuenta?: string;
  saldoInicial?: number;
  estado?: boolean;
  valorMovimiento?: number;
  saldoDisponible?: number;
}
