import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../enviroments/environment';
import { ICliente } from '../model/entidades/cliente.model';
import { IApiResponse } from '../model/entidades/api-response.model';
import { ICuenta } from '../model/entidades/cuenta.model';

@Injectable({
  providedIn: 'root',
})
export class CuentaService {
  address: string = environment.apiRoot + '/cuenta';

  constructor(private _http: HttpClient) {}

  obtener(): Observable<IApiResponse<ICuenta[]>> {
    return this._http.get<IApiResponse<ICuenta[]>>(`${this.address}`);
  }

  obtenerCuentaPorId(id: number): Observable<IApiResponse<ICuenta>> {
    return this._http.get<IApiResponse<ICuenta>>(`${this.address}/${id}`);
  }

  crearCuenta(Product: ICuenta): Observable<IApiResponse<ICuenta>> {
    return this._http.post<IApiResponse<ICuenta>>(this.address, Product);
  }

  editarCuenta(id: number, cuenta: ICuenta): Observable<IApiResponse<ICuenta>> {
    return this._http.put<IApiResponse<ICuenta>>(
      `${this.address}/${id}`,
      cuenta
    );
  }
  eliminarCuenta(id: number): Observable<IApiResponse<void>> {
    return this._http.delete<IApiResponse<void>>(`${this.address}/${id}`);
  }

  obtenerSaldoCuenta(id: number): Observable<IApiResponse<number>> {
    return this._http.get<IApiResponse<number>>(
      `${this.address}/obtiene-saldo-actual/${id}`
    );
  }
}
