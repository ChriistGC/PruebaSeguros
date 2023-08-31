import { Component } from '@angular/core';
import { Cliente } from '../../interface/seguros.interface';
import { SeguroService } from '../../services/seguro.service';
import { MatDialog } from '@angular/material/dialog';
import { AlertmodalComponent } from '../../component/modal/alertmodal/alertmodal.component';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandlingService } from '../../services/error-handling.service';

@Component({
  selector: 'app-agregar-cliente',
  templateUrl: './agregar-cliente.component.html',
  styles: [
  ]
})
export class AgregarClienteComponent {
  cliente!: Cliente;
  nombre!: string;
  cedula!: string;
  telefono!: string;
  edad!: number;
  file: File | null = null;
  clientes: Cliente[] = [];

  constructor(
    private seguroService: SeguroService,
    private dialog: MatDialog,
    private errorService: ErrorHandlingService) { }

  archivoSeleccionado(event: any): void {
    this.file = event.target.files[0];
    this.procesarArchivo();
  }

  crearCliente() {
    this.cliente = {
      nombre: this.nombre,
      cedula: this.cedula,
      telefono: this.telefono,
      edad: this.edad
    }
    this.seguroService.postCliente(this.cliente)
      .subscribe((response) => {
        this.abrirAlerta("Cliente creado correctamente")
      }, (error: HttpErrorResponse) => {
        const errorMessage = this.errorService.formatError(error);
        this.abrirAlerta(errorMessage);
      });
  }

  procesarArchivo() {
    if (!this.file) {
      console.log('No se ha seleccionado un archivo.');
      return;
    }

    const fileReader = new FileReader();
    fileReader.onload = () => {
      const fileExtension = this.getFileExtension(this.file!.name);
      if (fileExtension === 'xlsx') {
        this.seguroService.postFile(this.file!)
          .subscribe((response) => {
            this.abrirAlerta("Datos subidos correctamente")
          }, (error: HttpErrorResponse) => {
            const errorMessage = this.errorService.formatError(error);
            this.abrirAlerta(errorMessage);
          });
      } else {
        this.abrirAlerta('Tipo de archivo no soportado.')
      }
    };
    fileReader.readAsArrayBuffer(this.file);
  }

  getFileExtension(filename: string): string {
    return filename.split('.').pop()!.toLowerCase();
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
