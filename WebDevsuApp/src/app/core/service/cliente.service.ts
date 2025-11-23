import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../enviroments/environment';
import { ICliente } from '../model/entidades/cliente.model';
import { IApiResponse } from '../model/entidades/api-response.model';

@Injectable({
  providedIn: 'root',
})
export class ClienteService {
  address: string = environment.apiRoot + '/cliente';

  constructor(private _http: HttpClient) {}

  obtener(): Observable<IApiResponse<ICliente[]>> {
    return this._http.get<IApiResponse<ICliente[]>>(`${this.address}`);
  }

  obtenerClientePorId(id: number): Observable<IApiResponse<ICliente>> {
    return this._http.get<IApiResponse<ICliente>>(`${this.address}/${id}`);
  }

  crearCliente(Product: ICliente): Observable<IApiResponse<ICliente>> {
    return this._http.post<IApiResponse<ICliente>>(this.address, Product);
  }

  editarCliente(
    id: number,
    cliente: ICliente
  ): Observable<IApiResponse<ICliente>> {
    return this._http.put<IApiResponse<ICliente>>(
      `${this.address}/${id}`,
      cliente
    );
  }
  eliminarCliente(id: number): Observable<IApiResponse<ICliente>> {
    return this._http.delete<IApiResponse<ICliente>>(`${this.address}/${id}`);
  }
}
