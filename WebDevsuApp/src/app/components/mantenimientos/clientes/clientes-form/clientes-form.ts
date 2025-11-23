import { Component } from '@angular/core';
import { SharedModule } from '../../../../shared/module/shared.module';
import { ICliente } from '../../../../core/model/entidades/cliente.model';
import { ClienteService } from '../../../../core/service/cliente.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { APP_MESSAGES } from '../../../../shared/constant/messages-constant';
import { APP_ROUTES } from '../../../../shared/constant/routes-constant';

@Component({
  selector: 'app-clientes-form',
  imports: [SharedModule],
  templateUrl: './clientes-form.html',
})
export class ClientesForm {
  idCliente?: number;
  clienteData: ICliente = {};
  tituloForm?: string;

  constructor(
    private _clienteService: ClienteService,
    private _toastrService: ToastrService,
    private _activatedRoute: ActivatedRoute,
    private _router: Router
  ) {}

  ngOnInit() {
    this._activatedRoute!.queryParams!.subscribe((data) => {
      const { idCliente } = data;
      if (idCliente) {
        this.idCliente = idCliente;
        this._clienteService.obtenerClientePorId(this.idCliente!).subscribe({
          next: (res) => {
            if (res.success) {
              this.tituloForm = `Editando Cliente ${res.data.idCliente} - ${res.data.nombre}`;
              this.clienteData = { ...res.data };
              this.clienteData.fechaNacimiento = this.formatDateForInput(
                this.clienteData.fechaNacimiento!.toString()
              );
            } else {
              this._toastrService.error(`${res.message}`);
            }
          },
          error: (error) => {
            this._toastrService.error(
              `Error al consultar el cliente ${error.message}`
            );
          },
        });
      } else {
        this.idCliente = undefined;
        this.tituloForm = 'Creando Cliente';
      }
    });
  }

  onGuardarCliente() {
    if (!this.onValidarFormulario()) {
      return;
    }
    if (this.clienteData.estado!.toString() == 'true') {
      this.clienteData.estado = true;
    } else {
      this.clienteData.estado = false;
    }

    if (this.idCliente) {
      this.onEditarCliente();
    } else {
      this.onCrearCliente();
    }
  }

  onCrearCliente() {
    this._clienteService.crearCliente(this.clienteData).subscribe({
      next: (res) => {
        if (res.success) {
          this._toastrService.success(`${APP_MESSAGES.CREADO_OK}`);
          this._router.navigate([APP_ROUTES.CLIENTES]);
        } else {
          this._toastrService.error(`${res.message}`);
        }
      },
      error: (error) => {
        this._toastrService.error(
          `${APP_MESSAGES.ERROR_GUARDAR} - ${error.message}`
        );
      },
    });
  }

  onEditarCliente() {
    this._clienteService
      .editarCliente(this.idCliente!, this.clienteData)
      .subscribe({
        next: (res) => {
          if (res.success) {
            this._toastrService.success(`${APP_MESSAGES.ACTUALIZADO_OK}`);
            this._router.navigate([APP_ROUTES.CLIENTES]);
          } else {
            this._toastrService.error(`${res.message}`);
          }
        },
        error: (error) => {
          this._toastrService.error(
            `${APP_MESSAGES.ERROR_GUARDAR} - ${error.message}`
          );
        },
      });
  }

  onValidarFormulario() {
    if (!this.clienteData.nombre || this.clienteData.nombre.trim() === '') {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Nombre`,
        ''
      );
      return false;
    }
    if (!this.clienteData.genero || this.clienteData.genero.trim() === '') {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Genero`,
        ''
      );
      return false;
    }
    if (!this.clienteData.fechaNacimiento) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Fecha de Nacimiento`,
        ''
      );
      return false;
    }

    if (
      !this.clienteData.numeroIdentificacion ||
      this.clienteData.numeroIdentificacion.trim() === ''
    ) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Número de Identificación`,
        ''
      );
      return false;
    }
    if (
      !this.clienteData.direccion ||
      this.clienteData.direccion.trim() === ''
    ) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Dirección`,
        ''
      );
      return false;
    }
    if (!this.clienteData.telefono || this.clienteData.telefono.trim() === '') {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Teléfono`,
        ''
      );
      return false;
    }

    if (!this.clienteData.password || this.clienteData.password.trim() === '') {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Password`,
        ''
      );
      return false;
    }

    if (!this.clienteData.estado) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Estado`,
        ''
      );
      return false;
    }

    return true;
  }

  onCancelar() {
    this._router.navigate([APP_ROUTES.CLIENTES]);
  }

  formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0]; // 'YYYY-MM-DD'
  }
}
