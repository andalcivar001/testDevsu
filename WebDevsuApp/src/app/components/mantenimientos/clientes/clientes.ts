import { Component } from '@angular/core';
import { ICliente } from '../../../core/model/entidades/cliente.model';
import { ClienteService } from '../../../core/service/cliente.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedModule } from '../../../shared/module/shared.module';
import { APP_MESSAGES } from '../../../shared/constant/messages-constant';
import { APP_ROUTES } from '../../../shared/constant/routes-constant';
import { DialogConfirm } from '../../../shared/components/dialog-confirm/dialog-confirm';

@Component({
  selector: 'app-clientes',
  imports: [SharedModule, DialogConfirm],
  templateUrl: './clientes.html',
})
export class Clientes {
  clientesResp: ICliente[] = [];
  clientes: ICliente[] = [];
  pageSize = 5;
  showDropdown = false;
  busqueda: string = '';

  showModal = false;
  idSeleccionado?: number;

  get pagedData() {
    return this.clientes.slice(0, this.pageSize);
  }

  constructor(
    private _clienteService: ClienteService,
    private _router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.obtenerDatos();
  }

  obtenerDatos() {
    this._clienteService.obtener().subscribe({
      next: (res) => {
        if (res.success) {
          this.clientesResp = [...res.data];
          this.clientes = [...res.data];
          this.clientes = this.clientesResp.slice(0, this.pageSize);
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

  editarCliente(id: number) {
    this._router.navigate([APP_ROUTES.CLIENTES_FORM], {
      relativeTo: this.activatedRoute,
      queryParams: {
        idCliente: id,
      },
    });
  }

  eliminarCliente(idCliente: number) {
    this._clienteService.eliminarCliente(idCliente).subscribe({
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
    this.clientes = [...this.clientesResp];
    if (this.busqueda && this.busqueda.length > 0) {
      this.clientes = this.clientes.filter(
        (item) =>
          item.nombre?.toLowerCase().includes(this.busqueda) ||
          item.numeroIdentificacion?.toLowerCase().includes(this.busqueda) ||
          item.idCliente?.toString().toLowerCase().includes(this.busqueda)
      );
    }
  }

  crearCliente() {
    this._router.navigate([APP_ROUTES.CLIENTES_FORM]);
  }

  onChangePageSize(): void {
    this.clientes = this.clientesResp.slice(0, this.pageSize);
  }

  abrirModal(id: number) {
    this.idSeleccionado = id;
    this.showModal = true;
  }

  confirmar() {
    this.showModal = false;
    this.eliminarCliente(this.idSeleccionado!);
  }

  cancelar() {
    this.showModal = false;
  }
}
