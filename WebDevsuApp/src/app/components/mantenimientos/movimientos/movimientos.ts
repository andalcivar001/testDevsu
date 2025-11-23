import { Component } from '@angular/core';
import { SharedModule } from '../../../shared/module/shared.module';
import { IMovimiento } from '../../../core/model/entidades/movimiento.model';
import { MovimientoService } from '../../../core/service/movimiento.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { APP_MESSAGES } from '../../../shared/constant/messages-constant';
import { APP_ROUTES } from '../../../shared/constant/routes-constant';
import { DialogConfirm } from '../../../shared/components/dialog-confirm/dialog-confirm';

@Component({
  selector: 'app-movimientos',
  imports: [SharedModule, DialogConfirm],
  templateUrl: './movimientos.html',
})
export class Movimientos {
  movimientosResp: IMovimiento[] = [];
  movimientos: IMovimiento[] = [];
  pageSize = 5;
  showDropdown = false;
  busqueda: string = '';

  showModal = false;
  idSeleccionado?: number;

  get pagedData() {
    return this.movimientos.slice(0, this.pageSize);
  }

  constructor(
    private _movimientoService: MovimientoService,
    private _router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.obtenerDatos();
  }

  obtenerDatos() {
    this._movimientoService.obtener().subscribe({
      next: (res) => {
        if (res.success) {
          this.movimientosResp = [...res.data];
          this.movimientos = [...res.data];
          this.movimientos = this.movimientosResp.slice(0, this.pageSize);
        } else {
          this.toastr.error(`${res.message}`);
        }
      },
      error: (error) => {
        this.toastr.error(
          `${APP_MESSAGES.ERROR_CONSULTAR} ${error.message}`,
          'Error'
        );
      },
    });
  }

  editarMovimiento(id: number) {
    this._router.navigate([APP_ROUTES.MOVIMIENTOS_FORM], {
      relativeTo: this.activatedRoute,
      queryParams: {
        idMovimiento: id,
      },
    });
  }

  eliminarMovimiento(idMovimiento: number) {
    this._movimientoService.eliminarMovimiento(idMovimiento).subscribe({
      next: (res) => {
        if (res.success) {
          this.obtenerDatos();
          this.toastr.success(APP_MESSAGES.ELIMINADO_OK);
        } else {
          this.toastr.error(`${res.message}`);
        }
      },
      error: (error) => {
        this.toastr.error(
          `${APP_MESSAGES.ERROR_ELIMINAR} ${error.message}`,
          'Error'
        );
      },
    });
  }

  onChange(event: any) {
    this.movimientos = [...this.movimientosResp];
    if (this.busqueda && this.busqueda.length > 0) {
      this.movimientos = this.movimientos.filter(
        (item) =>
          item.numeroCuenta?.toLowerCase().includes(this.busqueda) ||
          item.nombreCliente?.toLowerCase().includes(this.busqueda) ||
          item.valor?.toString().toLowerCase().includes(this.busqueda) ||
          item.idMovimiento?.toString().includes(this.busqueda)
      );
    }
  }

  crearMovimiento() {
    this._router.navigate([APP_ROUTES.MOVIMIENTOS_FORM]);
  }

  onChangePageSize(): void {
    this.movimientos = this.movimientosResp.slice(0, this.pageSize);
  }

  abrirModal(id: number) {
    this.idSeleccionado = id;
    this.showModal = true;
  }

  confirmar() {
    this.showModal = false;
    this.eliminarMovimiento(this.idSeleccionado!);
  }

  cancelar() {
    this.showModal = false;
  }
}
