import { Component, OnInit } from '@angular/core';
import { MovimientoService } from '../../../core/service/movimiento.service';
import { ToastrService } from 'ngx-toastr';
import { ICliente } from '../../../core/model/entidades/cliente.model';
import { ClienteService } from '../../../core/service/cliente.service';
import { APP_MESSAGES } from '../../../shared/constant/messages-constant';
import { SharedModule } from '../../../shared/module/shared.module';
import { IMovimientoReporte } from '../../../core/model/reportes/movimiento-report-model';

@Component({
  selector: 'app-reporte-movimientos',
  imports: [SharedModule],
  templateUrl: './reporte-movimientos.html',
  styleUrl: './reporte-movimientos.scss',
})
export class ReporteMovimientos implements OnInit {
  idCliente?: number;
  fechaDesde?: Date | string;
  fechaHasta?: Date | string;
  clientes: ICliente[] = [];
  movimientos: IMovimientoReporte[] = [];

  constructor(
    private _movimientoService: MovimientoService,
    private toastr: ToastrService,
    private _clienteService: ClienteService
  ) {}

  ngOnInit(): void {
    this.fechaDesde = new Date();
    this.fechaHasta = new Date();
    this.fechaDesde.setDate(this.fechaHasta.getDate() - 7);
    this.fechaDesde = this.formatDateForInput(this.fechaDesde.toString());
    this.fechaHasta = this.formatDateForInput(this.fechaHasta.toString());
    this.obtenerClientes();
  }

  onConsultarReporte() {
    if (!this.validarFiltros()) {
      return;
    }

    const fechaDesdeStr = this.formatDateForInput(this.fechaDesde!.toString());
    const fechaHastaStr = this.formatDateForInput(this.fechaHasta!.toString());

    this._movimientoService
      .consultarReporteMovimientos(
        this.idCliente!,
        fechaDesdeStr,
        fechaHastaStr
      )
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.movimientos = [...res.data];
            if (this.movimientos.length === 0) {
              this.toastr.info(
                'No se encontraron movimientos para el reporte',
                ''
              );
            }
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

  obtenerClientes() {
    this._clienteService.obtener().subscribe({
      next: (res) => {
        if (res.success) {
          this.clientes = [...res.data];
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

  formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0]; // 'YYYY-MM-DD'
  }

  validarFiltros(): boolean {
    if (!this.idCliente) {
      this.toastr.warning('Debe seleccionar un cliente', '');
      return false;
    }
    if (!this.fechaDesde || !this.fechaHasta) {
      this.toastr.warning('Debe seleccionar ambas fechas', '');
      return false;
    }
    return true;
  }

  onGenerarPDF() {
    if (!this.validarFiltros()) {
      return;
    }

    if (this.movimientos.length === 0) {
      this.toastr.info('No hay datos para generar el reporte PDF', '');
      return;
    }

    const fechaDesdeStr = this.formatDateForInput(this.fechaDesde!.toString());
    const fechaHastaStr = this.formatDateForInput(this.fechaHasta!.toString());

    this._movimientoService
      .exportarReportePDF(this.idCliente!, fechaDesdeStr, fechaHastaStr)
      .subscribe({
        next: (res) => {
          console.log(res);
          const base64 = res.archivoBase64;

          // Lo conviertes en PDF descargable
          const linkSource = `data:application/pdf;base64,${base64}`;
          const downloadLink = document.createElement('a');
          downloadLink.href = linkSource;
          downloadLink.download = 'ReporteMovimientos.pdf';
          downloadLink.click();
        },
        error: (error) => {
          this.toastr.error(
            `${APP_MESSAGES.ERROR_CONSULTAR} ${error.message}`,
            'Error'
          );
        },
      });
  }
}
