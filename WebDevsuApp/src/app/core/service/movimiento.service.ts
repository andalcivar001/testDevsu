import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../enviroments/environment';
import { IMovimiento } from '../model/entidades/movimiento.model';
import { IApiResponse } from '../model/entidades/api-response.model';
import { IMovimientoReporte } from '../model/reportes/movimiento-report-model';

@Injectable({
  providedIn: 'root',
})
export class MovimientoService {
  address: string = environment.apiRoot + '/movimiento';

  constructor(private _http: HttpClient) {}

  obtener(): Observable<IApiResponse<IMovimiento[]>> {
    return this._http.get<IApiResponse<IMovimiento[]>>(`${this.address}`);
  }

  obtenerMovimientoPorId(id: number): Observable<IApiResponse<IMovimiento>> {
    return this._http.get<IApiResponse<IMovimiento>>(`${this.address}/${id}`);
  }

  crearMovimiento(Product: IMovimiento): Observable<IApiResponse<IMovimiento>> {
    return this._http.post<IApiResponse<IMovimiento>>(this.address, Product);
  }

  editarMovimiento(
    id: number,
    movimiento: IMovimiento
  ): Observable<IApiResponse<IMovimiento>> {
    return this._http.put<IApiResponse<IMovimiento>>(
      `${this.address}/${id}`,
      movimiento
    );
  }
  eliminarMovimiento(id: number): Observable<IApiResponse<IMovimiento>> {
    return this._http.delete<IApiResponse<IMovimiento>>(
      `${this.address}/${id}`
    );
  }

  excederCupoDiario(
    idCuenta: number,
    fecha: string
  ): Observable<IApiResponse<boolean>> {
    return this._http.get<IApiResponse<boolean>>(
      `${this.address}/excede-cupo-diario/${idCuenta}/${fecha}`
    );
  }

  consultarReporteMovimientos(
    idCliente: number,
    fechaDesde: string,
    fechaHasta: string
  ): Observable<IApiResponse<IMovimientoReporte[]>> {
    return this._http.get<IApiResponse<IMovimientoReporte[]>>(
      `${this.address}/consultar-reporte?idCliente=${idCliente}&fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`
    );
  }

  exportarReportePDF(
    idCliente: number,
    fechaDesde: string,
    fechaHasta: string
  ): Observable<any> {
    return this._http.get<any>(
      `${this.address}/exportar-movimientos?idCliente=${idCliente}&fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`
    );
  }
}
