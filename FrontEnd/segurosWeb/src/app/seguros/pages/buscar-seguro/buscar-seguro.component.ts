import { Component, OnInit } from '@angular/core';
import { Cliente } from '../../interface/seguros.interface';
import { SeguroService } from '../../services/seguro.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ErrorHandlingService } from '../../services/error-handling.service';
import { AlertmodalComponent } from '../../component/modal/alertmodal/alertmodal.component';

@Component({
  selector: 'app-buscar-seguro',
  templateUrl: './buscar-seguro.component.html',
  styles: [
  ]
})
export class BuscarSeguroComponent implements OnInit{
  termino: string = '';
  clientes:Cliente[]=[];
  ngOnInit(): void {}

  constructor(
    private seguroService:SeguroService,
    private dialog: MatDialog,
    private errorService: ErrorHandlingService,
    ){}

  buscarSeguro(){
    this.seguroService.getSeguro(this.termino)
      .subscribe(seguro=>{
        this.clientes = seguro.clientes
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
