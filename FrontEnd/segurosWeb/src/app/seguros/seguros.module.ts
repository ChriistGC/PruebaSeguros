import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SegurosRoutingModule } from './seguros-routing.module';
import { MaterialModule } from '../material/material.module';
import { FormsModule } from "@angular/forms";

//Componentes
import { AgregarClienteComponent } from './pages/agregar-cliente/agregar-cliente.component';
import { AgregarSeguroComponent } from './pages/agregar-seguro/agregar-seguro.component';
import { AsignarSeguroComponent } from './pages/asignar-seguro/asignar-seguro.component';
import { HomeComponent } from './pages/home/home.component';
import { ListadoComponent } from './pages/listado/listado.component';
import { BuscarClienteComponent } from './pages/buscar-cliente/buscar-cliente.component';
import { BuscarSeguroComponent } from './pages/buscar-seguro/buscar-seguro.component';
import { EditarusuariomodalComponent } from './component/modal/editarusuariomodal/editarusuariomodal.component';
import { AlertmodalComponent } from './component/modal/alertmodal/alertmodal.component';
import { EditarseguromodalComponent } from './component/modal/editarseguromodal/editarseguromodal.component';
import { EliminarmodalComponent } from './component/modal/eliminarmodal/eliminarmodal.component';



@NgModule({
  declarations: [
    AgregarClienteComponent,
    AgregarSeguroComponent,
    AsignarSeguroComponent,
    HomeComponent,
    ListadoComponent,
    BuscarClienteComponent,
    BuscarSeguroComponent,
    EditarusuariomodalComponent,
    AlertmodalComponent,
    EditarseguromodalComponent,
    EliminarmodalComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    SegurosRoutingModule,
    MaterialModule
  ]
})
export class EventosModule { }
