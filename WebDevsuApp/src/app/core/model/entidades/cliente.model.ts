export interface ICliente {
  idCliente?: number;
  nombre?: string;
  genero?: string;
  fechaNacimiento?: Date | string;
  numeroIdentificacion?: string;
  direccion?: string;
  telefono?: string;
  password?: string;
  estado?: boolean;
}
