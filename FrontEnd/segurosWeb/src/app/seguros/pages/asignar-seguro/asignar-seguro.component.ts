import { Component, OnInit } from '@angular/core';
import { Cliente, ClienteSeguro, Seguro } from '../../interface/seguros.interface';
import { SeguroService } from '../../services/seguro.service';
import { EditarusuariomodalComponent } from "../../component/modal/editarusuariomodal/editarusuariomodal.component";
import { EliminarmodalComponent } from "../../component/modal/eliminarmodal/eliminarmodal.component";
import { MatDialog } from '@angular/material/dialog';
import { AlertmodalComponent } from '../../component/modal/alertmodal/alertmodal.component';
import { ErrorHandlingService } from '../../services/error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-asignar-seguro',
  templateUrl: './asignar-seguro.component.html',
  styles: []
})
export class AsignarSeguroComponent implements OnInit {
  clientes: Cliente[] = [];
  seguros: Seguro[] = [];
  seguroSelected: any[] = [];
  clienteSeguros!: Seguro[];
  seguroId: number[] = [];
  datos!: ClienteSeguro;
  cliente!: Cliente;

  segurosSeleccionados: { [nombreSeguro: string]: boolean } = {};

  constructor(
    private seguroService: SeguroService,
    private dialog: MatDialog,
    private errorService: ErrorHandlingService) { }

  ngOnInit(): void {
    this.obtenerClientes()
  }

  obtenerClientes() {
    this.seguroService.getClientes()
      .subscribe(clientes => this.clientes = clientes);

    this.seguroService.getSeguros()
      .subscribe(seguro => this.seguros = seguro);
  }

  panelCliente(cliente: Cliente) {
    this.cliente = cliente;
    this.clienteSeguros = []
    this.seguroService.getCliente(cliente.cedula)
      .subscribe(client => {
        this.clienteSeguros = client.seguros;
        this.seguros.forEach(seguro => {
          this.segurosSeleccionados[seguro.nombre] = this.clienteSeguros.some(clienteSeguro => clienteSeguro.nombre === seguro.nombre);
        }, (error: HttpErrorResponse) => {
          const errorMessage = this.errorService.formatError(error);
          this.abrirAlerta(errorMessage);
        });
      })
  }

  toggleSeleccion(nombreSeguro: string) {
    console.log(this.segurosSeleccionados[nombreSeguro])
    this.segurosSeleccionados[nombreSeguro] = !this.segurosSeleccionados[nombreSeguro];
  }

  asignarSeguro() {
    this.seguroId = [];
    this.seguroSelected = [];
    const nombresSegurosSeleccionados = Object.keys(this.segurosSeleccionados)
      .filter(nombreSeguro => this.segurosSeleccionados[nombreSeguro]);

    nombresSegurosSeleccionados.forEach(nombreSeguro => {
      let idSeguro = this.seguros.find(seguro => seguro.nombre === nombreSeguro);
      this.seguroSelected.push(idSeguro)
    })

    for (let i of this.seguroSelected) {
      this.seguroId.push(i.id!);
    }


    this.datos = {
      id: this.cliente.id!,
      listaSeguros: this.seguroId
    }

    this.seguroService.postAsignar(this.datos)
      .subscribe((response) => {
        this.abrirAlerta("Seguros Asignados Correctamente")
      }, (error: HttpErrorResponse) => {
        const errorMessage = this.errorService.formatError(error);
        this.abrirAlerta(errorMessage);
      });
  }

  abrirModalEditarUsuario(cliente: Cliente): void {
    const dialogRef = this.dialog.open(EditarusuariomodalComponent, {
      width: '270px',
      data: cliente
    });

    dialogRef.afterClosed().subscribe(client => {
      this.seguroService.putUsuario(client.id!, client)
        .subscribe((response) => {
          this.obtenerClientes()
          this.abrirAlerta("Cliente actualizado correctamente")
        }, (error: HttpErrorResponse) => {
          const errorMessage = this.errorService.formatError(error);
          this.abrirAlerta(errorMessage);
        });
    });
  }

  abrirConfirmacionEliminacion(cliente: Cliente): void {
    const mensaje = '¿Estás seguro de que deseas eliminar esta fila?';
    const dialogRef = this.dialog.open(EliminarmodalComponent, {
      width: '300px',
      data: { mensaje }
    });

    dialogRef.afterClosed().subscribe(confirmacion => {
      if (confirmacion) {
        this.seguroService.deleteCliente(cliente.id!)
          .subscribe((response) => {
            this.clientes = this.clientes.filter(c => c.id !== cliente.id)
            this.abrirAlerta("Cliente eliminado")
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
