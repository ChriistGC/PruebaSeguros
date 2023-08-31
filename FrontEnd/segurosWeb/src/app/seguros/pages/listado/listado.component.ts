import { Component, OnInit } from '@angular/core';
import { Seguro } from '../../interface/seguros.interface';
import { SeguroService } from '../../services/seguro.service';
import { EditarseguromodalComponent } from "../../component/modal/editarseguromodal/editarseguromodal.component";
import { EliminarmodalComponent } from "../../component/modal/eliminarmodal/eliminarmodal.component";
import { MatDialog } from '@angular/material/dialog';
import { AlertmodalComponent } from '../../component/modal/alertmodal/alertmodal.component';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandlingService } from '../../services/error-handling.service';

@Component({
  selector: 'app-listado',
  templateUrl: './listado.component.html',
  styleUrls: ['./listado.component.css']
})
export class ListadoComponent implements OnInit {
  seguros: Seguro[] = [];

  constructor(
    private seguroService: SeguroService, 
    private dialog: MatDialog,
    private errorService: ErrorHandlingService) { }

  ngOnInit(): void {
    this.obtenerSeguros()
  }

  obtenerSeguros() {
    this.seguroService.getSeguros()
      .subscribe(seguro => this.seguros = seguro);
  }

  abrirModalEditarSeguro(seguro: Seguro): void {
    const dialogRef = this.dialog.open(EditarseguromodalComponent, {
      width: '270px',
      data: seguro
    });

    dialogRef.afterClosed().subscribe(seguro => {
      this.seguroService.putSeguro(seguro.id!, seguro)
        .subscribe((response) => {
          this.obtenerSeguros()
          this.abrirAlerta("Seguro actualizado correctamente")
        }, (error: HttpErrorResponse) => {
          const errorMessage = this.errorService.formatError(error);
          this.abrirAlerta(errorMessage);
        });
    });
  }

  abrirConfirmacionEliminacion(seguro: Seguro): void {
    const mensaje = '¿Estás seguro de que deseas eliminar este seguro?';
    const dialogRef = this.dialog.open(EliminarmodalComponent, {
      width: '300px',
      data: { mensaje }
    });

    dialogRef.afterClosed().subscribe(confirmacion => {
      if (confirmacion) {
        this.seguroService.deleteSeguro(seguro.id!)
          .subscribe((response) => {
            this.seguros = this.seguros.filter(s => s.id !== seguro.id)
            this.abrirAlerta("Seguro eliminado")
          }, (error: HttpErrorResponse) => {
            const errorMessage = this.errorService.formatError(error);
            this.abrirAlerta(errorMessage);
          });
      }
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
