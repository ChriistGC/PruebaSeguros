import { Component, OnInit } from '@angular/core';
import { SeguroService } from '../../services/seguro.service';
import { Seguro } from '../../interface/seguros.interface';

@Component({
  selector: 'app-buscar-cliente',
  templateUrl: './buscar-cliente.component.html',
  styles: [
  ]
})
export class BuscarClienteComponent implements OnInit{
  
  termino: string = '';
  seguros:Seguro[]=[];

  constructor(private seguroService:SeguroService){}
  ngOnInit(): void {}

  buscarCliente(){
    this.seguroService.getCliente(this.termino)
      .subscribe(cliente=>{
        this.seguros = cliente.seguros
      })
  }

}
