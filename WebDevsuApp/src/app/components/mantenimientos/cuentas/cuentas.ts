import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared/module/shared.module';
import { ICuenta } from '../../../core/model/entidades/cuenta.model';
import { CuentaService } from '../../../core/service/cuenta.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { APP_MESSAGES } from '../../../shared/constant/messages-constant';
import { APP_ROUTES } from '../../../shared/constant/routes-constant';
import { DialogConfirm } from '../../../shared/components/dialog-confirm/dialog-confirm';

@Component({
  selector: 'app-cuentas',
  imports: [SharedModule, DialogConfirm],
  templateUrl: './cuentas.html',
})
export class Cuentas implements OnInit {
  cuentasResp: ICuenta[] = [];
  cuentas: ICuenta[] = [];
  pageSize = 5;
  showDropdown = false;
  busqueda: string = '';

  showModal = false;
  idSeleccionado?: number;

  get pagedData() {
    return this.cuentas.slice(0, this.pageSize);
  }

  constructor(
    private _cuentaService: CuentaService,
    private _router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.obtenerDatos();
  }

  obtenerDatos() {
    this._cuentaService.obtener().subscribe({
      next: (res) => {
        if (res.success) {
          this.cuentasResp = [...res.data];
          this.cuentas = [...res.data];
          this.cuentas = this.cuentasResp.slice(0, this.pageSize);
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

  editarCuenta(id: number) {
    this._router.navigate([APP_ROUTES.CUENTAS_FORM], {
      relativeTo: this.activatedRoute,
      queryParams: {
        idCuenta: id,
      },
    });
  }

  eliminarCuenta(idCuenta: number) {
    this._cuentaService.eliminarCuenta(idCuenta!).subscribe({
      next: (res) => {
        if (res.success) {
          this.obtenerDatos();
          this.toastr.success(APP_MESSAGES.ELIMINADO_OK);
        } else {
          this.toastr.error(`${res.message}`, 'Error');
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
    this.cuentas = [...this.cuentasResp];
    if (this.busqueda && this.busqueda.length > 0) {
      this.cuentas = this.cuentas.filter(
        (item) =>
          item.numeroCuenta?.toLowerCase().includes(this.busqueda) ||
          item.saldoInicial?.toString().toLowerCase().includes(this.busqueda) ||
          item.nombreCliente?.toLowerCase().includes(this.busqueda) ||
          item.idCuenta?.toString().includes(this.busqueda)
      );
    }
  }

  crearCuenta() {
    this._router.navigate([APP_ROUTES.CUENTAS_FORM]);
  }

  onChangePageSize(): void {
    this.cuentas = this.cuentasResp.slice(0, this.pageSize);
  }

  abrirModal(id: number) {
    this.idSeleccionado = id;
    this.showModal = true;
  }

  confirmar() {
    this.showModal = false;
    this.eliminarCuenta(this.idSeleccionado!);
  }

  cancelar() {
    this.showModal = false;
  }
}
