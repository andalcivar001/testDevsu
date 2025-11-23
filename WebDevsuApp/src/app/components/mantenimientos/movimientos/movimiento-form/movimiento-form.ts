import { Component } from '@angular/core';
import { SharedModule } from '../../../../shared/module/shared.module';
import { MovimientoService } from '../../../../core/service/movimiento.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IMovimiento } from '../../../../core/model/entidades/movimiento.model';
import { ToastrService } from 'ngx-toastr';
import { APP_MESSAGES } from '../../../../shared/constant/messages-constant';
import { APP_ROUTES } from '../../../../shared/constant/routes-constant';
import { ICuenta } from '../../../../core/model/entidades/cuenta.model';
import { CuentaService } from '../../../../core/service/cuenta.service';
import { filter, switchMap } from 'rxjs';

@Component({
  selector: 'app-movimiento-form',
  imports: [SharedModule],
  templateUrl: './movimiento-form.html',
})
export class MovimientoForm {
  idMovimiento?: number;
  movimientoData: IMovimiento = {};
  cuentas: ICuenta[] = [];
  tituloForm?: string;
  constructor(
    private _cuentaService: CuentaService,
    private _toastrService: ToastrService,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _movimientoService: MovimientoService
  ) {}

  ngOnInit() {
    this._activatedRoute!.queryParams!.subscribe((data) => {
      const { idMovimiento } = data;
      if (idMovimiento) {
        this.idMovimiento = idMovimiento;
        this._movimientoService
          .obtenerMovimientoPorId(this.idMovimiento!)
          .subscribe({
            next: (res) => {
              if (res.success) {
                this.tituloForm = `Editando Movimiento - ${res.data.idMovimiento}`;
                this.movimientoData = { ...res.data };
                this.movimientoData.fecha = this.formatDateForInput(
                  this.movimientoData.fecha!.toString()
                );
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
        this.idMovimiento = undefined;
        this.tituloForm = 'Creando Movimiento';
      }
    });
    this.obtenerCuentas();
  }

  obtenerCuentas() {
    this._cuentaService.obtener().subscribe({
      next: (res) => {
        this.cuentas = [...res.data];
      },
      error: (error) => {
        this._toastrService.error(
          `${APP_MESSAGES.ERROR_CONSULTAR} ${error.message}`,
          'Error'
        );
      },
    });
  }

  onChangeIdCuenta() {
    if (this.movimientoData.idCuenta) {
      this._cuentaService
        .obtenerSaldoCuenta(this.movimientoData.idCuenta!)
        .subscribe({
          next: (res) => {
            this.movimientoData.saldo = res.data;
          },
          error: (error) => {
            this._toastrService.error(
              `${APP_MESSAGES.ERROR_CONSULTAR} ${error.message}`,
              'Error'
            );
          },
        });
    }
  }

  onGuardarMovimiento() {
    if (!this.onValidarFormulario()) {
      return;
    }

    if (this.movimientoData.estado!.toString() == 'true') {
      this.movimientoData.estado = true;
    } else {
      this.movimientoData.estado = false;
    }
    console.log('Validado correctamente');
    const fechaFormateada = this.formatDateForInput(
      this.movimientoData.fecha!.toString()
    );
    this._movimientoService
      .excederCupoDiario(this.movimientoData.idCuenta!, fechaFormateada)
      .subscribe({
        next: (res) => {
          console.log('Respuesta validacion cupo diario', res);
          if (res.success && !res.data) {
            if (this.idMovimiento) {
              this.onEditarMovimiento();
            } else {
              this.onCrearMovimiento();
            }
          } else {
            this._toastrService.error(
              `${APP_MESSAGES.EXCEDE_CUPO_DIARIO} - para la fecha: ${fechaFormateada}`
            );
          }
        },
        error: (error) => {
          this._toastrService.error(
            `${APP_MESSAGES.ERROR_GUARDAR} - ${error.message}`
          );
        },
      });
  }

  onCrearMovimiento() {
    this._movimientoService.crearMovimiento(this.movimientoData).subscribe({
      next: (res) => {
        if (res.success) {
          this._toastrService.success(`${APP_MESSAGES.CREADO_OK}`);
          this._router.navigate([APP_ROUTES.MOVIMIENTOS]);
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

  onEditarMovimiento() {
    this._movimientoService
      .editarMovimiento(this.idMovimiento!, this.movimientoData)
      .subscribe({
        next: (res) => {
          if (res.success) {
            this._toastrService.success(`${APP_MESSAGES.ACTUALIZADO_OK}`);
            this._router.navigate([APP_ROUTES.MOVIMIENTOS]);
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
    if (!this.movimientoData.idCuenta) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Cuenta`,
        ''
      );
      return false;
    }

    if (
      !this.movimientoData.tipoMovimiento ||
      this.movimientoData.tipoMovimiento.trim() === ''
    ) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Tipo de Movimiento`,
        ''
      );
      return false;
    }

    if (!this.movimientoData.fecha) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Fecha`,
        ''
      );
      return false;
    }

    if (!this.movimientoData.valor || this.movimientoData.valor <= 0) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Valor`,
        ''
      );
      return false;
    }

    if (!this.movimientoData.saldo || this.movimientoData.saldo <= 0) {
      this._toastrService.error(
        `${APP_MESSAGES.CAMPO_OBLIGATORIO} - Saldo`,
        ''
      );
      return false;
    }

    if (
      this.movimientoData.saldo < this.movimientoData.valor &&
      this.movimientoData.tipoMovimiento == 'D'
    ) {
      this._toastrService.error(`${APP_MESSAGES.SALDO_NO_DISPONIBLE}`, '');
      return false;
    }
    return true;
  }

  formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0]; // 'YYYY-MM-DD'
  }

  onCancelar() {
    this._router.navigate([APP_ROUTES.MOVIMIENTOS]);
  }
}
