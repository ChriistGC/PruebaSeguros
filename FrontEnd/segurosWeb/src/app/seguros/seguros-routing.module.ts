import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgregarClienteComponent } from './pages/agregar-cliente/agregar-cliente.component';
import { AgregarSeguroComponent } from './pages/agregar-seguro/agregar-seguro.component';
import { HomeComponent } from './pages/home/home.component';
import { AsignarSeguroComponent } from './pages/asignar-seguro/asignar-seguro.component';
import { ListadoComponent } from './pages/listado/listado.component';
import { BuscarClienteComponent } from './pages/buscar-cliente/buscar-cliente.component';
import { BuscarSeguroComponent } from './pages/buscar-seguro/buscar-seguro.component';

const routes: Routes = [
  {
    path:'',
    component: HomeComponent,
    children:[
      {
        path:'cliente',
        component:AgregarClienteComponent
      },
      {
        path:'seguro',
        component:AgregarSeguroComponent
      },
      {
        path:'asignar',
        component:AsignarSeguroComponent
      },
      {
        path:'lista',
        component:ListadoComponent
      },
      {
        path:"buscarcliente",
        component:BuscarClienteComponent
      },
      {
        path:"buscarseguro",
        component:BuscarSeguroComponent
      },
      {
        path:'**',
        redirectTo:'lista'
      }
    ]
  }
]


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports:[RouterModule]
})
export class SegurosRoutingModule { }
