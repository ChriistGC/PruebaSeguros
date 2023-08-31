import { Component } from '@angular/core';
import { SeguroService } from '../../services/seguro.service';
import { Seguro } from '../../interface/seguros.interface';
import { MatDialog } from '@angular/material/dialog';
import { AlertmodalComponent } from '../../component/modal/alertmodal/alertmodal.component';
import { ErrorHandlingService } from '../../services/error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-agregar-seguro',
  templateUrl: './agregar-seguro.component.html',
  styles: [ ]
})
export class AgregarSeguroComponent {

  seguro!: Seguro;
  nombre!: string;
  codigo!: string;
  suma!: number;
  prima!: number;

  constructor(
    private seguroService: SeguroService,
    private dialog: MatDialog,
    private errorService: ErrorHandlingService
  ) { }

  crearSeguro() {
    this.seguro = {
      nombre: this.nombre,
      codigo: this.codigo,
      suma: this.suma,
      prima: this.prima
    }
    this.seguroService.postSeguro(this.seguro)
    .subscribe((response) => {
      this.abrirAlerta("Seguro creado correctamente")
    }, (error: HttpErrorResponse) => {
      const errorMessage = this.errorService.formatError(error);
      this.abrirAlerta(errorMessage);
    });
  }

  abrirAlerta(mensaje: string): void {
    const dialogRef = this.dialog.open(AlertmodalComponent, {
      width: '300px',
      data: { mensaje }
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('Alerta cerrada');
    });
  }
}