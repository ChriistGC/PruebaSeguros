import { Component, OnInit } from '@angular/core';
import { Cliente } from '../../interface/seguros.interface';
import { SeguroService } from '../../services/seguro.service';

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

  constructor(private seguroService:SeguroService){}

  buscarSeguro(){
    this.seguroService.getSeguro(this.termino)
      .subscribe(seguro=>{
        this.clientes = seguro.clientes
      })
  } 

}
