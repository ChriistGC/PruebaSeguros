import { Component, OnInit } from '@angular/core';
import { SeguroService } from '../../services/seguro.service';
import { Seguro } from '../../interface/seguros.interface';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ErrorHandlingService } from '../../services/error-handling.service';
import { AlertmodalComponent } from '../../component/modal/alertmodal/alertmodal.component';

@Component({
  selector: 'app-buscar-cliente',
  templateUrl: './buscar-cliente.component.html',
  styles: [
  ]
})
export class BuscarClienteComponent implements OnInit{
  
  termino: string = '';
  seguros:Seguro[]=[];

  constructor(
    private seguroService:SeguroService,
    private dialog: MatDialog,
    private errorService: ErrorHandlingService
    ){}
  ngOnInit(): void {}

  buscarCliente(){
    this.seguroService.getCliente(this.termino)
      .subscribe(cliente=>{
        this.seguros = cliente.seguros
      }, (error:HttpErrorResponse)=>{
        console.log(error.error)
        const errorMessage = this.errorService.formatError(error);
        this.abrirAlerta(errorMessage);
      })
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
