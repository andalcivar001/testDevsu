export interface IMovimiento {
  idMovimiento?: number;
  fecha?: Date | string;
  tipoMovimiento?: string;
  valor?: number;
  idCuenta?: number;
  saldo?: number;
  nombreCliente?: string;
  numeroCuenta?: string;
  estado?: boolean;
}
