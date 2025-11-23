import { Component } from '@angular/core';
import { ICuenta } from '../../../../core/model/entidades/cuenta.model';
import { ClienteService } from '../../../../core/service/cliente.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { CuentaService } from '../../../../core/service/cuenta.service';
import { APP_ROUTES } from '../../../../shared/constant/routes-constant';
import { APP_MESSAGES } from '../../../../shared/constant/messages-constant';
import { SharedModule } from '../../../../shared/module/shared.module';
import { ICliente } from '../../../../core/model/entidades/cliente.model';

@Component({
  selector: 'app-cuentas-form',
  imports: [SharedModule],
  templateUrl: './cuentas-form.html',
})
export class CuentasForm {
  idCuenta?: number;
  cuentaData: ICuenta = {};
  clientes: ICliente[] = [];
  tituloForm?: string;
  constructor(
    private _clienteService: ClienteService,
    private _toastrService: ToastrService,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _cuentaService: CuentaService
  ) {}

  ngOnInit() {
    this._activatedRoute!.queryParams!.subscribe((data) => {
      const { idCuenta } = data;
      if (idCuenta) {
        this.idCuenta = idCuenta;
        this._cuentaService.obtenerCuentaPorId(this.idCuenta!).subscribe({
          next: (res) => {
            if (res.success) {
              this.tituloForm = `Editando Cuenta - ${res.data.idCuenta}`;
              this.cuentaData = { ...res.data };
              console.log(this.cuentaData);
            } else {
              this._toastrService.error(`${res.message}`);
            }
          },
          error: (error) => {
            this._toastrService.error(
              `${APP_MESSAGES.ERROR_CONSULTAR} ${error.message}`
            );
          },
        });
      } else {
        this.idCuenta = undefined;
        this.tituloForm = 'Creando Cliente';
      }
    });
    this.obtenerClientes();
  }

  obtenerClientes() {
    this._clienteService.obtener().subscribe({
      next: (res) => {
        this.clientes = [...res.data];
      },
      error: (error) => {
        this._toastrService.error(
          `${APP_MESSAGES.ERROR_CONSULTAR} ${error.message}`,
          'Error'
        );
      },
    });
  }

  onGuardarCliente() {
    if (!this.onValidarFormulario()) {
      return;
    }
    if (this.cuentaData.estado!.toString() == 'true') {
      this.cuentaData.estado = true;
    } else {
      this.cuentaData.estado = false;
    }

    if (this.idCuenta) {
      this.onEditarCuenta();
    } else {
      this.onCrearCuenta();
    }
  }

  onCrearCuenta() {
    this._cuentaService.crearCuenta(this.cuentaData).subscribe({
      next: (res) => {
        if (res.success) {
          this._toastrService.success(`${APP_MESSAGES.CREADO_OK}`);
          this._router.navigate([APP_ROUTES.CUENTAS]);
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

  onEditarCuenta() {
    this._cuentaService
      .editarCuenta(this.idCuenta!, this.cuentaData)
      .subscribe({
        next: (res) => {
          if (res.success) {
            this._toastrService.success(`${APP_MESSAGES.ACTUALIZADO_OK}`);
            this._router.navigate([APP_ROUTES.CUENTAS]);
          } else {
            this._toastrService.error(`${res.message}`);
          }
        },
        error: (error) => {
          this._toastrService.error(
            `${APP_MESSAGES.ERROR_ACTUALIZAR} - ${error.message}`
          );
        },
      });
  }

  onValidarFormulario() {
    if (
      !this.cuentaData.numeroCuenta ||
      this.cuentaData.numeroCuenta.trim() === ''
    ) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Numero de Cuenta`,
        ''
      );
      return false;
    }

    if (
      !this.cuentaData.tipoCuenta ||
      this.cuentaData.tipoCuenta.trim() === ''
    ) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Tipo de Cuenta`,
        ''
      );
      return false;
    }

    if (!this.cuentaData.saldoInicial) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Saldo Inicial`,
        ''
      );
      return false;
    }

    if (!this.cuentaData.idCliente) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Cliente`,
        ''
      );
      return false;
    }

    if (!this.cuentaData.estado) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Estado`,
        ''
      );
      return false;
    }

    return true;
  }

  onCancelar() {
    this._router.navigate([APP_ROUTES.CUENTAS]);
  }
}
